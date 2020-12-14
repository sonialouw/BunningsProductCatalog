using BunningsProductCatalog.Services.Data.Products.Results;

namespace BunningsProductCatalog.Services.Products
{
	public interface IProductService
	{
		GetProductCatalogResult GetProductCatalog();
		GenerateProductCatalogFileResult GenerateProductCatalogFile();
	}
}
