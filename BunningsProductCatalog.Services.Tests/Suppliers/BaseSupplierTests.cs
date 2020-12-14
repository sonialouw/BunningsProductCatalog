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

namespace BunningsProductCatalog.Services.Tests.Suppliers
{
	public abstract class BaseSupplierTests
	{
		protected ISupplierService Subject { get; }
		protected Mock<ICsvService<ImportSupplierDto>> ImportSupplierCsvService { get; }
		protected Mock<ILogger<ISupplierService>> Logger { get; }
		protected Mock<IUnitOfWork> UoW { get; }
		protected Mock<ICompanyService> CompanyService { get; }

		public BaseSupplierTests()
		{
			UoW = new Mock<IUnitOfWork>();
			Logger = new Mock<ILogger<ISupplierService>>();
			ImportSupplierCsvService = new Mock<ICsvService<ImportSupplierDto>>();
			CompanyService = new Mock<ICompanyService>();

			Subject = new SupplierService(UoW.Object, Logger.Object, ImportSupplierCsvService.Object, CompanyService.Object);

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

			CompanyService.Setup(m => m.GetCompany(TestData.CompanyCodeA)).Returns(companyA);
			CompanyService.Setup(m => m.GetCompany(TestData.CompanyCodeB)).Returns(companyB);
			CompanyService.Setup(m => m.ValidateCompanyExist(It.IsAny<string>())).Returns(new List<Error>());

			var supplier = new Supplier
			{
				SupplierCode = TestData.ExistingSupplierCode,
				SupplierName = "SupplierName2",
				Company = companyA
			};

			UoW.Setup(m => m.Suppliers.GetAll()).Returns(new List<Supplier>
			{
				supplier
			}.AsQueryable());

		}


	}
}
