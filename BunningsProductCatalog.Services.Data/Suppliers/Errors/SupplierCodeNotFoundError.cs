using BunningsProductCatalog.Services.Data.Common;

namespace BunningsProductCatalog.Services.Data.Suppliers.Errors
{
	public class SupplierCodeNotFoundError : Error
	{
		public SupplierCodeNotFoundError(string supplierCode, string companyCode)
		{
			Field = "SupplierCode";
			Message = $"Supplier with code of {supplierCode} for company code {companyCode}  not found.";
		}
	}
}