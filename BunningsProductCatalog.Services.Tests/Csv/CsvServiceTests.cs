using BunningsProductCatalog.Services.Csv;
using BunningsProductCatalog.Services.Products;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace BunningsProductCatalog.Services.Tests.Csv
{
	public class CsvServiceTests
	{
		public CsvServiceTests()
		{
			Subject = new CsvService<CsvTestObject>();
		}

		private ICsvService<CsvTestObject> Subject { get; }

		[Trait("Category", "unit")]
		[Fact]
		public void MatchOnHeader()
		{
			var localSubject = new CsvService<CsvTestObject>();
			using (var writer = new StreamWriter(new MemoryStream()))
			{
				//Assemble
				writer.WriteLine(
					"FirstName,LastName,Quantity,DateOfBirth");

				writer.WriteLine("Test, User, 1, 2018-1-1");
				writer.WriteLine("Second Test, Second User, 2, 2018-2-1");
				writer.WriteLine("Third Test, Third User, 3, 2018-3-1");
				writer.Flush();
				writer.BaseStream.Position = 0;

				//Act
				var records = localSubject.GetRecords(writer.BaseStream, null);

				//Assert
				Assert.Equal(3, records.Count);
				Assert.Equal("Test", records[0].FirstName);
				Assert.Equal("User", records[0].LastName);
				Assert.Equal(1, records[0].Quantity);
				Assert.Equal(new DateTime(2018,1,1), records[0].DateOfBirth);
				Assert.Equal("Second Test", records[1].FirstName);
				Assert.Equal("Second User", records[1].LastName);
				Assert.Equal(2, records[1].Quantity);
				Assert.Equal(new DateTime(2018, 2, 1), records[1].DateOfBirth);
				Assert.Equal("Third Test", records[2].FirstName);
				Assert.Equal("Third User", records[2].LastName);
				Assert.Equal(3, records[2].Quantity);
				Assert.Equal(new DateTime(2018, 3, 1), records[2].DateOfBirth);
			}
		}


		[Trait("Category", "unit")]
		[Fact]
		public void WriteRecordsWithQuote()
		{
			//Assemble
			System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("en-AU");
			System.Threading.Thread.CurrentThread.CurrentCulture = ci;

			var localSubject = new CsvService<CsvTestObject>();

			var inputRecords = new List<CsvTestObject>()
			{
				new CsvTestObject() { FirstName = "FirstName1", LastName="LastName1",Quantity = 1, DateOfBirth= new DateTime(1980,1,1) },
				new CsvTestObject() { FirstName = "FirstName2", LastName="LastName2",Quantity = 2, DateOfBirth= new DateTime(1979,1,1) },
			};

			// Act
			var result = localSubject.WriteRecords(inputRecords, false, new CsvTestObjectClassMap(), ",", true);

			//Assert
			Assert.Equal(86, result.Length);


			var records = result.Split(
					new[] { "\r\n", "\r", "\n" },
					StringSplitOptions.None
			);

			Assert.Equal(3, records.Length);

			var record = records[0].Split(",");

			Assert.Equal(4, record.Length);
			Assert.Equal($"\"{inputRecords[0].FirstName}\"", record[0]);
			Assert.Equal($"\"{inputRecords[0].LastName}\"", record[1]);
			Assert.Equal($"\"{inputRecords[0].Quantity}\"", record[2]);
			Assert.Equal($"\"{inputRecords[0].DateOfBirth:yyyy-MM-dd}\"", record[3]);
		}


		[Trait("Category", "unit")]
		[Fact]
		public void WriteRecordsWithoutQuote()
		{
			//Assemble
			System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("en-AU");
			System.Threading.Thread.CurrentThread.CurrentCulture = ci;

			var localSubject = new CsvService<CsvTestObject>();

			var inputRecords = new List<CsvTestObject>()
			{
				new CsvTestObject() { FirstName = "FirstName1", LastName="LastName1",Quantity = 1, DateOfBirth= new DateTime(1980,1,1) },
				new CsvTestObject() { FirstName = "FirstName2", LastName="LastName2",Quantity = 2, DateOfBirth= new DateTime(1979,1,1) },
			};

			// Act
			var result = localSubject.WriteRecords(inputRecords, false, new CsvTestObjectClassMap(), ",", false);

			//Assert
			Assert.Equal(70, result.Length);

			var records = result.Split(
					new[] { "\r\n", "\r", "\n" },
					StringSplitOptions.None
			);

			Assert.Equal(3, records.Length);

			var record = records[0].Split(",");

			Assert.Equal(4, record.Length);
			Assert.Equal($"{inputRecords[0].FirstName}", record[0]);
			Assert.Equal($"{inputRecords[0].LastName}", record[1]);
			Assert.Equal($"{inputRecords[0].Quantity}", record[2]);
			Assert.Equal($"{inputRecords[0].DateOfBirth:yyyy-MM-dd}", record[3]);
		}

		[Trait("Category", "unit")]
		[Fact]
	
		public void WriteRecordsWithPipeDelimiter()
		{
			//Assemble
			System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("en-AU");
			System.Threading.Thread.CurrentThread.CurrentCulture = ci;

			var localSubject = new CsvService<CsvTestObject>();

			var inputRecords = new List<CsvTestObject>()
			{
				new CsvTestObject() { FirstName = "FirstName1", LastName="LastName1",Quantity = 1, DateOfBirth= new DateTime(1980,1,1) },
				new CsvTestObject() { FirstName = "FirstName2", LastName="LastName2",Quantity = 2, DateOfBirth= new DateTime(1979,1,1) },
			};

			// Act
			var result = localSubject.WriteRecords(inputRecords, false, new CsvTestObjectClassMap(), "|", false);

			//Assert
			Assert.Equal(70, result.Length);

			var records = result.Split(
					new[] { "\r\n", "\r", "\n" },
					StringSplitOptions.None
			);

			Assert.Equal(3, records.Length);

			var record = records[0].Split("|");

			Assert.Equal(4, record.Length);
			Assert.Equal($"{inputRecords[0].FirstName}", record[0]);
			Assert.Equal($"{inputRecords[0].LastName}", record[1]);
			Assert.Equal($"{inputRecords[0].Quantity}", record[2]);
			Assert.Equal($"{inputRecords[0].DateOfBirth:yyyy-MM-dd}", record[3]);

		}

	}
}