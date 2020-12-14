using BunningsProductCatalog.Services.Data.Common;

namespace BunningsProductCatalog.Services.Data.Suppliers.Errors
{
	public class DuplicateSupplierCodeFoundError : Error
	{
		public DuplicateSupplierCodeFoundError(string supplierCode, string companyCode)
		{
			Field = "SupplierCode";
			Message = $"Supplier with code of {supplierCode} for company code {companyCode} already exist.";
		}
	}
}