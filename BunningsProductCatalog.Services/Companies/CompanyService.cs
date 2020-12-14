using BunningsProductCatalog.Domain.Models;
using BunningsProductCatalog.Services.Common;
using BunningsProductCatalog.Services.Data.Common;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using BunningsProductCatalog.Domain.Repository;
using BunningsProductCatalog.Services.Data.Companies.Errors;

namespace BunningsProductCatalog.Services.Companies
{
	public class CompanyService : BaseService, ICompanyService
	{
		public CompanyService(IUnitOfWork uow, ILogger<ICompanyService> logger) : base(uow, logger)
		{

		}

		public Company GetCompany(string companyCode)
		{
			return UoW.Companies.GetAll().FirstOrDefault(i => i.CompanyCode.Trim().ToUpper() == companyCode.Trim().ToUpper());
		}

		public IEnumerable<Error> ValidateCompanyExist(string companyCode)
		{
			var newErrors = new List<Error>();

			if (!string.IsNullOrEmpty(companyCode) && GetCompany(companyCode) == null)
			{
				newErrors.Add(new CompanyCodeNotFoundError(companyCode));
			}

			return newErrors;
		}
	}
}
