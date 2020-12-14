using Microsoft.EntityFrameworkCore;

namespace BunningsProductCatalog.Repository
{
	public static class DbInitializer
	{
		public static void Initialize(BunningsProductCatalogContext context)
		{
			context.Database.Migrate();
		}
	}
}