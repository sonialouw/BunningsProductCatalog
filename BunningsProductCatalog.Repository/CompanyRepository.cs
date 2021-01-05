using BunningsProductCatalog.Domain.Models;
using BunningsProductCatalog.Domain.Repository;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BunningsProductCatalog.Repository
{
	public class CompanyRepository : EFRepository<Company>, ICompanyRepository
	{
		public CompanyRepository(DbContext dbContext) : base(dbContext)
		{
		}

		public Company GetByCompanyCode(string companyCode)
		{
			return DbSet.FirstOrDefault(i => i.CompanyCode.Trim().ToUpper() == companyCode.Trim().ToUpper());
		}

	}
}
