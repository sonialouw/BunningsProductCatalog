using BunningsProductCatalog.Domain.Models;
using BunningsProductCatalog.Services.Common;
using BunningsProductCatalog.Services.Companies;
using BunningsProductCatalog.Services.CompanyProducts;
using BunningsProductCatalog.Services.Csv;
using BunningsProductCatalog.Services.Data.Common;
using BunningsProductCatalog.Services.Data.CompanyProductBarcodes.Errors;
using BunningsProductCatalog.Services.Data.CompanyProductBarcodes.Requests;
using BunningsProductCatalog.Services.Data.CompanyProductBarcodes.Results;
using BunningsProductCatalog.Services.Data.CompanyProducts.Dto;
using BunningsProductCatalog.Services.Suppliers;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using BunningsProductCatalog.Domain.Repository;
using static BunningsProductCatalog.Services.Data.CompanyProducts.Companies.Validations.CompanyValidations;
using static BunningsProductCatalog.Services.Data.CompanyProducts.Validations.CompanyProductValidations;
using static BunningsProductCatalog.Services.Data.Suppliers.Validations.SupplierValidations;

namespace BunningsProductCatalog.Services.CompanyProductBarcodes
{
	public class CompanyProductBarcodeService : BaseService, ICompanyProductBarcodeService
	{
		private ICsvService<ImportCompanyProductBarcodeDto> ImportCompanyProductBarcodeCsvService { get; }
		private ICompanyService CompanyService { get; }
		private ISupplierService SupplierService { get; }
		private ICompanyProductService CompanyProductService { get; }

		public CompanyProductBarcodeService(IUnitOfWork uow, ILogger<ICompanyProductBarcodeService> logger,
			ICsvService<ImportCompanyProductBarcodeDto> importCompanyProductBarcodeCsvService, ICompanyProductService companyProductService,
			ISupplierService supplierService, ICompanyService companyService) : base(uow, logger)
		{
			SupplierService = supplierService;
			CompanyService = companyService;
			CompanyProductService = companyProductService;
			ImportCompanyProductBarcodeCsvService = importCompanyProductBarcodeCsvService;
		}

		public ImportCompanyProductBarcodeResult ImportCompanyProductBarcodesFromFileStream(ImportCompanyProductBarcodeRequest request)
		{
			var result = new ImportCompanyProductBarcodeResult();

			try
			{
				// validate
				result.Errors.AddRange(ValidateCompanyCodeRequired(request.CompanyCode));
				result.Errors.AddRange(CompanyService.ValidateCompanyExist(request.CompanyCode));
				if (result.Errors.Any())
				{
					return result;
				}

				// get all records
				var records = ImportCompanyProductBarcodeCsvService.GetRecords(request.FileStream, new ImportCompanyProductBarcodeDtoClassMap());
				records.ForEach(barcode =>
				{
					// check if barcode exist
					var existingCompanyProductBarcode = GetCompanyProductBarcode(barcode.Barcode, barcode.SupplierCode, barcode.ProductSku, request.CompanyCode);
					if (existingCompanyProductBarcode == null)
					{
						// create product
						var createCompanyProductBarcodeResult = CreateCompanyProductBarcode(new CreateCompanyProductBarcodeRequest() { Barcode = barcode.Barcode, ProductSku = barcode.ProductSku, SupplierCode = barcode.SupplierCode, CompanyCode = request.CompanyCode });
						if (!createCompanyProductBarcodeResult.Success)
						{
							result.Errors.AddRange(createCompanyProductBarcodeResult.Errors);
						}
					}
				});
			}
			catch (Exception e)
			{
				result.Success = false;
				result.Errors.Add(new ExceptionError(e.Message));
			}

			result.Success = !result.Errors.Any();

			return result;
		}

		public CreateCompanyProductBarcodeResult CreateCompanyProductBarcode(CreateCompanyProductBarcodeRequest request)
		{
			var result = new CreateCompanyProductBarcodeResult();

			try
			{
				// validate
				result.Errors.AddRange(ValidateProductSkuRequired(request.ProductSku));
				result.Errors.AddRange(CompanyProductService.ValidateCompanyProductExist(request.ProductSku, request.CompanyCode));
				result.Errors.AddRange(ValidateSupplierCodeRequired(request.SupplierCode));
				result.Errors.AddRange(SupplierService.ValidateSupplierExist(request.SupplierCode, request.CompanyCode));
				result.Errors.AddRange(ValidateCompanyCodeRequired(request.CompanyCode));
				result.Errors.AddRange(CompanyService.ValidateCompanyExist(request.CompanyCode));
				result.Errors.AddRange(ValidateDuplicateCompanyProductBarcode(request.Barcode, request.SupplierCode, request.ProductSku, request.CompanyCode));

				if (result.Errors.Any())
				{
					return result;
				}

				var supplier = UoW.Suppliers.GetBySupplierCodeAndCompanyCode(request.SupplierCode, request.CompanyCode);
				var companyProduct = UoW.CompanyProducts.GetBySkuAndCompanyCode(request.ProductSku, request.CompanyCode);

				// create product
				var newCompanyProductBarcode = new CompanyProductBarcode
				{
					SupplierId = supplier.SupplierId,
					CompanyProductId = companyProduct.CompanyProductId,
					Barcode = request.Barcode.ToLower().Trim(),
					CreatedDateUtc = DateTime.UtcNow,
					ModifiedDateUtc = DateTime.UtcNow
				};

				UoW.CompanyProductBarcodes.Add(newCompanyProductBarcode);
				UoW.Save();

				result.Success = true;
			}
			catch (Exception e)
			{
				result.Success = false;
				result.Errors.Add(new ExceptionError(e.Message));
			}

			return result;
		}

		private CompanyProductBarcode GetCompanyProductBarcode(string barcode, string supplierCode, string productSku, string companyCode)
		{
			var cp = UoW.CompanyProductBarcodes.GetAll().FirstOrDefault(i => i.Barcode == barcode
			&& i.Supplier.SupplierCode.Trim().ToUpper() == supplierCode.Trim().ToUpper()
			&& i.Supplier.Company.CompanyCode.Trim().ToUpper() == companyCode.Trim().ToUpper()
			&& i.CompanyProduct.ProductSku.Trim().ToUpper() == productSku.Trim().ToUpper()
			&& i.CompanyProduct.Company.CompanyCode.Trim().ToUpper() == companyCode.Trim().ToUpper());

			return cp;
		}

		private IEnumerable<Error> ValidateDuplicateCompanyProductBarcode(string barcode, string supplierCode, string productSku, string companyCode)
		{
			var newErrors = new List<Error>();

			if (!string.IsNullOrEmpty(companyCode) && !string.IsNullOrEmpty(supplierCode) && GetCompanyProductBarcode(barcode, supplierCode, productSku, companyCode) != null)
			{
				newErrors.Add(new DuplicateBarcodeFoundError(barcode, supplierCode, productSku, companyCode));
			}

			return newErrors;
		}




	}
}
