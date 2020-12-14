using BunningsProductCatalog.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BunningsProductCatalog.Repository
{
	public class BunningsProductCatalogContext : DbContext
	{
		public BunningsProductCatalogContext(DbContextOptions<BunningsProductCatalogContext> options) : base(options)
		{
		}

		public DbSet<Supplier> Suppliers { get; set; }
		public DbSet<Company> Companies { get; set; }
		public DbSet<CompanyProductBarcode> CompanyProductBarcodes { get; set; }
		public DbSet<CompanyProduct> CompanyProducts { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
			{
				relationship.DeleteBehavior = DeleteBehavior.Restrict;
			}

			modelBuilder.Entity<Company>()
		.HasData(
			new Company()
			{
				CompanyId = 1,
				CompanyCode = "A",
				CompanyName = "Company A",
			},
			new Company()
			{
				CompanyId = 2,
				CompanyCode = "B",
				CompanyName = "Company B",
			}
		);

			base.OnModelCreating(modelBuilder);
		}
	}
}