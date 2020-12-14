using BunningsProductCatalog.Services.Data.Suppliers.Requests;

namespace BunningsProductCatalog.Services.Tests.Suppliers
{
	public static class TestData
	{
		public static readonly string CompanyCodeA = "A";
		public static readonly string CompanyCodeB = "B";
		public static readonly string ExistingSupplierCode = "00002";
		public static readonly string ExistingProductSku = "647-vyk-317";
		public static readonly string ExistingProductSku2 = "280-oad-768";
		public static readonly string ExistingBarcode = "280-oad-768";

		public static CreateSupplierRequest GetCreateSupplierRequest()
		{
			return new CreateSupplierRequest
			{
				SupplierName = "Twitter bridge",
				SupplierCode = "00001",
				CompanyCode = CompanyCodeA,
			};
		}
	}
}
