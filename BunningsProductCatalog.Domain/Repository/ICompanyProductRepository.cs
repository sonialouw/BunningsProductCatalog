using System;
using System.Collections.Generic;
using System.Text;
using BunningsProductCatalog.Domain.Models;

namespace BunningsProductCatalog.Domain.Repository
{
	public interface ICompanyProductRepository : IRepository<CompanyProduct>
	{
		CompanyProduct GetBySkuAndCompanyCode(string productSku, string companyCode);
	}
}
