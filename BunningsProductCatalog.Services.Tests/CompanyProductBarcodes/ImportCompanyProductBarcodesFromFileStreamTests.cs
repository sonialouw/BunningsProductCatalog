using BunningsProductCatalog.Domain.Models;
using BunningsProductCatalog.Services.Data.Common;
using BunningsProductCatalog.Services.Data.CompanyProductBarcodes.Requests;
using BunningsProductCatalog.Services.Data.CompanyProducts.Dto;
using CsvHelper.Configuration;
using Moq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BunningsProductCatalog.Services.Data.Companies.Errors;
using Xunit;

namespace BunningsProductCatalog.Services.Tests.CompanyProductBarcodes
{
	public class ImportCompanyProductBarcodesFromFileStreamTests : BaseCompanyProductBarcodeTests
	{
		[Trait("Category", "unit")]
		[Fact]
		public void CompanyCodeIsEmpty()
		{
			// Setup
			ImportCompanyProductBarcodeCsvService.Setup(m => m.GetRecords(It.IsAny<Stream>(), It.IsAny<ClassMap<ImportCompanyProductBarcodeDto>>()))
				.Returns(new List<ImportCompanyProductBarcodeDto>()
			{
				new ImportCompanyProductBarcodeDto
				{
					SupplierCode = TestData.SupplierCode,
					ProductSku =  TestData.ProductSkuCompanyCodeA,
					Barcode = "Barcode1"
				},
				new ImportCompanyProductBarcodeDto
				{
					SupplierCode = TestData.SupplierCode,
					ProductSku =  TestData.ProductSkuCompanyCodeA,
					Barcode = "Barcode2"
				},
			});

			using var writer = new StreamWriter(new MemoryStream());
			var result = Subject.ImportCompanyProductBarcodesFromFileStream(new ImportCompanyProductBarcodeRequest() { FileStream = writer.BaseStream });

			// Assert
			Assert.Contains(typeof(RequiredFieldMissingError),
				result.Errors.Where(m => m.Field == "CompanyCode").Select(e => e.GetType()).ToList());
		}

		[Trait("Category", "unit")]
		[Fact]
		public void CompanyCodeNotFound()
		{
			ImportCompanyProductBarcodeCsvService.Setup(m => m.GetRecords(It.IsAny<Stream>(), It.IsAny<ClassMap<ImportCompanyProductBarcodeDto>>()))
				.Returns(new List<ImportCompanyProductBarcodeDto>()
			{
				new ImportCompanyProductBarcodeDto
				{
					SupplierCode = TestData.SupplierCode,
					ProductSku =  TestData.ProductSkuCompanyCodeA,
					Barcode = "Barcode1"
				},
				new ImportCompanyProductBarcodeDto
				{
					SupplierCode = TestData.SupplierCode,
					ProductSku =  TestData.ProductSkuCompanyCodeA,
					Barcode = "Barcode2"
				},
			});

			var companyCode = "123";

			UoW.Setup(m => m.Companies.GetByCompanyCode(It.IsAny<string>())).Returns((Company)null);
			CompanyService.Setup(m => m.ValidateCompanyExist(It.IsAny<string>())).Returns(new List<Error>() { new CompanyCodeNotFoundError(companyCode) });

			using var writer = new StreamWriter(new MemoryStream());
			var result = Subject.ImportCompanyProductBarcodesFromFileStream(new ImportCompanyProductBarcodeRequest() { FileStream = writer.BaseStream, CompanyCode = companyCode });

			// Assert
			Assert.Contains(typeof(CompanyCodeNotFoundError),
				result.Errors.Where(m => m.Field == "CompanyCode").Select(e => e.GetType()).ToList());
		}

		[Trait("Category", "unit")]
		[Fact]
		public void CompanyProductBarcodesAreSaved()
		{
			// Setup
			ImportCompanyProductBarcodeCsvService.Setup(m => m.GetRecords(It.IsAny<Stream>(), It.IsAny<ClassMap<ImportCompanyProductBarcodeDto>>()))
					.Returns(new List<ImportCompanyProductBarcodeDto>()
				{
				new ImportCompanyProductBarcodeDto
				{
					SupplierCode = TestData.SupplierCode,
					ProductSku =  TestData.ProductSkuCompanyCodeA,
					Barcode = "Barcode1"
				},
				new ImportCompanyProductBarcodeDto
				{
					SupplierCode = TestData.SupplierCode,
					ProductSku =  TestData.ProductSkuCompanyCodeA,
					Barcode = "Barcode2"
				},
				});

			using var writer = new StreamWriter(new MemoryStream());
			var result = Subject.ImportCompanyProductBarcodesFromFileStream(new ImportCompanyProductBarcodeRequest() { FileStream = writer.BaseStream, CompanyCode = TestData.CompanyCodeA });

			// Assert
			Assert.True(result.Success);
			UoW.Verify(m => m.CompanyProductBarcodes.Add(It.IsAny<CompanyProductBarcode>()), Times.Exactly(2));
		}


		[Trait("Category", "unit")]
		[Fact]
		public void DuplicateBarcodesAreNotSaved()
		{
			// Setup
			ImportCompanyProductBarcodeCsvService.Setup(m => m.GetRecords(It.IsAny<Stream>(), It.IsAny<ClassMap<ImportCompanyProductBarcodeDto>>()))
			.Returns(new List<ImportCompanyProductBarcodeDto>()
			{
					new ImportCompanyProductBarcodeDto
					{
						SupplierCode = TestData.SupplierCode,
						ProductSku =  TestData.ProductSkuCompanyCodeA,
						Barcode = TestData.BarcodeProductSkuCompanyCodeA
					},
					new ImportCompanyProductBarcodeDto
					{
						SupplierCode = TestData.SupplierCode,
						ProductSku =  TestData.ProductSkuCompanyCodeA,
						Barcode = "Barcode2"
					},
			});


			using var writer = new StreamWriter(new MemoryStream());
			var result = Subject.ImportCompanyProductBarcodesFromFileStream(new ImportCompanyProductBarcodeRequest() { FileStream = writer.BaseStream, CompanyCode = TestData.CompanyCodeA });

			//Assert
			Assert.True(result.Success);
			UoW.Verify(m => m.CompanyProductBarcodes.Add(It.IsAny<CompanyProductBarcode>()), Times.Exactly(1));
		}

	}
}
