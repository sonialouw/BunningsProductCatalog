using BunningsProductCatalog.Services.Data.Products.Dto;
using CsvHelper.Configuration;
using Moq;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace BunningsProductCatalog.Services.Tests.Products
{
	public class GenerateProductCatalogFileTests : BaseProductTests
	{
		[Trait("Category", "unit")]
		[Fact]
		public void FileGenerated()
		{
			//Setup
			var csv = new StringBuilder();
			csv.AppendLine($"SKU,Description,Source");
			csv.AppendLine($"ProductSku1,ProductName1,A");
			csv.AppendLine($"ProductSku3,ProductName3,B");

			ProductCatalogCsvService.Setup(m => m.WriteRecords(It.IsAny<List<ProductCatalogDto>>(), true, It.IsAny<ClassMap<ProductCatalogDto>>(), ",", false))
				.Returns(csv.ToString());

			// Act
			var result = Subject.GenerateProductCatalogFile();

			//Assert
			Assert.True(result.Success);
			Assert.Equal(result.FileContents, csv.ToString());
		}
	}
}
