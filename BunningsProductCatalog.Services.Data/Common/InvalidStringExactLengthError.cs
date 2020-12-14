namespace BunningsProductCatalog.Services.Data.Common
{
	public class InvalidStringExactLengthError : Error
	{
		public InvalidStringExactLengthError(string field, int length)
		{
			Field = field;
			Message = $"{field} must be exactly {length} characters long.";
		}
	}
}