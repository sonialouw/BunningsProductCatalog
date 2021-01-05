using BunningsProductCatalog.Services.CompanyProductBarcodes;
using BunningsProductCatalog.Services.Data.CompanyProductBarcodes.Requests;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Linq;

namespace ImportCompanyProductBarcodes
{
	public class Import
	{
		private ICompanyProductBarcodeService CompanyProductBarcodeService { get; }
		private IConfiguration Config { get; }
		private ILogger<Import> Logger { get; }

		public Import(ICompanyProductBarcodeService companyProductBarcodeService, IConfiguration config, ILogger<Import> logger)
		{
			CompanyProductBarcodeService = companyProductBarcodeService;
			Config = config;
			Logger = logger;
		}

		[NoAutomaticTrigger]
		public void ImportCompanyProductBarcodes()
		{
			Logger.LogInformation($"Web job started.");

			var inputFilePath = Config["InputFilePath"];

			var files = Directory.GetFiles(inputFilePath, "barcodes*.csv");
			foreach (var file in files)
			{
				var companyCode = file.Replace("barcodes", "").Replace(inputFilePath, "").Replace(".csv", "");
				using FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read);
				var result = CompanyProductBarcodeService.ImportCompanyProductBarcodesFromFileStream(new ImportCompanyProductBarcodeRequest() { FileStream = fs, CompanyCode = companyCode });
				if (!result.Success)
				{
					Logger.LogError($"Import failed {string.Join(",", result.Errors?.Select(i => i.Message))}.");
				}
			}
			Logger.LogInformation($"Web job completed.");
		}
	}
}
