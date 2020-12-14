using System;
using System.Collections.Generic;
using System.Text;

namespace BunningsProductCatalog.Services.Data.CompanyProductBarcodes.Requests
{
	public class CreateCompanyProductBarcodeRequest
	{
		public string SupplierCode { get; set; }
		public string ProductSku { get; set; }
		public string Barcode { get; set; }
		public string CompanyCode { get; set; }
	}
}
