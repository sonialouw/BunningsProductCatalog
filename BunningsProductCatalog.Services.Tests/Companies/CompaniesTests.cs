using BunningsProductCatalog.Domain.Models;
using BunningsProductCatalog.Services.Data.Companies.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace BunningsProductCatalog.Services.Tests.Companies
{
	public class CompaniesTests: BaseCompaniesTests
	{
		[Trait("Category", "unit")]
		[Fact]
		public void ValidateCompanyExist()
		{
			// Act
			var result = Subject.ValidateCompanyExist(TestData.CompanyCodeA);

			// Assert
			Assert.Empty(result);
		}

		[Trait("Category", "unit")]
		[Fact]
		public void ValidateCompanyDoesNotExist()
		{
			UoW.Setup(m => m.Companies.GetAll()).Returns(new List<Company> { }.AsQueryable());

			// Act
			var result = Subject.ValidateCompanyExist(TestData.CompanyCodeA);

			// Assert
			Assert.Contains(typeof(CompanyCodeNotFoundError),
				result.Where(m => m.Field == "CompanyCode").Select(e => e.GetType()).ToList());
		}

		[Trait("Category", "unit")]
		[Fact]
		public void GetCompanyFound()
		{
			// Act
			var result = Subject.GetCompany(TestData.CompanyCodeA);

			// Assert
			Assert.NotNull(result);
		}

		[Trait("Category", "unit")]
		[Fact]
		public void GetCompanyNotFound()
		{
			UoW.Setup(m => m.Companies.GetAll()).Returns(new List<Company> { }.AsQueryable());

			// Act
			var result = Subject.GetCompany(TestData.CompanyCodeA);

			// Assert
			Assert.Null(result);
		}
	}
}
