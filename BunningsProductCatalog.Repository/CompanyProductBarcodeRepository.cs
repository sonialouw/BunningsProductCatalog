using BunningsProductCatalog.Domain.Models;
using BunningsProductCatalog.Domain.Repository;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BunningsProductCatalog.Repository
{
	public class CompanyProductBarcodeRepository : EFRepository<CompanyProductBarcode>, ICompanyProductBarcodeRepository
	{
		public CompanyProductBarcodeRepository(DbContext dbContext) : base(dbContext)
		{
		}
	}
}
