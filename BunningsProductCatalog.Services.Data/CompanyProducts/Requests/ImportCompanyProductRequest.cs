using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BunningsProductCatalog.Services.Data.CompanyProducts.Requests
{
	public class ImportCompanyProductRequest
	{
		public string CompanyCode { get; set; }
		public Stream FileStream { get; set; }
		public string FileName { get; set; }
	}
}
