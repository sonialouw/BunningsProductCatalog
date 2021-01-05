using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using Azure.Identity;

namespace BunningsProductCatalog.Web
{
	public class Program
	{
		public static void Main(string[] args)
		{
			CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args)
		{
			return Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();

					// disable app insights for now
					//webBuilder.ConfigureLogging((context, b) =>
					//{
					//	var instrumentationKey = context.Configuration["APPINSIGHTS_INSTRUMENTATIONKEY"];
					//	if (!string.IsNullOrEmpty(instrumentationKey))
					//	{
					//		b.AddApplicationInsights(instrumentationKey);
					//	}

					//	b.AddFilter<Microsoft.Extensions.Logging.ApplicationInsights.ApplicationInsightsLoggerProvider>(typeof(Program).FullName, LogLevel.Trace);
					//	b.AddFilter<Microsoft.Extensions.Logging.ApplicationInsights.ApplicationInsightsLoggerProvider>(typeof(Startup).FullName, LogLevel.Trace);
					//	b.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning);
					//	b.AddFilter("Microsoft.EntityFrameworkCore.Infrastructure", LogLevel.Warning);

					//});

					webBuilder.ConfigureAppConfiguration((hostingContext, config) =>
					{

						// app settings
						var env = hostingContext.HostingEnvironment;
						config.AddJsonFile("appsettings.json", true, true)
							.AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true)
							.AddJsonFile($"appsettings.{Environment.MachineName}.json", true, true);

						// disable key vault as it is not needed here
						//if (!hostingContext.HostingEnvironment.IsDevelopment())
						//{
						//	var builtConfig = config.Build();
						//	config.AddAzureKeyVault(new Uri(builtConfig["KeyVault:BaseUrl"]), new DefaultAzureCredential());
						//}

					});


				});
		}
	}
}
