using BunningsProductCatalog.Domain.Models;
using BunningsProductCatalog.Services.Data.Common;
using System.Collections.Generic;

namespace BunningsProductCatalog.Services.Companies
{
	public interface ICompanyService
	{
		IEnumerable<Error> ValidateCompanyExist(string companyCode);
	}
}
