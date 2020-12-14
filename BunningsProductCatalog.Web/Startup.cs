using AutoMapper;
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
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using BunningsProductCatalog.Domain.Repository;
using BunningsProductCatalog.Repository;


namespace BunningsProductCatalog.Web
{
	public class Startup
	{
		public Startup(IConfiguration configuration, IHostEnvironment env)
		{
			Configuration = configuration;
			Env = env;
		}

		public IConfiguration Configuration { get; }
		public IHostEnvironment Env { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			var mvcBuilder = services.AddControllersWithViews().AddNewtonsoftJson(options =>
			{
				options.SerializerSettings.ContractResolver = new DefaultContractResolver();
				options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
			});

#if DEBUG
			if (Env.IsDevelopment())
			{
				mvcBuilder.AddRazorRuntimeCompilation();
			}
#endif

			var aiOptions = new ApplicationInsightsServiceOptions
			{
				EnableAdaptiveSampling = false
			};
			services.AddApplicationInsightsTelemetry(aiOptions);

			services.AddDbContext<BunningsProductCatalogContext>(options =>
				options.UseLazyLoadingProxies().UseSqlServer(
					Configuration.GetConnectionString("BunningsProductCatalogContext")));

			services.AddMvcCore().AddNewtonsoftJson(
				options => options.SerializerSettings.ReferenceLoopHandling =
					ReferenceLoopHandling.Ignore);

			services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
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
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					"default",
					"{controller=Home}/{action=Index}/{id?}");
			});

			UpdateDatabase(app);
		}

		private static void UpdateDatabase(IApplicationBuilder app)
		{
			using var serviceScope = app.ApplicationServices
				.GetRequiredService<IServiceScopeFactory>()
				.CreateScope();

			using var context = serviceScope.ServiceProvider.GetService<BunningsProductCatalogContext>();
			context.Database.Migrate();
		}
	}
}
