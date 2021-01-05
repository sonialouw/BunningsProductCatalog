using BunningsProductCatalog.Domain.Models;
using BunningsProductCatalog.Domain.Repository;
using Microsoft.EntityFrameworkCore;
using System.Linq;


namespace BunningsProductCatalog.Repository
{
	public class CompanyProductRepository : EFRepository<CompanyProduct>, ICompanyProductRepository
	{
		public CompanyProductRepository(DbContext dbContext) : base(dbContext)
		{
		}

		public CompanyProduct GetBySkuAndCompanyCode(string productSku, string companyCode)
		{
			return DbSet.FirstOrDefault(i => i.Company.CompanyCode.Trim().ToUpper() == companyCode.Trim().ToUpper()
			                                 && i.ProductSku.Trim().ToUpper() == productSku.Trim().ToUpper()
			                                 && !i.IsDeleted);
		}

	
	}
}
