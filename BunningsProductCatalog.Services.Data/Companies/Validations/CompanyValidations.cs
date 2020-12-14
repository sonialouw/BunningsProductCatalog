using BunningsProductCatalog.Services.Data.Common;
using System.Collections.Generic;
using static BunningsProductCatalog.Services.Data.Common.CommonValidationFunctions;

namespace BunningsProductCatalog.Services.Data.CompanyProducts.Companies.Validations
{
	public class CompanyValidations
	{
		public static List<Error> ValidateCompanyCodeRequired(string input)
		{
			return ValidateRequired("CompanyCode", "Company code", input);
		}

	
	}
}
