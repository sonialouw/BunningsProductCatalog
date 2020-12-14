using BunningsProductCatalog.Domain.Models;
using BunningsProductCatalog.Domain.Repository;
using BunningsProductCatalog.Services.Companies;
using BunningsProductCatalog.Services.CompanyProductBarcodes;
using BunningsProductCatalog.Services.CompanyProducts;
using BunningsProductCatalog.Services.Csv;
using BunningsProductCatalog.Services.Data.Common;
using BunningsProductCatalog.Services.Data.CompanyProducts.Dto;
using BunningsProductCatalog.Services.Suppliers;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace BunningsProductCatalog.Services.Tests.CompanyProductBarcodes
{
	public abstract class BaseCompanyProductBarcodeTests
	{
		protected ICompanyProductBarcodeService Subject { get; }
		protected Mock<ICsvService<ImportCompanyProductBarcodeDto>> ImportCompanyProductBarcodeCsvService { get; }
		protected Mock<ILogger<ICompanyProductBarcodeService>> Logger { get; }
		protected Mock<IUnitOfWork> UoW { get; }
		protected Mock<ICompanyService> CompanyService { get; }
		protected Mock<ICompanyProductService> CompanyProductService { get; }
		protected Mock<ISupplierService> SupplierService { get; }

		public BaseCompanyProductBarcodeTests()
		{
			UoW = new Mock<IUnitOfWork>();
			Logger = new Mock<ILogger<ICompanyProductBarcodeService>>();
			ImportCompanyProductBarcodeCsvService = new Mock<ICsvService<ImportCompanyProductBarcodeDto>>();
			CompanyService = new Mock<ICompanyService>();
			CompanyProductService = new Mock<ICompanyProductService>();
			SupplierService = new Mock<ISupplierService>();
			Subject = new CompanyProductBarcodeService(UoW.Object, Logger.Object, ImportCompanyProductBarcodeCsvService.Object, CompanyProductService.Object, SupplierService.Object, CompanyService.Object);

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
				SupplierCode = TestData.SupplierCode,
				SupplierName = "SupplierName2",
				Company = companyA
			};

			SupplierService.Setup(m => m.GetSupplier(TestData.SupplierCode, TestData.CompanyCodeA)).Returns(supplier);
			SupplierService.Setup(m => m.ValidateSupplierExist(TestData.SupplierCode, TestData.CompanyCodeA)).Returns(new List<Error>());

			var companyProductProductCompanyA = new CompanyProduct
			{
				ProductSku = TestData.ProductSkuCompanyCodeA,
				ProductName = "ProductName1",
				Company = companyA
			};

			var companyProductProductCompanyB = new CompanyProduct
			{
				ProductSku = TestData.ProductSkuCompanyCodeB,
				ProductName = "ProductName2",
				Company = companyB
			};

			CompanyProductService.Setup(m => m.GetCompanyProduct(TestData.ProductSkuCompanyCodeA, TestData.CompanyCodeA)).Returns(companyProductProductCompanyA);
			CompanyProductService.Setup(m => m.ValidateCompanyProductExist(TestData.ProductSkuCompanyCodeA, TestData.CompanyCodeA)).Returns(new List<Error>());

			CompanyProductService.Setup(m => m.GetCompanyProduct(TestData.ProductSkuCompanyCodeB, TestData.CompanyCodeB)).Returns(companyProductProductCompanyB);
			CompanyProductService.Setup(m => m.ValidateCompanyProductExist(TestData.ProductSkuCompanyCodeB, TestData.CompanyCodeB)).Returns(new List<Error>());

			var companyProductBarcode1 = new CompanyProductBarcode
			{
				Barcode = TestData.BarcodeProductSkuCompanyCodeA,
				CompanyProduct = companyProductProductCompanyA,
				Supplier = supplier
			};

			UoW.Setup(m => m.CompanyProductBarcodes.GetAll()).Returns(new List<CompanyProductBarcode>
			{
				companyProductBarcode1
			}.AsQueryable());


		}


	}
}
