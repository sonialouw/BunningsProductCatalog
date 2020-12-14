using BunningsProductCatalog.Domain.Models;
using BunningsProductCatalog.Services.Data.Common;
using BunningsProductCatalog.Services.Data.CompanyProducts.Errors;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using BunningsProductCatalog.Services.Data.Companies.Errors;
using Xunit;

namespace BunningsProductCatalog.Services.Tests.CompanyProducts
{
	public class CreateCompanyProductTests : BaseCompanyProductTests
	{
		[Trait("Category", "unit")]
		[Fact]
		public void ProductNameIsEmpty()
		{
			// Assemble
			var request = TestData.GetCreateCompanyProductRequest();
			request.ProductName = null;

			// Act
			var result = Subject.CreateCompanyProduct(request);

			// Assert
			Assert.Contains(typeof(RequiredFieldMissingError),
				result.Errors.Where(m => m.Field == "ProductName").Select(e => e.GetType()).ToList());
		}

		[Trait("Category", "unit")]
		[Fact]
		public void SupplierNameIsLongerThanMaxLength()
		{
			// Assemble
			var request = TestData.GetCreateCompanyProductRequest();
			request.ProductName = "A4qGmLBhaWXIlr7fqwpKRMiKjXmbUQJezvLJivVyCKCYTiAiYZbsHVgpO1gzS1czhbzS5JlfmgHt6U66ifHWPwJeZq5TTiBB3QCAtYw3fL1AQ9TSchhVZ8vXj8F7wNaLIGZa5UuEics8onv7XAxLwV0YnI7la6gUMyK42p30gNqHyuN4sVlqQMfOdQKyN4pEtvNJX6idcWeHvrd3gyCdIuUNMNJGGR6FblI3d7r2Rvode8muRaNVVdrPqZdqMVmDEGcrCHLWYD6QZ8zyOhjvHeClIHpwBwor0dzEdGFTANXYi";

			// Act
			var result = Subject.CreateCompanyProduct(request);

			// Assert
			Assert.Contains(typeof(InvalidStringMaxLengthError),
				result.Errors.Where(m => m.Field == "ProductName").Select(e => e.GetType()).ToList());
		}

		[Trait("Category", "unit")]
		[Fact]
		public void ProductSkuIsEmpty()
		{
			// Assemble
			var request = TestData.GetCreateCompanyProductRequest();
			request.ProductSku = null;

			// Act
			var result = Subject.CreateCompanyProduct(request);

			// Assert
			Assert.Contains(typeof(RequiredFieldMissingError),
				result.Errors.Where(m => m.Field == "ProductSku").Select(e => e.GetType()).ToList());
		}

		[Trait("Category", "unit")]
		[Fact]
		public void ProductSkuIsLongerThanMaxLength()
		{
			// Assemble
			var request = TestData.GetCreateCompanyProductRequest();
			request.ProductSku = "i6b4eqj2c2Nz9GdKvRR6eG0cWQJttG3GgH08P9aAH2h9rjixK73";

			// Act
			var result = Subject.CreateCompanyProduct(request);

			// Assert
			Assert.Contains(typeof(InvalidStringMaxLengthError),
				result.Errors.Where(m => m.Field == "ProductSku").Select(e => e.GetType()).ToList());
		}

		[Trait("Category", "unit")]
		[Fact]
		public void CompanyCodeIsEmpty()
		{
			// Assemble
			var request = TestData.GetCreateCompanyProductRequest();
			request.CompanyCode = null;

			CompanyService.Setup(m => m.GetCompany(It.IsAny<string>())).Returns((Company)null);
			
			// Act
			var result = Subject.CreateCompanyProduct(request);

			// Assert
			Assert.Contains(typeof(RequiredFieldMissingError),
				result.Errors.Where(m => m.Field == "CompanyCode").Select(e => e.GetType()).ToList());
		}

