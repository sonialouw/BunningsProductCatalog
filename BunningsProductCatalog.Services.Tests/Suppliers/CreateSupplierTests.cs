using BunningsProductCatalog.Domain.Models;
using BunningsProductCatalog.Services.Data.Common;
using BunningsProductCatalog.Services.Data.Suppliers.Errors;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using BunningsProductCatalog.Services.Data.Companies.Errors;
using Xunit;

namespace BunningsProductCatalog.Services.Tests.Suppliers
{
	public class CreateSupplierTests: BaseSupplierTests
	{
		[Trait("Category", "unit")]
		[Fact]
		public void SupplierNameIsEmpty()
		{
			// Assemble
			var request = TestData.GetCreateSupplierRequest();
			request.SupplierName = null;

			// Act
			var result = Subject.CreateSupplier(request);

			// Assert
			Assert.Contains(typeof(RequiredFieldMissingError),
				result.Errors.Where(m => m.Field == "SupplierName").Select(e => e.GetType()).ToList());
		}

		[Trait("Category", "unit")]
		[Fact]
		public void SupplierNameIsLongerThanMaxLength()
		{
			// Assemble
			var request = TestData.GetCreateSupplierRequest();
			request.SupplierName = "A4qGmLBhaWXIlr7fqwpKRMiKjXmbUQJezvLJivVyCKCYTiAiYZbsHVgpO1gzS1czhbzS5JlfmgHt6U66ifHWPwJeZq5TTiBB3QCAtYw3fL1AQ9TSchhVZ8vXj8F7wNaLIGZa5UuEics8onv7XAxLwV0YnI7la6gUMyK42p30gNqHyuN4sVlqQMfOdQKyN4pEtvNJX6idcWeHvrd3gyCdIuUNMNJGGR6FblI3d7r2Rvode8muRaNVVdrPqZdqMVmDEGcrCHLWYD6QZ8zyOhjvHeClIHpwBwor0dzEdGFTANXYi";

			// Act
			var result = Subject.CreateSupplier(request);

			// Assert
			Assert.Contains(typeof(InvalidStringMaxLengthError),
				result.Errors.Where(m => m.Field == "SupplierName").Select(e => e.GetType()).ToList());
		}

		[Trait("Category", "unit")]
		[Fact]
		public void SupplierCodeIsEmpty()
		{
			// Assemble
			var request = TestData.GetCreateSupplierRequest();
			request.SupplierCode = null;

			// Act
			var result = Subject.CreateSupplier(request);

			// Assert
			Assert.Contains(typeof(RequiredFieldMissingError),
				result.Errors.Where(m => m.Field == "SupplierCode").Select(e => e.GetType()).ToList());
		}

		[Trait("Category", "unit")]
		[Fact]
		public void SupplierCodeIsLongerThanMaxLength()
		{
			// Assemble
			var request = TestData.GetCreateSupplierRequest();
			request.SupplierCode = "i6b4eqj2c2Nz9GdKvRR6eG0cWQJttG3GgH08P9aAH2h9rjixK73";

			// Act
			var result = Subject.CreateSupplier(request);

			// Assert
			Assert.Contains(typeof(InvalidStringMaxLengthError),
				result.Errors.Where(m => m.Field == "SupplierCode").Select(e => e.GetType()).ToList());
		}

		[Trait("Category", "unit")]
		[Fact]
		public void CompanyCodeIsEmpty()
		{
			// Assemble
			var request = TestData.GetCreateSupplierRequest();
			request.CompanyCode = null;

			CompanyService.Setup(m => m.GetCompany(It.IsAny<string>())).Returns((Company)null);

			// Act
			var result = Subject.CreateSupplier(request);

			// Assert
			Assert.Contains(typeof(RequiredFieldMissingError),
				result.Errors.Where(m => m.Field == "CompanyCode").Select(e => e.GetType()).ToList());
		}

