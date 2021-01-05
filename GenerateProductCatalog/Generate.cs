using BunningsProductCatalog.Domain.Repository;
using BunningsProductCatalog.Services.Products;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Linq;

namespace GenerateProductCatalog
{
	public class Generate
	{
		private IProductService ProductService { get; }
		private IConfiguration Config { get; }
		private ILogger<Generate> Logger { get; }
		private IUnitOfWork UoW { get; }


		public Generate(IProductService productService, IConfiguration config, ILogger<Generate> logger, IUnitOfWork uow)
		{
			ProductService = productService;
			Config = config;
			Logger = logger;
			UoW = uow;
		}

		[NoAutomaticTrigger]
		public void GenerateProductCatalogFile()
		{
			Logger.LogInformation($"Web job started.");

			var outputFilePath = Config["OutputFilePath"];
			var generateProductCatalogResult = ProductService.GenerateProductCatalogFile();

			if (generateProductCatalogResult.Success)
			{
				using var outputFile = new StreamWriter(Path.Combine(@$"{outputFilePath}", generateProductCatalogResult.FileName));
				outputFile.Write(generateProductCatalogResult.FileContents.ToString());
			}
			else
			{
				Logger.LogError($"Create product catalog failed {string.Join(",", generateProductCatalogResult.Errors?.Select(i => i.Message))}.");

			}

			Logger.LogInformation($"Web job completed.");
		}
	}
}
