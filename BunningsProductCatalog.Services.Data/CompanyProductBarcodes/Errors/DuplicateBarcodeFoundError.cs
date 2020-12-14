using BunningsProductCatalog.Services.Data.Common;

namespace BunningsProductCatalog.Services.Data.CompanyProductBarcodes.Errors
{
	public class DuplicateBarcodeFoundError : Error
	{
		public DuplicateBarcodeFoundError(string barcode, string supplierCode, string productSku, string companyCode)
		{
			Field = "Barcode";
			Message = $"Barcode {barcode} for product SKU {productSku}, supplier code {supplierCode} and company code {companyCode} alreadye exist.";
		}
	}
}