		[Trait("Category", "unit")]
		[Fact]
		public void CompanyCodeNotFound()
		{
			// Assemble
			var request = TestData.GetCreateSupplierRequest();
			request.CompanyCode = "C";

			CompanyService.Setup(m => m.GetCompany(It.IsAny<string>())).Returns((Company)null);
			CompanyService.Setup(m => m.ValidateCompanyExist(It.IsAny<string>())).Returns(new List<Error>() { new CompanyCodeNotFoundError(request.CompanyCode) });

			// Act
			var result = Subject.CreateSupplier(request);

			// Assert
			Assert.False(result.Success);
			Assert.Contains(typeof(CompanyCodeNotFoundError),
				result.Errors.Where(m => m.Field == "CompanyCode").Select(e => e.GetType()).ToList());
		}

		[Trait("Category", "unit")]
		[Fact]
		public void DuplicateSupplierCodeForSameCompanyFound()
		{
			// Assemble
			var request = TestData.GetCreateSupplierRequest();
			request.SupplierCode = TestData.ExistingSupplierCode;

			// Act
			var result = Subject.CreateSupplier(request);

			// Assert
			Assert.False(result.Success);
			Assert.Contains(typeof(DuplicateSupplierCodeFoundError),
				result.Errors.Where(m => m.Field == "SupplierCode").Select(e => e.GetType()).ToList());
		}

		[Trait("Category", "unit")]
		[Fact]
		public void DuplicateSupplierCodeNotForSameCompanySuccess()
		{
			// Assemble
			var request = TestData.GetCreateSupplierRequest();
			request.SupplierCode = TestData.ExistingSupplierCode;
			request.CompanyCode = TestData.CompanyCodeB;

			// Act
			var result = Subject.CreateSupplier(request);

			// Assert
			Assert.True(result.Success);
		}

		[Trait("Category", "unit")]
		[Fact]
		public void ExceptionErrorIsReturned()
		{
			// Assemble
			var request = TestData.GetCreateSupplierRequest();

			UoW.Setup(m => m.Save())
				.Throws<Exception>();

			// Act
			var result = Subject.CreateSupplier(request);

			// Assert
			Assert.Contains(typeof(ExceptionError), result.Errors.Select(e => e.GetType()).ToList());
		}

		[Trait("Category", "unit")]
		[Fact]
		public void SupplierIsSaved()
		{
			// Assemble
			var request = TestData.GetCreateSupplierRequest();

			// Act
			var result = Subject.CreateSupplier(request);

			// Assert
			Assert.True(result.Success);
			UoW.Verify(m => m.Save(), Times.AtLeastOnce);
		}


		[Trait("Category", "unit")]
		[Fact]
		public void ValidateSupplierExist()
		{
			// Act
			var result = Subject.ValidateSupplierExist(TestData.ExistingSupplierCode, TestData.CompanyCodeA);

			// Assert
			Assert.Empty(result);
		}

		[Trait("Category", "unit")]
		[Fact]
		public void ValidateSupplierDoesNotExist()
		{
			UoW.Setup(m => m.Suppliers.GetAll()).Returns(new List<Supplier>{}.AsQueryable());

			// Act
			var result = Subject.ValidateSupplierExist(TestData.ExistingSupplierCode, TestData.CompanyCodeA);

			// Assert
			Assert.Contains(typeof(SupplierCodeNotFoundError),
				result.Where(m => m.Field == "SupplierCode").Select(e => e.GetType()).ToList());
		}

		[Trait("Category", "unit")]
		[Fact]
		public void GetSupplierFound()
		{
			// Act
			var result = Subject.GetSupplier(TestData.ExistingSupplierCode, TestData.CompanyCodeA);

			// Assert
			Assert.NotNull(result);
		}

		[Trait("Category", "unit")]
		[Fact]
		public void GetSupplierNotFound()
		{
			UoW.Setup(m => m.Suppliers.GetAll()).Returns(new List<Supplier> { }.AsQueryable());

			// Act
			var result = Subject.GetSupplier(TestData.ExistingSupplierCode, TestData.CompanyCodeA);

			// Assert
			Assert.Null(result);
		}

	}
}
