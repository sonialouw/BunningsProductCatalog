using BunningsProductCatalog.Services.Data.Suppliers.Requests;
using BunningsProductCatalog.Services.Suppliers;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.IO;
using BunningsProductCatalog.Domain.Repository;

namespace ImportSuppliers
{
	public class Import
	{
		private ISupplierService SupplierService { get; }
		private IConfiguration Config { get; }
		private ILogger<Import> Logger { get; }

		public Import(ISupplierService supplierService, IConfiguration config, ILogger<Import> logger, IUnitOfWork uow)
		{
			SupplierService = supplierService;
			Config = config;
			Logger = logger;
		}

		[NoAutomaticTrigger]
		public void ImportSuppliers()
		{
			Logger.LogInformation($"Web job started.");

			var inputFilePath = Config["InputFilePath"];

			var files = Directory.GetFiles(inputFilePath, "suppliers*.csv");
			foreach (var file in files)
			{
				var companyCode = file.Replace("suppliers", "").Replace(inputFilePath, "").Replace(".csv", "");
				using FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read);
				SupplierService.ImportSuppliersFromFileStream(new ImportSupplierRequest() { FileStream = fs, CompanyCode = companyCode });
			}

			Logger.LogInformation($"Web job completed.");
		}
	}
}
