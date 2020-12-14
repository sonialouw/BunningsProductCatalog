using BunningsProductCatalog.Services.Data.CompanyProducts.Requests;

namespace BunningsProductCatalog.Services.Tests.CompanyProducts
{
	public static class TestData
	{
		public static readonly string CompanyCodeA = "A";
		public static readonly string CompanyCodeB = "B";
		public static readonly string ExistingSupplierCode = "00002";
		public static readonly string ProductSkuCompanyCodeA = "647-vyk-317";
		public static readonly string ProductSkuCompanyCodeB = "280-oad-768";
		public static readonly string ExistingBarcode = "280-oad-768";

		public static CreateCompanyProductRequest GetCreateCompanyProductRequest()
		{
			return new CreateCompanyProductRequest
			{
				ProductSku = "280-oad-768",
				ProductName = "00001",
				CompanyCode = CompanyCodeA,
			};
		}

		public static DeleteCompanyProductRequest GetDeleteCompanyProductRequest()
		{
			return new DeleteCompanyProductRequest
			{
				ProductSku = ProductSkuCompanyCodeA,
				CompanyCode = CompanyCodeA,
			};
		}
	}
}
