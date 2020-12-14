using BunningsProductCatalog.Services.Data.Common;
using System.Collections.Generic;
using static BunningsProductCatalog.Services.Data.Common.CommonValidationFunctions;

namespace BunningsProductCatalog.Services.Data.CompanyProducts.Validations
{
	public class CompanyProductValidations
	{
		public static List<Error> ValidateProductNameRequired(string input)
		{
			return ValidateRequired("ProductName", "Product name", input);
		}

		public static List<Error> ValidateProductNameMaxLength(string input)
		{
			return ValidateRequiredAndMaxLength("ProductName", "Product name", input, 300);
		}

		public static List<Error> ValidateProductSkuRequired(string input)
		{
			return ValidateRequired("ProductSku", "Product Sku", input);
		}

		public static List<Error> ValidateProductSkuMaxLength(string input)
		{
			return ValidateMaxLength("ProductSku", "Product Sku", input, 50);
		}
	}
}
