using BunningsProductCatalog.Domain.Models;

namespace BunningsProductCatalog.Domain.Repository
{
	public interface ISupplierRepository : IRepository<Supplier>
	{
		Supplier GetBySupplierCodeAndCompanyCode(string supplierCode, string companyCode);
	}
}
