using BunningsProductCatalog.Services.Data.Common;
using System.Collections.Generic;
using static BunningsProductCatalog.Services.Data.Common.CommonValidationFunctions;

namespace BunningsProductCatalog.Services.Data.Suppliers.Validations
{
	public class SupplierValidations
	{
		public static List<Error> ValidateSupplierNameRequired(string input)
		{
			return ValidateRequired("SupplierName", "Supplier name", input);
		}

		public static List<Error> ValidateSupplierNameMaxLength(string input)
		{
			return ValidateMaxLength("SupplierName", "Supplier name", input, 300);
		}

		public static List<Error> ValidateSupplierCodeRequired(string input)
		{
			return ValidateRequired("SupplierCode", "Supplier code", input);
		}

		public static List<Error> ValidateSupplierCodeMaxLength(string input)
		{
			return ValidateMaxLength("SupplierCode", "Supplier code", input, 50);
		}
	}
}
