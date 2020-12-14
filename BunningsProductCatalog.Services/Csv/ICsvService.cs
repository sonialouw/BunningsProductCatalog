using System.Collections.Generic;
using System.IO;
using CsvHelper.Configuration;

namespace BunningsProductCatalog.Services.Csv
{
	public interface ICsvService<T> where T : class
	{
		List<T> GetRecords(Stream fileStream, ClassMap<T> classMap);
		string WriteRecords(List<T> records, bool hasHeader, ClassMap<T> classMap, string delimiter, bool shouldQuote);
	}
}
