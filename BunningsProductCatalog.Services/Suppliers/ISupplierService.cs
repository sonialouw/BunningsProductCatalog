using BunningsProductCatalog.Domain.Models;
using BunningsProductCatalog.Services.Data.Common;
using BunningsProductCatalog.Services.Data.Suppliers.Requests;
using BunningsProductCatalog.Services.Data.Suppliers.Results;
using System.Collections.Generic;

namespace BunningsProductCatalog.Services.Suppliers
{
	public interface ISupplierService
	{
		ImportSupplierResult ImportSuppliersFromFileStream(ImportSupplierRequest request);
		CreateSupplierResult CreateSupplier(CreateSupplierRequest request);
		IEnumerable<Error> ValidateSupplierExist(string supplierCode, string companyCode);

	}
}
