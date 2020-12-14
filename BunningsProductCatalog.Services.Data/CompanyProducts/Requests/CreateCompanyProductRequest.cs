using System;
using System.Collections.Generic;
using System.Text;

namespace BunningsProductCatalog.Services.Data.CompanyProducts.Requests
{
	public class CreateCompanyProductRequest
	{ 
		public string ProductName { get; set; }
		public string ProductSku { get; set; }
		public string CompanyCode { get; set; }
	}
}
