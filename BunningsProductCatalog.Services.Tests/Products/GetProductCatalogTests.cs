using BunningsProductCatalog.Services.Data.Products.Dto;
using CsvHelper.Configuration;
using Moq;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace BunningsProductCatalog.Services.Tests.Products
{
	public class GetProductCatalogTests : BaseProductTests
	{
		[Trait("Category", "unit")]
		[Fact]
		public void GetProductCatalog()
		{
			// Act
			var result = Subject.GetProductCatalog();

			//Assert
			Assert.True(result.Success);
			Assert.NotNull(result.ProductCatalog);
			Assert.True(result.ProductCatalog.Count == 2);

			Assert.True(result.ProductCatalog[0].ProductSku == "ProductSku1");
			Assert.True(result.ProductCatalog[0].ProductName == "ProductName1");
			Assert.True(result.ProductCatalog[0].CompanyCode == "A");

			Assert.True(result.ProductCatalog[1].ProductSku == "ProductSku3");
			Assert.True(result.ProductCatalog[1].ProductName == "ProductName3");
			Assert.True(result.ProductCatalog[1].CompanyCode == "B");
		}


	}
}
