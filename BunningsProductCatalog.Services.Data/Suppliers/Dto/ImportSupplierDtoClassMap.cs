using CsvHelper.Configuration;

namespace BunningsProductCatalog.Services.Data.Suppliers.Dto
{
	public sealed class ImportSupplierDtoClassMap : ClassMap<ImportSupplierDto>
	{
		public ImportSupplierDtoClassMap()
		{
			Map(m => m.SupplierCode).Name("ID");
			Map(m => m.SupplierName).Name("Name");
		}
	}
}
