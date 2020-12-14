namespace BunningsProductCatalog.Services.Data.Common
{
	public class ExceptionError : Error
	{
		public ExceptionError(string field, string message)
		{
			Field = field;
			Message = message;
		}

		public ExceptionError(string message)
		{
			Field = string.Empty;
			Message = message;
		}
	}
}