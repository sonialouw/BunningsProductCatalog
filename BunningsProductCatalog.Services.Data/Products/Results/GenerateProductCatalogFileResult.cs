using BunningsProductCatalog.Services.Data.Common;

namespace BunningsProductCatalog.Services.Data.Products.Results
{
	public class GenerateProductCatalogFileResult: BaseResult
	{
		public string FileContents { get; set; }

		public string FileName { get; set; }
	}
}

