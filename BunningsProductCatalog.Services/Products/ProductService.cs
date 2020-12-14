using BunningsProductCatalog.Domain.Models;
using BunningsProductCatalog.Domain.Repository;
using BunningsProductCatalog.Services.Common;
using BunningsProductCatalog.Services.Csv;
using BunningsProductCatalog.Services.Data.Common;
using BunningsProductCatalog.Services.Data.Products.Dto;
using BunningsProductCatalog.Services.Data.Products.Results;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BunningsProductCatalog.Services.Products
{
	public class ProductService : BaseService, IProductService
	{
		private ICsvService<ProductCatalogDto> ProductCatalogCsvService { get; }

		public ProductService(IUnitOfWork uow, ILogger<IProductService> logger, ICsvService<ProductCatalogDto> productCatalogCsvService) : base(uow, logger)
		{
			ProductCatalogCsvService = productCatalogCsvService;
		}

		public GetProductCatalogResult GetProductCatalog()
		{
			var result = new GetProductCatalogResult();

			try
			{
				var companyProducts = UoW.CompanyProducts.GetAll().Where(i => !i.IsDeleted);
				var productCatalog = new List<CompanyProduct>();

				foreach (var companyProduct in companyProducts)
				{
					var matchingBarcodeFound = false;

					foreach (var barcode in companyProduct.CompanyProductBarcodes)
					{
						if (productCatalog.SelectMany(i => i.CompanyProductBarcodes).Select(i => i.Barcode).ToList().Contains(barcode.Barcode))
						{
							matchingBarcodeFound = true;
							break;
						}
					}

					if (!matchingBarcodeFound)
					{
						productCatalog.Add(companyProduct);
					}
				}

				result.ProductCatalog = productCatalog
						.Select(i => new ProductCatalogDto()
						{
							ProductSku = i.ProductSku,
							ProductName = i.ProductName,
							CompanyCode = i.Company.CompanyCode,
						}).ToList();

				result.Success = true;
			}
			catch (Exception e)
			{
				result.Success = false;
				result.Errors.Add(new ExceptionError(e.Message));
			}

			return result;
		}

		public GenerateProductCatalogFileResult GenerateProductCatalogFile()
		{
			var result = new GenerateProductCatalogFileResult();

			try
			{
				var getProductCatalogResult = GetProductCatalog();
				if (getProductCatalogResult.Success)
				{
					var fileContents = new StringBuilder();
					fileContents.Append(ProductCatalogCsvService.WriteRecords(getProductCatalogResult.ProductCatalog, true, new ProductCatalogDtoClassMap(), ",", false));

					result.FileContents = fileContents.ToString();
					result.FileName = $"result_output.csv";
					result.Success = true;
				}
				else
				{
					result.Errors.AddRange(getProductCatalogResult.Errors);
				}

			}
			catch (Exception e)
			{
				result.Success = false;
				result.Errors.Add(new ExceptionError(e.Message));
			}

			return result;
		}

	}
}
