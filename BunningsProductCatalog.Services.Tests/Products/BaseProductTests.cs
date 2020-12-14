using BunningsProductCatalog.Domain.Models;
using BunningsProductCatalog.Services.Csv;
using BunningsProductCatalog.Services.Data.Products.Dto;
using BunningsProductCatalog.Services.Products;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Linq;
using BunningsProductCatalog.Domain.Repository;

namespace BunningsProductCatalog.Services.Tests.Products
{
	public abstract class BaseProductTests
	{
		protected IProductService Subject { get; }
		protected Mock<ICsvService<ProductCatalogDto>> ProductCatalogCsvService { get; }
		protected Mock<ILogger<ProductService>> Logger { get; }
		protected Mock<IUnitOfWork> UoW { get; }

		public BaseProductTests()
		{
			UoW = new Mock<IUnitOfWork>();
			Logger = new Mock<ILogger<ProductService>>();
			ProductCatalogCsvService = new Mock<ICsvService<ProductCatalogDto>>();
			Subject = new ProductService(UoW.Object, Logger.Object, ProductCatalogCsvService.Object);

			var companyA = new Company
			{
				CompanyCode = "A",
				CompanyName = "Company A"
			};

			var companyB = new Company
			{
				CompanyCode = "B",
				CompanyName = "Company A"
			};

			var companyASupplier1 = new Supplier
			{
				SupplierCode = "00001",
				SupplierName = "Supplier 1",
				Company = companyA
			};

			var companyBSupplier1 = new Supplier
			{
				SupplierCode = "00001",
				SupplierName = "Supplier 1",
				Company = companyA
			};

			var companyACompanyProduct1 = new CompanyProduct
			{
				ProductSku = "ProductSku1",
				ProductName = "ProductName1",
				Company = companyA,
				CompanyProductBarcodes = new List<CompanyProductBarcode>()
			};

			var companyBCompanyProduct1 = new CompanyProduct
			{
				ProductSku = "ProductSku2",
				ProductName = "ProductName2",
				Company = companyB,
				CompanyProductBarcodes = new List<CompanyProductBarcode>()
			};

			var companyBCompanyProduct2 = new CompanyProduct
			{
				ProductSku = "ProductSku3",
				ProductName = "ProductName3",
				Company = companyB,
				CompanyProductBarcodes = new List<CompanyProductBarcode>()
			};

			var companyACompanyProduct1Barcode1 = new CompanyProductBarcode
			{
				Barcode = "Barcode1",
				CompanyProduct = companyACompanyProduct1,
				Supplier = companyASupplier1
			};
			companyACompanyProduct1.CompanyProductBarcodes.Add(companyACompanyProduct1Barcode1);

			var companyACompanyProduct1Barcode2 = new CompanyProductBarcode
			{
				Barcode = "Barcode2",
				CompanyProduct = companyACompanyProduct1,
				Supplier = companyASupplier1
			};
			companyACompanyProduct1.CompanyProductBarcodes.Add(companyACompanyProduct1Barcode2);

			var companyBCompanyProduct1Barcode1 = new CompanyProductBarcode
			{
				Barcode = "Barcode1",
				CompanyProduct = companyBCompanyProduct1,
				Supplier = companyBSupplier1
			};
			companyBCompanyProduct1.CompanyProductBarcodes.Add(companyBCompanyProduct1Barcode1);

			var companyBCompanyProduct2Barcode1 = new CompanyProductBarcode
			{
				Barcode = "Barcode3",
				CompanyProduct = companyBCompanyProduct2,
				Supplier = companyBSupplier1
			};
			companyBCompanyProduct2.CompanyProductBarcodes.Add(companyBCompanyProduct2Barcode1);

			UoW.Setup(m => m.CompanyProducts.GetAll()).Returns(new List<CompanyProduct>
			{
				companyACompanyProduct1,
				companyBCompanyProduct1,
				companyBCompanyProduct2,
			}.AsQueryable());



		}


	}
}
