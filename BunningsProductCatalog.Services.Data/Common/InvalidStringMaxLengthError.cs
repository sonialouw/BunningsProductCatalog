namespace BunningsProductCatalog.Services.Data.Common
{
	public class InvalidStringMaxLengthError : Error
	{
		public InvalidStringMaxLengthError(string fieldName, string displayName, int maxlength)
		{
			Field = fieldName;
			Message = $"{displayName} exceeds the maximum length of {maxlength} characters.";
		}
	}
}