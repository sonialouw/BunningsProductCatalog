using BunningsProductCatalog.Domain.Models;
using BunningsProductCatalog.Domain.Repository;
using Microsoft.EntityFrameworkCore;
using System.Linq;


namespace BunningsProductCatalog.Repository
{
	public class SupplierRepository : EFRepository<Supplier>, ISupplierRepository
	{
		public SupplierRepository(DbContext dbContext) : base(dbContext)
		{
		}

		public Supplier GetBySupplierCodeAndCompanyCode(string supplierCode, string companyCode)
		{
			return DbSet.FirstOrDefault(i => i.Company.CompanyCode.Trim().ToUpper() == companyCode.Trim().ToUpper()
			                                 && i.SupplierCode.Trim().ToUpper() == supplierCode.Trim().ToUpper());
		}
	}
}
