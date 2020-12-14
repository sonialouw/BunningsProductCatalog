using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;


namespace BunningsProductCatalog.Services.Csv
{
	public class CsvService<T> : ICsvService<T> where T : class
	{
		public List<T> GetRecords(Stream fileStream, ClassMap<T> classMap)
		{
			var config = new CsvConfiguration(CultureInfo.CurrentCulture)
			{
				HasHeaderRecord = true,
				TrimOptions = TrimOptions.Trim
			};
			if (classMap != null)
			{
				config.RegisterClassMap(classMap);
			}
			using (var csvStream = new MemoryStream())
			{
				fileStream.CopyTo(csvStream);
				csvStream.Position = 0;

				using (var reader = new CsvReader(new StreamReader(csvStream), config))
				{
					var records = reader.GetRecords<T>().ToList();
					return records;
				}
			}
		}

		public string WriteRecords(List<T> records, bool hasHeader, ClassMap<T> classMap, string delimiter, bool shouldQuote)
		{
			var contents = string.Empty;

			var config = new CsvConfiguration(CultureInfo.CurrentCulture)
			{
				HasHeaderRecord = hasHeader,
				TrimOptions = TrimOptions.Trim,
				Delimiter = delimiter,
				ShouldQuote = (field, context) => shouldQuote,
			};

			if (classMap != null)
			{
				config.RegisterClassMap(classMap);
			}

			using (var stream = new MemoryStream())
			using (var reader = new StreamReader(stream))
			using (var writer = new StreamWriter(stream))
			using (var csv = new CsvWriter(writer, config))
			{
				csv.WriteRecords(records);
				writer.Flush();
				stream.Position = 0;
				contents = reader.ReadToEnd();
			}

			return contents;
		}
	}
}

