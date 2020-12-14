using BunningsProductCatalog.Services.Data.Common;

namespace BunningsProductCatalog.Services.Data.CompanyProducts.Errors
{
	public class DuplicateProductSkuFoundError : Error
	{
		public DuplicateProductSkuFoundError(string productSku, string companyCode)
		{
			Field = "ProductSku";
			Message = $"Product with SKU of {productSku} for company code {companyCode} already exist.";
		}
	}
}