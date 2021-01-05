using BunningsProductCatalog.Domain.Models;

namespace BunningsProductCatalog.Domain.Repository
{
	public interface ICompanyRepository : IRepository<Company>
	{
		Company GetByCompanyCode(string companyCode);
	}
}
