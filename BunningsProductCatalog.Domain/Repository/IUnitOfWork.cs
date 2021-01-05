using BunningsProductCatalog.Domain.Models;
using System;

namespace BunningsProductCatalog.Domain.Repository
{
	public interface IUnitOfWork : IDisposable
	{
		ICompanyRepository Companies { get; }
		ISupplierRepository Suppliers { get; }
		ICompanyProductRepository CompanyProducts { get; }
		ICompanyProductBarcodeRepository CompanyProductBarcodes { get; }

		void Save();
	}
}