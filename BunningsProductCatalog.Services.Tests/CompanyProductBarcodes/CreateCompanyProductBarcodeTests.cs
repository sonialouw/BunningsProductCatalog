using BunningsProductCatalog.Domain.Models;
using BunningsProductCatalog.Services.Data.Common;
using BunningsProductCatalog.Services.Data.CompanyProductBarcodes.Errors;
using BunningsProductCatalog.Services.Data.CompanyProducts.Errors;
using BunningsProductCatalog.Services.Data.Suppliers.Errors;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using BunningsProductCatalog.Services.Data.Companies.Errors;
using Xunit;

namespace BunningsProductCatalog.Services.Tests.CompanyProductBarcodes
{
	public class CreateCompanyProductBarcodeTests : BaseCompanyProductBarcodeTests
	{
		[Trait("Category", "unit")]
		[Fact]
		public void ProductSkuIsEmpty()
		{
			// Assemble
			var request = TestData.GetCreateCompanyProductBarcodeRequest();
			request.ProductSku = null;

			// Act
			var result = Subject.CreateCompanyProductBarcode(request);

			// Assert
			Assert.Contains(typeof(RequiredFieldMissingError),
				result.Errors.Where(m => m.Field == "ProductSku").Select(e => e.GetType()).ToList());
		}

		[Trait("Category", "unit")]
		[Fact]
		public void ProductSkuNotFound()
		{
			// Assemble
			var request = TestData.GetCreateCompanyProductBarcodeRequest();
			request.ProductSku = "ABC";

			CompanyProductService.Setup(m => m.GetCompanyProduct(request.ProductSku, TestData.CompanyCodeA)).Returns((CompanyProduct)null);
			CompanyProductService.Setup(m => m.ValidateCompanyProductExist(request.ProductSku, TestData.CompanyCodeA)).Returns(new List<Error>() { new ProductSkuNotFoundError(request.ProductSku, request.CompanyCode) });

			// Act
			var result = Subject.CreateCompanyProductBarcode(request);

			// Assert
			Assert.False(result.Success);
			Assert.Contains(typeof(ProductSkuNotFoundError),
				result.Errors.Where(m => m.Field == "ProductSku").Select(e => e.GetType()).ToList());
		}


		[Trait("Category", "unit")]
		[Fact]
		public void SupplierCodeIsEmpty()
		{
			// Assemble
			var request = TestData.GetCreateCompanyProductBarcodeRequest();
			request.SupplierCode = null;

			SupplierService.Setup(m => m.GetSupplier(request.SupplierCode, request.CompanyCode)).Returns((Supplier)null);
			SupplierService.Setup(m => m.ValidateSupplierExist(request.SupplierCode, request.CompanyCode)).Returns(new List<Error>() { new RequiredFieldMissingError("SupplierCode", "Supplier code cannot be empty") });

			// Act
			var result = Subject.CreateCompanyProductBarcode(request);

			// Assert
			Assert.Contains(typeof(RequiredFieldMissingError),
				result.Errors.Where(m => m.Field == "SupplierCode").Select(e => e.GetType()).ToList());
		}

		[Trait("Category", "unit")]
		[Fact]
		public void SupplierCodeNotFound()
		{
			// Assemble
			var request = TestData.GetCreateCompanyProductBarcodeRequest();
			request.SupplierCode = "ABC";

			SupplierService.Setup(m => m.GetSupplier(request.SupplierCode, request.CompanyCode)).Returns((Supplier)null);
			SupplierService.Setup(m => m.ValidateSupplierExist(request.SupplierCode, request.CompanyCode)).Returns(new List<Error>() { new SupplierCodeNotFoundError(request.SupplierCode, request.CompanyCode) });

			// Act
			var result = Subject.CreateCompanyProductBarcode(request);

			// Assert
			Assert.False(result.Success);
			Assert.Contains(typeof(SupplierCodeNotFoundError),
				result.Errors.Where(m => m.Field == "SupplierCode").Select(e => e.GetType()).ToList());
		}

		[Trait("Category", "unit")]
		[Fact]
		public void CompanyCodeIsEmpty()
		{
			// Assemble
			var request = TestData.GetCreateCompanyProductBarcodeRequest();
			request.CompanyCode = null;

			// Act
			var result = Subject.CreateCompanyProductBarcode(request);

			// Assert
			Assert.Contains(typeof(RequiredFieldMissingError),
				result.Errors.Where(m => m.Field == "CompanyCode").Select(e => e.GetType()).ToList());
		}

		[Trait("Category", "unit")]
		[Fact]
		public void CompanyCodeNotFound()
		{
			// Assemble
			var request = TestData.GetCreateCompanyProductBarcodeRequest();
			request.CompanyCode = "C";

			CompanyService.Setup(m => m.GetCompany(It.IsAny<string>())).Returns((Company)null);
			CompanyService.Setup(m => m.ValidateCompanyExist(It.IsAny<string>())).Returns(new List<Error>() { new CompanyCodeNotFoundError(request.CompanyCode) });

			// Act
			var result = Subject.CreateCompanyProductBarcode(request);

			// Assert
			Assert.False(result.Success);
			Assert.Contains(typeof(CompanyCodeNotFoundError),
				result.Errors.Where(m => m.Field == "CompanyCode").Select(e => e.GetType()).ToList());
		}

		[Trait("Category", "unit")]
		[Fact]
		public void DuplicateBarcodeFound()
		{
			// Assemble
			var request = TestData.GetCreateCompanyProductBarcodeRequest();
			request.Barcode = TestData.BarcodeProductSkuCompanyCodeA;

			// Act
			var result = Subject.CreateCompanyProductBarcode(request);

			// Assert
			Assert.False(result.Success);
			Assert.Contains(typeof(DuplicateBarcodeFoundError),
				result.Errors.Where(m => m.Field == "Barcode").Select(e => e.GetType()).ToList());
		}

		[Trait("Category", "unit")]
		[Fact]
		public void ExceptionErrorIsReturned()
		{
			// Assemble
			var request = TestData.GetCreateCompanyProductBarcodeRequest();

			UoW.Setup(m => m.Save())
				.Throws<Exception>();

			// Act
			var result = Subject.CreateCompanyProductBarcode(request);

			// Assert
			Assert.Contains(typeof(ExceptionError), result.Errors.Select(e => e.GetType()).ToList());
		}

		[Trait("Category", "unit")]
		[Fact]
		public void CompanyProductBarcodeIsSaved()
		{
			// Assemble
			var request = TestData.GetCreateCompanyProductBarcodeRequest();

			// Act
			var result = Subject.CreateCompanyProductBarcode(request);

			// Assert
			Assert.True(result.Success);
			UoW.Verify(m => m.Save(), Times.AtLeastOnce);
		}
	}
}