		[Trait("Category", "unit")]
		[Fact]
		public void CompanyCodeNotFound()
		{
			// Assemble
			var request = TestData.GetCreateCompanyProductRequest();
			request.CompanyCode = "C";

			CompanyService.Setup(m => m.GetCompany(It.IsAny<string>())).Returns((Company)null);
			CompanyService.Setup(m => m.ValidateCompanyExist(It.IsAny<string>())).Returns(new List<Error>() { new CompanyCodeNotFoundError(request.CompanyCode) });

			// Act
			var result = Subject.CreateCompanyProduct(request);

			// Assert
			Assert.False(result.Success);
			Assert.Contains(typeof(CompanyCodeNotFoundError),
				result.Errors.Where(m => m.Field == "CompanyCode").Select(e => e.GetType()).ToList());
		}

		[Trait("Category", "unit")]
		[Fact]
		public void DuplicateProductSkuForSameCompanyFound()
		{
			// Assemble
			var request = TestData.GetCreateCompanyProductRequest();
			request.ProductSku = TestData.ProductSkuCompanyCodeA;
			request.CompanyCode = TestData.CompanyCodeA;

			// Act
			var result = Subject.CreateCompanyProduct(request);

			// Assert
			Assert.False(result.Success);
			Assert.Contains(typeof(DuplicateProductSkuFoundError),
				result.Errors.Where(m => m.Field == "ProductSku").Select(e => e.GetType()).ToList());
		}

		[Trait("Category", "unit")]
		[Fact]
		public void DuplicateProductSkuNotForSameCompanySuccess()
		{
			// Assemble
			var request = TestData.GetCreateCompanyProductRequest();
			request.ProductSku = TestData.ProductSkuCompanyCodeA;
			request.CompanyCode = TestData.CompanyCodeB;

			// Act
			var result = Subject.CreateCompanyProduct(request);

			// Assert
			Assert.True(result.Success);
		}

		[Trait("Category", "unit")]
		[Fact]
		public void ExceptionErrorIsReturned()
		{
			// Assemble
			var request = TestData.GetCreateCompanyProductRequest();

			UoW.Setup(m => m.Save())
				.Throws<Exception>();

			// Act
			var result = Subject.CreateCompanyProduct(request);

			// Assert
			Assert.Contains(typeof(ExceptionError), result.Errors.Select(e => e.GetType()).ToList());
		}

		[Trait("Category", "unit")]
		[Fact]
		public void CompanyProductIsSaved()
		{
			// Assemble
			var request = TestData.GetCreateCompanyProductRequest();

			// Act
			var result = Subject.CreateCompanyProduct(request);

			// Assert
			Assert.True(result.Success);
			UoW.Verify(m => m.Save(), Times.AtLeastOnce);
		}

		[Trait("Category", "unit")]
		[Fact]
		public void ValidateCompanyProductExist()
		{
			// Act
			var result = Subject.ValidateCompanyProductExist(TestData.ProductSkuCompanyCodeA, TestData.CompanyCodeA);

			// Assert
			Assert.Empty(result);
		}

		[Trait("Category", "unit")]
		[Fact]
		public void ValidateCompanyProductDoesNotExist()
		{
			UoW.Setup(m => m.CompanyProducts.GetAll()).Returns(new List<CompanyProduct> { }.AsQueryable());

			// Act
			var result = Subject.ValidateCompanyProductExist(TestData.ProductSkuCompanyCodeA, TestData.CompanyCodeA);

			// Assert
			Assert.Contains(typeof(ProductSkuNotFoundError),
				result.Where(m => m.Field == "ProductSku").Select(e => e.GetType()).ToList());
		}

		[Trait("Category", "unit")]
		[Fact]
		public void GetCompanyProductFound()
		{
			// Act
			var result = Subject.GetCompanyProduct(TestData.ProductSkuCompanyCodeA, TestData.CompanyCodeA);

			// Assert
			Assert.NotNull(result);
		}

		[Trait("Category", "unit")]
		[Fact]
		public void GetCompanyProductNotFound()
		{
			UoW.Setup(m => m.CompanyProducts.GetAll()).Returns(new List<CompanyProduct> { }.AsQueryable());

			// Act
			var result = Subject.GetCompanyProduct(TestData.ProductSkuCompanyCodeA, TestData.CompanyCodeA);

			// Assert
			Assert.Null(result);
		}

	}
}
