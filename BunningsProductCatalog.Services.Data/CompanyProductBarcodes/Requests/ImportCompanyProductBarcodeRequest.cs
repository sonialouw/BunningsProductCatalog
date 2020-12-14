using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BunningsProductCatalog.Services.Data.CompanyProductBarcodes.Requests
{
	public class ImportCompanyProductBarcodeRequest
	{
		public string CompanyCode { get; set; }
		public Stream FileStream { get; set; }
		public string FileName { get; set; }
	}
}
