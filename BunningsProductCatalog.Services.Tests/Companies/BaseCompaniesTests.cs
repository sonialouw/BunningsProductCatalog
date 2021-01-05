using BunningsProductCatalog.Domain.Models;
using BunningsProductCatalog.Domain.Repository;
using BunningsProductCatalog.Services.Companies;
using BunningsProductCatalog.Services.Csv;
using BunningsProductCatalog.Services.Data.Common;
using BunningsProductCatalog.Services.Data.Suppliers.Dto;
using BunningsProductCatalog.Services.Suppliers;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace BunningsProductCatalog.Services.Tests.Companies
{
	public abstract class BaseCompaniesTests
	{
		protected ICompanyService Subject { get; }
		protected Mock<ILogger<ICompanyService>> Logger { get; }
		protected Mock<IUnitOfWork> UoW { get; }


		public BaseCompaniesTests()
		{
			UoW = new Mock<IUnitOfWork>();
			Logger = new Mock<ILogger<ICompanyService>>();
			Subject = new CompanyService(UoW.Object, Logger.Object);

			var companyA = new Company
			{
				CompanyCode = TestData.CompanyCodeA,
				CompanyName = "Company A"
			};

			var companyB = new Company
			{
				CompanyCode = TestData.CompanyCodeB,
				CompanyName = "Company A"
			};

			UoW.Setup(m => m.Companies.GetAll()).Returns(new List<Company>
			{
				companyA,
				companyB
			}.AsQueryable());

			UoW.Setup(m => m.Companies.GetByCompanyCode(TestData.CompanyCodeA)).Returns(companyA);
			UoW.Setup(m => m.Companies.GetByCompanyCode(TestData.CompanyCodeB)).Returns(companyB);
		}


	}
}
