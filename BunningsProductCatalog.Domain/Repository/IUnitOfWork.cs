using BunningsProductCatalog.Domain.Models;
using System;

namespace BunningsProductCatalog.Domain.Repository
{
	public interface IUnitOfWork : IDisposable
	{
		IRepository<Company> Companies { get; }
		IRepository<Supplier> Suppliers { get; }
		IRepository<CompanyProduct> CompanyProducts { get; }
		IRepository<CompanyProductBarcode> CompanyProductBarcodes { get; }

		void Save();
	}
}