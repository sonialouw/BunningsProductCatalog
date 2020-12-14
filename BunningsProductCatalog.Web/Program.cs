using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

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


					webBuilder.ConfigureLogging((context, b) =>
					{


					
						string instrumentationKey = context.Configuration["APPINSIGHTS_INSTRUMENTATIONKEY"];
						if (!string.IsNullOrEmpty(instrumentationKey))
						{
							b.AddApplicationInsights(instrumentationKey);
						}

						// Adding the filter below to ensure logs of all severity from Program.cs
						// is sent to ApplicationInsights.
						b.AddFilter<Microsoft.Extensions.Logging.ApplicationInsights.ApplicationInsightsLoggerProvider>
														 (typeof(Program).FullName, LogLevel.Trace);

						// Adding the filter below to ensure logs of all severity from Startup.cs
						// is sent to ApplicationInsights.
						b.AddFilter<Microsoft.Extensions.Logging.ApplicationInsights.ApplicationInsightsLoggerProvider>
														 (typeof(Startup).FullName, LogLevel.Trace);

						b.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning);
						b.AddFilter("Microsoft.EntityFrameworkCore.Infrastructure", LogLevel.Warning);
					});

					webBuilder.ConfigureAppConfiguration((hostingContext, config) =>
					{
						var env = hostingContext.HostingEnvironment;
						config.AddJsonFile("appsettings.json", true, true)
							.AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true)
							.AddJsonFile($"appsettings.{Environment.MachineName}.json", true, true);

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
