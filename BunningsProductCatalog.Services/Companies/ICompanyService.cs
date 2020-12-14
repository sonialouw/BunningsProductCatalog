using BunningsProductCatalog.Domain.Models;
using BunningsProductCatalog.Services.Data.Common;
using System.Collections.Generic;

namespace BunningsProductCatalog.Services.Companies
{
	public interface ICompanyService
	{
		Company GetCompany(string companyCode);
		IEnumerable<Error> ValidateCompanyExist(string companyCode);
	}
}
