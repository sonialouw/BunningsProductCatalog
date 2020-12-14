using BunningsProductCatalog.Services.Data.CompanyProducts.Requests;
using CsvHelper.Configuration;

namespace BunningsProductCatalog.Services.Data.CompanyProducts.Dto
{
	public sealed class ImportCompanyProductBarcodeDtoClassMap : ClassMap<ImportCompanyProductBarcodeDto>
	{
		public ImportCompanyProductBarcodeDtoClassMap()
		{
			Map(m => m.SupplierCode).Name("SupplierID");
			Map(m => m.ProductSku).Name("SKU");
			Map(m => m.Barcode).Name("Barcode");
		}
	}
}
