using BunningsProductCatalog.Domain.Models;
using BunningsProductCatalog.Domain.Repository;
using BunningsProductCatalog.Services.Companies;
using BunningsProductCatalog.Services.CompanyProducts;
using BunningsProductCatalog.Services.Csv;
using BunningsProductCatalog.Services.Data.Common;
using BunningsProductCatalog.Services.Data.CompanyProductBarcodes.Dto;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace BunningsProductCatalog.Services.Tests.CompanyProducts
{
	public abstract class BaseCompanyProductTests
	{
		protected ICompanyProductService Subject { get; }
		protected Mock<ICsvService<ImportCompanyProductDto>> ImportCompanyProductCsvService { get; }
		protected Mock<ILogger<ICompanyProductService>> Logger { get; }
		protected Mock<IUnitOfWork> UoW { get; }
		protected Mock<ICompanyService> CompanyService { get; }

		public BaseCompanyProductTests()
		{
			UoW = new Mock<IUnitOfWork>();
			Logger = new Mock<ILogger<ICompanyProductService>>();
			ImportCompanyProductCsvService = new Mock<ICsvService<ImportCompanyProductDto>>();
			CompanyService = new Mock<ICompanyService>();
			Subject = new CompanyProductService(UoW.Object, Logger.Object, ImportCompanyProductCsvService.Object, CompanyService.Object);

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

			UoW.Setup(m => m.Companies.GetByCompanyCode(TestData.CompanyCodeA)).Returns(companyA);
			UoW.Setup(m => m.Companies.GetByCompanyCode(TestData.CompanyCodeB)).Returns(companyB);
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

			var companyProductCompanyA = new CompanyProduct
			{
				ProductSku = TestData.ProductSkuCompanyCodeA,
				ProductName = "ProductName1",
				Company = companyA
			};

			var companyProductCompanyB = new CompanyProduct
			{
				ProductSku = TestData.ProductSkuCompanyCodeB,
				ProductName = "ProductName2",
				Company = companyB
			};

			UoW.Setup(m => m.CompanyProducts.GetAll()).Returns(new List<CompanyProduct>
			{
				companyProductCompanyA,
				companyProductCompanyB
			}.AsQueryable());

			UoW.Setup(m => m.CompanyProducts.GetBySkuAndCompanyCode(TestData.ProductSkuCompanyCodeA, TestData.CompanyCodeA)).Returns(companyProductCompanyA);
			UoW.Setup(m => m.CompanyProducts.GetBySkuAndCompanyCode(TestData.ProductSkuCompanyCodeB, TestData.CompanyCodeB)).Returns(companyProductCompanyB);
		}


	}
}
