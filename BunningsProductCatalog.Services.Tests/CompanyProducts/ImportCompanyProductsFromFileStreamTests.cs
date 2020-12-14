using BunningsProductCatalog.Domain.Models;
using BunningsProductCatalog.Services.Data.Common;
using BunningsProductCatalog.Services.Data.CompanyProductBarcodes.Dto;
using BunningsProductCatalog.Services.Data.CompanyProducts.Requests;
using CsvHelper.Configuration;
using Moq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BunningsProductCatalog.Services.Data.Companies.Errors;
using Xunit;

namespace BunningsProductCatalog.Services.Tests.CompanyProducts
{
	public class ImportCompanyProductsFromFileStreamTests : BaseCompanyProductTests
	{
		[Trait("Category", "unit")]
		[Fact]
		public void CompanyCodeIsEmpty()
		{
			// Setup
			ImportCompanyProductCsvService.Setup(m => m.GetRecords(It.IsAny<Stream>(), It.IsAny<ClassMap<ImportCompanyProductDto>>()))
				.Returns(new List<ImportCompanyProductDto>()
			{
				new ImportCompanyProductDto
				{
					ProductName = "ProductName1",
					ProductSku = "ProductSku1"
				},
				new ImportCompanyProductDto
				{
					ProductName = "ProductName2",
					ProductSku = "ProductSku"
				},
			});

			CompanyService.Setup(m => m.GetCompany(It.IsAny<string>())).Returns((Company)null);

			using var writer = new StreamWriter(new MemoryStream());
			var result = Subject.ImportCompanyProductsFromFileStream(new ImportCompanyProductRequest() { FileStream = writer.BaseStream });

			// Assert
			Assert.Contains(typeof(RequiredFieldMissingError),
				result.Errors.Where(m => m.Field == "CompanyCode").Select(e => e.GetType()).ToList());
		}

		[Trait("Category", "unit")]
		[Fact]
		public void CompanyCodeNotFound()
		{
			var companyCode = "123";

			ImportCompanyProductCsvService.Setup(m => m.GetRecords(It.IsAny<Stream>(), It.IsAny<ClassMap<ImportCompanyProductDto>>()))
					.Returns(new List<ImportCompanyProductDto>()
				{
				new ImportCompanyProductDto
				{
					ProductName = "ProductName1",
					ProductSku = "ProductSku1"
				},
				new ImportCompanyProductDto
				{
					ProductName = "ProductName2",
					ProductSku = "ProductSku"
				},
				});

			CompanyService.Setup(m => m.GetCompany(It.IsAny<string>())).Returns((Company)null);
			CompanyService.Setup(m => m.ValidateCompanyExist(It.IsAny<string>())).Returns(new List<Error>() { new CompanyCodeNotFoundError(companyCode) });

			using var writer = new StreamWriter(new MemoryStream());
			var result = Subject.ImportCompanyProductsFromFileStream(new ImportCompanyProductRequest() { FileStream = writer.BaseStream, CompanyCode = companyCode });

			// Assert
			Assert.Contains(typeof(CompanyCodeNotFoundError),
				result.Errors.Where(m => m.Field == "CompanyCode").Select(e => e.GetType()).ToList());
		}

		[Trait("Category", "unit")]
		[Fact]
		public void CompanyProductsAreSaved()
		{
			// Setup
			ImportCompanyProductCsvService.Setup(m => m.GetRecords(It.IsAny<Stream>(), It.IsAny<ClassMap<ImportCompanyProductDto>>()))
					.Returns(new List<ImportCompanyProductDto>()
				{
				new ImportCompanyProductDto
				{
					ProductName = "ProductName1",
					ProductSku = "ProductSku1"
				},
				new ImportCompanyProductDto
				{
					ProductName = "ProductName2",
					ProductSku = "ProductSku"
				},
				});

			using var writer = new StreamWriter(new MemoryStream());
			var result = Subject.ImportCompanyProductsFromFileStream(new ImportCompanyProductRequest() { FileStream = writer.BaseStream, CompanyCode = TestData.CompanyCodeA });

			// Assert
			Assert.True(result.Success);
			UoW.Verify(m => m.CompanyProducts.Add(It.IsAny<CompanyProduct>()), Times.Exactly(2));
		}


		[Trait("Category", "unit")]
		[Fact]
		public void ExistingProductSkusAreNotSaved()
		{
			// Setup
			ImportCompanyProductCsvService.Setup(m => m.GetRecords(It.IsAny<Stream>(), It.IsAny<ClassMap<ImportCompanyProductDto>>()))
					.Returns(new List<ImportCompanyProductDto>()
				{
				new ImportCompanyProductDto
				{
					ProductName = "ProductName1",
					ProductSku = TestData.ProductSkuCompanyCodeA
				},
				new ImportCompanyProductDto
				{
					ProductName = "ProductName2",
					ProductSku = "ProductSku"
				},
				});

			using var writer = new StreamWriter(new MemoryStream());
			var result = Subject.ImportCompanyProductsFromFileStream(new ImportCompanyProductRequest() { FileStream = writer.BaseStream, CompanyCode = TestData.CompanyCodeA });

			//Assert
			Assert.True(result.Success);
			UoW.Verify(m => m.CompanyProducts.Add(It.IsAny<CompanyProduct>()), Times.Exactly(1));
		}

	}
}
