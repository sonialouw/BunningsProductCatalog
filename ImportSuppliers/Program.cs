using BunningsProductCatalog.WebJobs.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace ImportSuppliers
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
					Configuration.ConfigureAppSettings(context, config);
				})
				.ConfigureServices((context, services) =>
				{
					Configuration.ConfigureServices(context, services);
				})
				.ConfigureWebJobs(b =>
				{
					b.AddAzureStorageCoreServices();
				})
				.ConfigureLogging((context, config) =>
				{
					Configuration.ConfigureLogging(context, config, typeof(Program).FullName);
				});

			var host = builder.Build();
			using (host)
			{
				var jobHost = host.Services.GetService(typeof(IJobHost)) as JobHost;
				await host.StartAsync();
				await jobHost.CallAsync("ImportSuppliers");
				await host.StopAsync();
			}
		}
	}
}
