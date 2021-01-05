using BunningsProductCatalog.Domain.Models;
using BunningsProductCatalog.Domain.Repository;
using BunningsProductCatalog.Services.Common;
using BunningsProductCatalog.Services.Companies;
using BunningsProductCatalog.Services.Csv;
using BunningsProductCatalog.Services.Data.Common;
using BunningsProductCatalog.Services.Data.CompanyProductBarcodes.Dto;
using BunningsProductCatalog.Services.Data.CompanyProducts.Dto;
using BunningsProductCatalog.Services.Data.CompanyProducts.Errors;
using BunningsProductCatalog.Services.Data.CompanyProducts.Requests;
using BunningsProductCatalog.Services.Data.CompanyProducts.Results;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using static BunningsProductCatalog.Services.Data.CompanyProducts.Companies.Validations.CompanyValidations;
using static BunningsProductCatalog.Services.Data.CompanyProducts.Validations.CompanyProductValidations;

namespace BunningsProductCatalog.Services.CompanyProducts
{
	public class CompanyProductService : BaseService, ICompanyProductService
	{
		private ICsvService<ImportCompanyProductDto> ImportCompanyProductCsvService { get; }
		private ICompanyService CompanyService { get; }

		public CompanyProductService(IUnitOfWork uow, ILogger<ICompanyProductService> logger,
			ICsvService<ImportCompanyProductDto> importCompanyProductCsvService, ICompanyService companyService) : base(uow, logger)
		{
			ImportCompanyProductCsvService = importCompanyProductCsvService;
			CompanyService = companyService;
		}

		public ImportCompanyProductResult ImportCompanyProductsFromFileStream(ImportCompanyProductRequest request)
		{
			var result = new ImportCompanyProductResult();

			try
			{

				result.Errors.AddRange(ValidateCompanyCodeRequired(request.CompanyCode));
				result.Errors.AddRange(CompanyService.ValidateCompanyExist(request.CompanyCode));

				if (result.Errors.Any())
				{
					return result;
				}

				var records = ImportCompanyProductCsvService.GetRecords(request.FileStream, new ImportCompanyProductDtoClassMap());
				records.ForEach(companyProduct =>
				{
					var existingCompanyProduct = UoW.CompanyProducts.GetBySkuAndCompanyCode(companyProduct.ProductSku, request.CompanyCode);
					if (existingCompanyProduct == null)
					{
						var createCompanyProductResult = CreateCompanyProduct(new CreateCompanyProductRequest() { ProductSku = companyProduct.ProductSku, ProductName = companyProduct.ProductName, CompanyCode = request.CompanyCode });
						if (!createCompanyProductResult.Success)
						{
							result.Errors.AddRange(createCompanyProductResult.Errors);
						}
					}
				});

				var companyProducts = UoW.CompanyProducts.GetAll().Where(i => i.Company.CompanyCode == request.CompanyCode).ToList();
				companyProducts.ForEach(companyProduct =>
				{
					var deletedCompanyProduct = !records.Where(i=>i.ProductSku.Trim().ToUpper() == companyProduct.ProductSku.Trim().ToUpper()).Any();
					if (deletedCompanyProduct)
					{
						var deleteCompanyProductResult = DeleteCompanyProduct(new DeleteCompanyProductRequest() { ProductSku = companyProduct.ProductSku, CompanyCode = request.CompanyCode });
						if (!deleteCompanyProductResult.Success)
						{
							result.Errors.AddRange(deleteCompanyProductResult.Errors);
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

		public CreateCompanyProductResult CreateCompanyProduct(CreateCompanyProductRequest request)
		{
			var result = new CreateCompanyProductResult();

			try
			{

				result.Errors.AddRange(ValidateProductNameRequired(request.ProductName));
				result.Errors.AddRange(ValidateProductNameMaxLength(request.ProductName));
				result.Errors.AddRange(ValidateProductSkuRequired(request.ProductSku));
				result.Errors.AddRange(ValidateProductSkuMaxLength(request.ProductSku));
				result.Errors.AddRange(ValidateCompanyCodeRequired(request.CompanyCode));
				result.Errors.AddRange(CompanyService.ValidateCompanyExist(request.CompanyCode));
				result.Errors.AddRange(ValidateDuplicateCompanyProduct(request.ProductSku, request.CompanyCode));

				if (result.Errors.Any())
				{
					return result;
				}

				var company = UoW.Companies.GetByCompanyCode(request.CompanyCode);
				var newProduct = new CompanyProduct
				{
					ProductSku = request.ProductSku.ToUpper().Trim(),
					ProductName = request.ProductName.Trim(),
					CompanyId = company.CompanyId,
					CreatedDateUtc = DateTime.UtcNow,
					ModifiedDateUtc = DateTime.UtcNow
				};

				UoW.CompanyProducts.Add(newProduct);
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

		private IEnumerable<Error> ValidateDuplicateCompanyProduct(string productSku, string companyCode)
		{
			var newErrors = new List<Error>();

			if (!string.IsNullOrEmpty(companyCode) && !string.IsNullOrEmpty(productSku) && UoW.CompanyProducts.GetBySkuAndCompanyCode(productSku, companyCode) != null)
			{
				newErrors.Add(new DuplicateProductSkuFoundError(productSku, companyCode));
			}

			return newErrors;
		}

		public IEnumerable<Error> ValidateCompanyProductExist(string productSku, string companyCode)
		{
			var newErrors = new List<Error>();

			if (!string.IsNullOrEmpty(companyCode) && !string.IsNullOrEmpty(productSku) && UoW.CompanyProducts.GetBySkuAndCompanyCode(productSku, companyCode) == null)
			{
				newErrors.Add(new ProductSkuNotFoundError(productSku, companyCode));
			}

			return newErrors;
		}

		public DeleteCompanyProductResult DeleteCompanyProduct(DeleteCompanyProductRequest request)
		{ 
			var result = new DeleteCompanyProductResult();

			try
			{
				result.Errors.AddRange(ValidateProductSkuRequired(request.ProductSku));
				result.Errors.AddRange(ValidateCompanyCodeRequired(request.CompanyCode));
				result.Errors.AddRange(ValidateCompanyProductExist(request.ProductSku, request.CompanyCode));

				if (result.Errors.Any())
				{
					return result;
				}

				var companyProduct = UoW.CompanyProducts.GetBySkuAndCompanyCode(request.ProductSku, request.CompanyCode);
				companyProduct.IsDeleted = true;
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

	}
}
