using System.Globalization;
using CsvHelper.Configuration;

namespace BunningsProductCatalog.Services.Tests.Csv
{
	public sealed class CsvTestObjectClassMap : ClassMap<CsvTestObject>
	{
		public CsvTestObjectClassMap()
		{
			AutoMap(CultureInfo.InvariantCulture);
			Map(m => m.DateOfBirth).TypeConverterOption.Format("yyyy-MM-dd");
		}
	}
}
