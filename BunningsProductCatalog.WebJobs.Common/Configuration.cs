using System;
using BunningsProductCatalog.Domain.Repository;
using BunningsProductCatalog.Repository;
using BunningsProductCatalog.Services.Companies;
using BunningsProductCatalog.Services.CompanyProductBarcodes;
using BunningsProductCatalog.Services.CompanyProducts;
using BunningsProductCatalog.Services.Csv;
using BunningsProductCatalog.Services.Data.CompanyProductBarcodes.Dto;
using BunningsProductCatalog.Services.Data.CompanyProducts.Dto;
using BunningsProductCatalog.Services.Data.Products.Dto;
using BunningsProductCatalog.Services.Data.Suppliers.Dto;
using BunningsProductCatalog.Services.Products;
using BunningsProductCatalog.Services.Suppliers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BunningsProductCatalog.WebJobs.Common
{
	public static class Configuration
	{

		public static void ConfigureAppSettings(HostBuilderContext context, IConfigurationBuilder config)
		{
			var envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
			              ?? Environment.GetEnvironmentVariable("ENV")
			              ?? "Development";

			config.AddEnvironmentVariables();

			config
				.AddJsonFile("appsettings.json", true, true)
				.AddJsonFile($"appsettings.{envName}.json", true, true)
				.AddJsonFile($"appsettings.{Environment.MachineName}.json", true, true);
		}

		public static void ConfigureLogging(HostBuilderContext context, ILoggingBuilder config, string applicationInsightsCategory)
		{
			config.ClearProviders();

			if (context.HostingEnvironment.IsDevelopment())
			{
				config.AddConsole();
			}
			config.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning);
			config.AddFilter("Microsoft.EntityFrameworkCore.Infrastructure", LogLevel.Warning);

			// disable app insights for now
			//string instrumentationKey = context.Configuration["APPINSIGHTS_INSTRUMENTATIONKEY"];
			//if (!string.IsNullOrEmpty(instrumentationKey))
			//{
			//	config.AddApplicationInsightsWebJobs(o =>
			//	{
			//		o.InstrumentationKey = instrumentationKey;
			//		o.SamplingSettings = new Microsoft.ApplicationInsights.WindowsServer.Channel.Implementation.SamplingPercentageEstimatorSettings()
			//		{
			//			InitialSamplingPercentage = 100,
			//			MinSamplingPercentage = 100,
			//			MaxSamplingPercentage = 100
			//		};
			//	});
			//}

			//config.AddFilter<Microsoft.Extensions.Logging.ApplicationInsights.ApplicationInsightsLoggerProvider>(applicationInsightsCategory, LogLevel.Trace);

			}


		public static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
		{
			services.AddScoped<IUnitOfWork, UnitOfWork>();
			services.AddScoped<IRepositoryProvider, RepositoryProvider>();
			services.AddSingleton<RepositoryFactories>();
			services.AddScoped<IProductService, ProductService>();
			services.AddScoped<ICompanyService, CompanyService>();
			services.AddScoped<ICompanyProductService, CompanyProductService>();
			services.AddScoped<ICompanyProductBarcodeService, CompanyProductBarcodeService>();
			services.AddScoped<ISupplierService, SupplierService>();
			services.AddScoped<ICsvService<ImportSupplierDto>, CsvService<ImportSupplierDto>>();
			services.AddScoped<ICsvService<ImportCompanyProductDto>, CsvService<ImportCompanyProductDto>>();
			services.AddScoped<ICsvService<ImportCompanyProductBarcodeDto>, CsvService<ImportCompanyProductBarcodeDto>>();
			services.AddScoped<ICsvService<ProductCatalogDto>, CsvService<ProductCatalogDto>>();

			services.AddDbContext<BunningsProductCatalogContext>(options =>
				options.UseLazyLoadingProxies().UseSqlServer(
					context.Configuration.GetConnectionString("BunningsProductCatalogContext")));

		}
	}
}
