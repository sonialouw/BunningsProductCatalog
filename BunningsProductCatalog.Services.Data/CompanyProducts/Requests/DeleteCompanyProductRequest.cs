using System;
using System.Collections.Generic;
using System.Text;

namespace BunningsProductCatalog.Services.Data.CompanyProducts.Requests
{
	public class DeleteCompanyProductRequest
	{ 
		public string ProductSku { get; set; }
		public string CompanyCode { get; set; }
	}
}
