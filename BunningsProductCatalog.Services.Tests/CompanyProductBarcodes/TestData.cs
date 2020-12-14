using BunningsProductCatalog.Services.Data.CompanyProductBarcodes.Requests;

namespace BunningsProductCatalog.Services.Tests.CompanyProductBarcodes
{
	public static class TestData
	{
		public static readonly string CompanyCodeA = "A";
		public static readonly string CompanyCodeB = "B";
		public static readonly string SupplierCode = "00002";
		public static readonly string ProductSkuCompanyCodeA = "647-vyk-317";
		public static readonly string ProductSkuCompanyCodeB = "280-oad-768";
		public static readonly string BarcodeProductSkuCompanyCodeA = "c7417468772846";

		public static CreateCompanyProductBarcodeRequest GetCreateCompanyProductBarcodeRequest()
		{
			return new CreateCompanyProductBarcodeRequest
			{
				ProductSku = ProductSkuCompanyCodeA,
				SupplierCode = SupplierCode,
				CompanyCode = CompanyCodeA,
				Barcode = "a5056026479965"
			};
		}


	}
}
