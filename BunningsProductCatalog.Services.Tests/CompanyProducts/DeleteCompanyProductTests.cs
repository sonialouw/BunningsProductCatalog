using BunningsProductCatalog.Domain.Models;
using BunningsProductCatalog.Services.Data.Common;
using BunningsProductCatalog.Services.Data.CompanyProducts.Errors;
using Moq;
using System;
using System.Linq;
using Xunit;

namespace BunningsProductCatalog.Services.Tests.CompanyProducts
{
	public class DeleteCompanyProductTests : BaseCompanyProductTests
	{
		

		[Trait("Category", "unit")]
		[Fact]
		public void ProductSkuIsEmpty()
		{
			// Assemble
			var request = TestData.GetDeleteCompanyProductRequest();
			request.ProductSku = null;

			// Act
			var result = Subject.DeleteCompanyProduct(request);

			// Assert
			Assert.Contains(typeof(RequiredFieldMissingError),
				result.Errors.Where(m => m.Field == "ProductSku").Select(e => e.GetType()).ToList());
		}

		[Trait("Category", "unit")]
		[Fact]
		public void CompanyCodeIsEmpty()
		{
			// Assemble
			var request = TestData.GetDeleteCompanyProductRequest();
			request.CompanyCode = null;

			UoW.Setup(m => m.Companies.GetByCompanyCode(It.IsAny<string>())).Returns((Company)null);

			// Act
			var result = Subject.DeleteCompanyProduct(request);

			// Assert
			Assert.Contains(typeof(RequiredFieldMissingError),
				result.Errors.Where(m => m.Field == "CompanyCode").Select(e => e.GetType()).ToList());
		}

		[Trait("Category", "unit")]
		[Fact]
		public void ProductSkuNotFound()
		{
			// Assemble
			var request = TestData.GetDeleteCompanyProductRequest();
			request.ProductSku = "ABC";

			// Act
			var result = Subject.DeleteCompanyProduct(request);

			// Assert
			Assert.False(result.Success);
			Assert.Contains(typeof(ProductSkuNotFoundError),
				result.Errors.Where(m => m.Field == "ProductSku").Select(e => e.GetType()).ToList());
		}

		[Trait("Category", "unit")]
		[Fact]
		public void ExceptionErrorIsReturned()
		{
			// Assemble
			var request = TestData.GetDeleteCompanyProductRequest();

			UoW.Setup(m => m.Save())
				.Throws<Exception>();

			// Act
			var result = Subject.DeleteCompanyProduct(request);

			// Assert
			Assert.Contains(typeof(ExceptionError), result.Errors.Select(e => e.GetType()).ToList());
		}

		[Trait("Category", "unit")]
		[Fact]
		public void CompanyProductIsSaved()
		{
			// Assemble
			var request = TestData.GetDeleteCompanyProductRequest();

			// Act
			var result = Subject.DeleteCompanyProduct(request);

			// Assert
			Assert.True(result.Success);
			UoW.Verify(m => m.Save(), Times.AtLeastOnce);
		}
	}
}
