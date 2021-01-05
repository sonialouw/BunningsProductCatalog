using BunningsProductCatalog.Domain.Repository;
using BunningsProductCatalog.Services.Common;
using BunningsProductCatalog.Services.Data.Common;
using BunningsProductCatalog.Services.Data.Companies.Errors;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace BunningsProductCatalog.Services.Companies
{
	public class CompanyService : BaseService, ICompanyService
	{
		public CompanyService(IUnitOfWork uow, ILogger<ICompanyService> logger) : base(uow, logger)
		{

		}

		public IEnumerable<Error> ValidateCompanyExist(string companyCode)
		{
			var newErrors = new List<Error>();

			if (!string.IsNullOrEmpty(companyCode) && UoW.Companies.GetByCompanyCode(companyCode) == null)
			{
				newErrors.Add(new CompanyCodeNotFoundError(companyCode));
			}

			return newErrors;
		}
	}
}
