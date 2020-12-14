namespace BunningsProductCatalog.Services.Data.Common
{
	public class RequiredFieldMissingError : Error
	{
		public RequiredFieldMissingError(string field, string message)
		{
			Field = field;
			Message = message;
		}
	}
}