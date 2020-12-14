namespace BunningsProductCatalog.Services.Data.Common
{
	public class InvalidEmailAddressError : Error
	{
		public InvalidEmailAddressError(string field, string emailAddress)
		{
			Field = field;
			Message = $"{emailAddress} is not a valid email address.";
		}
	}
}