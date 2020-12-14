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
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.WebJobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using BunningsProductCatalog.Domain.Repository;
using BunningsProductCatalog.Repository;


namespace ImportCompanyProducts
{
	class Program
	{
		static async Task Main(string[] args)
		{
			var envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
				?? Environment.GetEnvironmentVariable("ENV")
				?? "Development";

			var builder = new HostBuilder()
				.UseEnvironment(envName)
				.ConfigureAppConfiguration((context, config) =>
				{
					var env = context.HostingEnvironment;

					config.AddEnvironmentVariables();

					config
						.AddJsonFile("appsettings.json", true, true)
						.AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true)
						.AddJsonFile($"appsettings.{Environment.MachineName}.json", true, true);


				})
				.ConfigureServices((context, services) =>
				{
					var connectionString = context.Configuration.GetConnectionString("BunningsProductCatalogContext");

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
				})
				.ConfigureWebJobs(b =>
				{
					b.AddAzureStorageCoreServices();
				})
				.ConfigureLogging((context, config) =>
				{
					config.ClearProviders();

					if (context.HostingEnvironment.IsDevelopment())
					{
						config.AddConsole();
					}

					string instrumentationKey = context.Configuration["APPINSIGHTS_INSTRUMENTATIONKEY"];
					if (!string.IsNullOrEmpty(instrumentationKey))
					{
						config.AddApplicationInsightsWebJobs(o =>
						{
							o.InstrumentationKey = instrumentationKey;
							o.SamplingSettings = new Microsoft.ApplicationInsights.WindowsServer.Channel.Implementation.SamplingPercentageEstimatorSettings()
							{
								InitialSamplingPercentage = 100,
								MinSamplingPercentage = 100,
								MaxSamplingPercentage = 100
							};
						});
					}

					config.AddFilter<Microsoft.Extensions.Logging.ApplicationInsights.ApplicationInsightsLoggerProvider>
													 (typeof(Program).FullName, LogLevel.Trace);
				});

			var host = builder.Build();
			using (host)
			{
				var jobHost = host.Services.GetService(typeof(IJobHost)) as JobHost;
				await host.StartAsync();
				await jobHost.CallAsync("ImportCompanyProducts");
				await host.StopAsync();
			}
		}
	}
}
