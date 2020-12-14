using System;
using System.Collections.Generic;
using System.Text;

namespace BunningsProductCatalog.Services.Data.Suppliers.Requests
{
	public class CreateSupplierRequest
	{	
		public string SupplierName { get; set; }
		public string SupplierCode { get; set; }
		public string CompanyCode { get; set; }
	}
}
