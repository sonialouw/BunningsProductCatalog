using BunningsProductCatalog.Services.Data.Common;

namespace BunningsProductCatalog.Services.Data.Companies.Errors
{
	public class CompanyCodeNotFoundError : Error
	{
		public CompanyCodeNotFoundError(string companyCode)
		{
			Field = "CompanyCode";
			Message = $"Company with code of {companyCode} not found.";

		}
	}
}