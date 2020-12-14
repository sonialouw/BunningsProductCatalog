namespace BunningsProductCatalog.Services.Data.Common
{
	public class InvalidStringMinLengthError : Error
	{
		public InvalidStringMinLengthError(string field, int length)
		{
			Field = field;
			Message = $"{field} must be at least {length} characters long.";
		}
	}
}