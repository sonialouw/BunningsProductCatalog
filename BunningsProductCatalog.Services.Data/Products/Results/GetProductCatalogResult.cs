using BunningsProductCatalog.Services.Data.Common;
using BunningsProductCatalog.Services.Data.Products.Dto;
using System.Collections.Generic;

namespace BunningsProductCatalog.Services.Data.Products.Results
{
	public class GetProductCatalogResult : BaseResult
	{
		public List<ProductCatalogDto> ProductCatalog { get; set; }
	}
}

