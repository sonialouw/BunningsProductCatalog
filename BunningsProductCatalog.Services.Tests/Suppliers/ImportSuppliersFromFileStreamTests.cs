using BunningsProductCatalog.Domain.Models;
using BunningsProductCatalog.Services.Data.Common;
using BunningsProductCatalog.Services.Data.Suppliers.Dto;
using BunningsProductCatalog.Services.Data.Suppliers.Requests;
using CsvHelper.Configuration;
using Moq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BunningsProductCatalog.Services.Data.Companies.Errors;
using Xunit;

namespace BunningsProductCatalog.Services.Tests.Suppliers
{
	public class ImportSuppliersFromFileStreamTests : BaseSupplierTests
	{
		[Trait("Category", "unit")]
		[Fact]
		public void CompanyCodeIsEmpty()
		{
			// Setup
			ImportSupplierCsvService.Setup(m => m.GetRecords(It.IsAny<Stream>(), It.IsAny<ClassMap<ImportSupplierDto>>()))
				.Returns(new List<ImportSupplierDto>()
			{
				new ImportSupplierDto
				{
					SupplierName = "Supplier 1000",
					SupplierCode = "1000"
				},
				new ImportSupplierDto
				{
					SupplierName = "Supplier 2000",
					SupplierCode = "2000"
				},
			});

			UoW.Setup(m => m.Companies.GetByCompanyCode(It.IsAny<string>())).Returns((Company)null);

			using var writer = new StreamWriter(new MemoryStream());
			var result = Subject.ImportSuppliersFromFileStream(new ImportSupplierRequest() { FileStream = writer.BaseStream });

			// Assert
			Assert.Contains(typeof(RequiredFieldMissingError),
				result.Errors.Where(m => m.Field == "CompanyCode").Select(e => e.GetType()).ToList());
		}

		[Trait("Category", "unit")]
		[Fact]
		public void CompanyCodeNotFound()
		{
			var companyCode = "123";

			// Setup
			ImportSupplierCsvService.Setup(m => m.GetRecords(It.IsAny<Stream>(),  It.IsAny<ClassMap<ImportSupplierDto>>()))
				.Returns(new List<ImportSupplierDto>()
			{
				new ImportSupplierDto
				{
					SupplierName = "Supplier 1000",
					SupplierCode = "1000"
				},
				new ImportSupplierDto
				{
					SupplierName = "Supplier 2000",
					SupplierCode = "2000"
				},
			});

			UoW.Setup(m => m.Companies.GetByCompanyCode(It.IsAny<string>())).Returns((Company)null);
			CompanyService.Setup(m => m.ValidateCompanyExist(It.IsAny<string>())).Returns(new List<Error>() { new CompanyCodeNotFoundError(companyCode) });

			using var writer = new StreamWriter(new MemoryStream());
			var result = Subject.ImportSuppliersFromFileStream(new ImportSupplierRequest() { FileStream = writer.BaseStream, CompanyCode = companyCode });

			// Assert
			Assert.Contains(typeof(CompanyCodeNotFoundError),
				result.Errors.Where(m => m.Field == "CompanyCode").Select(e => e.GetType()).ToList());
		}

		[Trait("Category", "unit")]
		[Fact]
		public void SuppliersAreSaved()
		{
			// Setup
			ImportSupplierCsvService.Setup(m => m.GetRecords(It.IsAny<Stream>(),  It.IsAny<ClassMap<ImportSupplierDto>>()))
				.Returns(new List<ImportSupplierDto>()
			{
				new ImportSupplierDto
				{
					SupplierName = "Supplier 1000",
					SupplierCode = "1000"
				},
				new ImportSupplierDto
				{
					SupplierName = "Supplier 2000",
					SupplierCode = "2000"
				},
			});

			using var writer = new StreamWriter(new MemoryStream());
			var result = Subject.ImportSuppliersFromFileStream(new ImportSupplierRequest() { FileStream = writer.BaseStream, CompanyCode = TestData.CompanyCodeA });

			// Assert
			//Assert
			Assert.True(result.Success);
			UoW.Verify(m => m.Suppliers.Add(It.IsAny<Supplier>()), Times.Exactly(2));
		}


		[Trait("Category", "unit")]
		[Fact]
		public void DuplicateSupplierCodesNotSaved()
		{
			// Setup
			ImportSupplierCsvService.Setup(m => m.GetRecords(It.IsAny<Stream>(), It.IsAny<ClassMap<ImportSupplierDto>>()))
				.Returns(new List<ImportSupplierDto>()
			{
				new ImportSupplierDto
				{
					SupplierName = "Supplier 1000",
					SupplierCode = TestData.ExistingSupplierCode
				},
				new ImportSupplierDto
				{
					SupplierName = "Supplier 2000",
					SupplierCode = "2000"
				},
			});

			using var writer = new StreamWriter(new MemoryStream());
			var result = Subject.ImportSuppliersFromFileStream(new ImportSupplierRequest() { FileStream = writer.BaseStream, CompanyCode = TestData.CompanyCodeA });

			// Assert
			Assert.True(result.Success);
			UoW.Verify(m => m.Suppliers.Add(It.IsAny<Supplier>()), Times.Exactly(1));
		}

	}
}
