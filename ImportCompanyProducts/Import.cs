using BunningsProductCatalog.Services.CompanyProducts;
using BunningsProductCatalog.Services.Data.CompanyProducts.Requests;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Linq;

namespace ImportCompanyProducts
{
	public class Import
	{
		private ICompanyProductService CompanyProductService { get; }
		private IConfiguration Config { get; }
		private ILogger<Import> Logger { get; }

		public Import(ICompanyProductService companyProductService, IConfiguration config, ILogger<Import> logger)
		{
			CompanyProductService = companyProductService;
			Config = config;
			Logger = logger;
		}

		[NoAutomaticTrigger]
		public void ImportCompanyProducts()
		{
			Logger.LogInformation($"Web job started.");

			var inputFilePath = Config["InputFilePath"];

			var files = Directory.GetFiles(inputFilePath, "catalog*.csv");
			foreach (var file in files)
			{
				var companyCode = file.Replace("catalog", "").Replace(inputFilePath, "").Replace(".csv", "");
				using FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read);
				var result = CompanyProductService.ImportCompanyProductsFromFileStream(new ImportCompanyProductRequest() { FileStream = fs, CompanyCode = companyCode });
				if (!result.Success)
				{
					Logger.LogError($"Import failed {string.Join(",", result.Errors?.Select(i => i.Message))}.");
				}
			}
			Logger.LogInformation($"Web job completed.");
		}
	}
}
