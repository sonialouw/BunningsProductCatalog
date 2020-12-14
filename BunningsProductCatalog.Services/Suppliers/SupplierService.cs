using BunningsProductCatalog.Domain.Models;
using BunningsProductCatalog.Domain.Repository;
using BunningsProductCatalog.Services.Common;
using BunningsProductCatalog.Services.Companies;
using BunningsProductCatalog.Services.Csv;
using BunningsProductCatalog.Services.Data.Common;
using BunningsProductCatalog.Services.Data.Suppliers.Dto;
using BunningsProductCatalog.Services.Data.Suppliers.Errors;
using BunningsProductCatalog.Services.Data.Suppliers.Requests;
using BunningsProductCatalog.Services.Data.Suppliers.Results;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using static BunningsProductCatalog.Services.Data.CompanyProducts.Companies.Validations.CompanyValidations;
using static BunningsProductCatalog.Services.Data.Suppliers.Validations.SupplierValidations;

namespace BunningsProductCatalog.Services.Suppliers
{
	public class SupplierService : BaseService, ISupplierService
	{
		private ICsvService<ImportSupplierDto> ImportSupplierCsvService { get; }
		private ICompanyService CompanyService { get; }

		public SupplierService(IUnitOfWork uow, ILogger<ISupplierService> logger, ICsvService<ImportSupplierDto> importSupplierCsvService,
			ICompanyService companyService) : base(uow, logger)
		{
			ImportSupplierCsvService = importSupplierCsvService;
			CompanyService = companyService;
		}

		public ImportSupplierResult ImportSuppliersFromFileStream(ImportSupplierRequest request)
		{
			var result = new ImportSupplierResult();

			try
			{
				result.Errors.AddRange(ValidateCompanyCodeRequired(request.CompanyCode));
				result.Errors.AddRange(CompanyService.ValidateCompanyExist(request.CompanyCode));

				if (result.Errors.Any())
				{
					return result;
				}

				var records = ImportSupplierCsvService.GetRecords(request.FileStream, new ImportSupplierDtoClassMap());
				records.ForEach(supplier =>
				{
					var existingSupplier = GetSupplier(supplier.SupplierCode, request.CompanyCode);
					if (existingSupplier == null)
					{
						var createSupplierResult = CreateSupplier(new CreateSupplierRequest() { SupplierName = supplier.SupplierName, SupplierCode = supplier.SupplierCode, CompanyCode = request.CompanyCode });
						if (!createSupplierResult.Success)
						{
							result.Errors.AddRange(createSupplierResult.Errors);
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

		public CreateSupplierResult CreateSupplier(CreateSupplierRequest request)
		{
			var result = new CreateSupplierResult();

			try
			{
				result.Errors.AddRange(ValidateSupplierNameRequired(request.SupplierName));
				result.Errors.AddRange(ValidateSupplierNameMaxLength(request.SupplierName));
				result.Errors.AddRange(ValidateCompanyCodeRequired(request.CompanyCode));
				result.Errors.AddRange(CompanyService.ValidateCompanyExist(request.CompanyCode));
				result.Errors.AddRange(ValidateSupplierCodeRequired(request.SupplierCode));
				result.Errors.AddRange(ValidateSupplierCodeMaxLength(request.SupplierCode));
				result.Errors.AddRange(ValidateDuplicateSupplier(request.SupplierCode, request.CompanyCode));

				if (result.Errors.Any())
				{
					return result;
				}

				var company = CompanyService.GetCompany(request.CompanyCode);
				var newSupplier = new Supplier
				{
					SupplierCode = request.SupplierCode.ToUpper().Trim(),
					SupplierName = request.SupplierName.Trim(),
					CompanyId = company.CompanyId,
					CreatedDateUtc = DateTime.UtcNow,
					ModifiedDateUtc = DateTime.UtcNow
				};

				UoW.Suppliers.Add(newSupplier);
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

		public Supplier GetSupplier(string supplierCode, string companyCode)
		{
			return UoW.Suppliers.GetAll().FirstOrDefault(i => i.Company.CompanyCode.Trim().ToUpper() == companyCode.Trim().ToUpper()
			  && i.SupplierCode.Trim().ToUpper() == supplierCode.Trim().ToUpper() );
		}

		public IEnumerable<Error> ValidateSupplierExist(string supplierCode, string companyCode)
		{
			var newErrors = new List<Error>();

			if (!string.IsNullOrEmpty(companyCode) && !string.IsNullOrEmpty(companyCode) && GetSupplier(supplierCode, companyCode) == null)
			{
				newErrors.Add(new SupplierCodeNotFoundError(supplierCode, companyCode));
			}

			return newErrors;
		}

		private IEnumerable<Error> ValidateDuplicateSupplier(string supplierCode, string companyCode)
		{
			var newErrors = new List<Error>();

			if (!string.IsNullOrEmpty(companyCode) && !string.IsNullOrEmpty(supplierCode) && GetSupplier(supplierCode, companyCode) != null)
			{
				newErrors.Add(new DuplicateSupplierCodeFoundError(supplierCode, companyCode));
			}

			return newErrors;
		}



	}
}
