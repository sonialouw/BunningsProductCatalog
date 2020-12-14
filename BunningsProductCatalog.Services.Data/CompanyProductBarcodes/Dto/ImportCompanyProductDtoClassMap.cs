using BunningsProductCatalog.Services.Data.CompanyProductBarcodes.Dto;
using CsvHelper.Configuration;

namespace BunningsProductCatalog.Services.Data.CompanyProducts.Dto
{
	public sealed class ImportCompanyProductDtoClassMap : ClassMap<ImportCompanyProductDto>
	{
		public ImportCompanyProductDtoClassMap()
		{
			Map(m => m.ProductName).Name("Description");
			Map(m => m.ProductSku).Name("SKU");
		}
	}
}
