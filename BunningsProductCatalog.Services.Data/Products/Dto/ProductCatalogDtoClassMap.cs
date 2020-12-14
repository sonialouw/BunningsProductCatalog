using CsvHelper.Configuration;

namespace BunningsProductCatalog.Services.Data.Products.Dto
{
	public sealed class ProductCatalogDtoClassMap : ClassMap<ProductCatalogDto>
	{
		public ProductCatalogDtoClassMap()
		{
			Map(m => m.ProductSku).Index(1).Name("SKU");
			Map(m => m.ProductName).Index(2).Name("Description");
			Map(m => m.CompanyCode).Index(3).Name("Source");
		}
	}
}
