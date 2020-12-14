using System.Collections.Generic;

namespace BunningsProductCatalog.Services.Data.Common
{
	public static class CommonValidationFunctions
	{

		public static List<Error> ValidateRequiredAndMaxLength(string fieldName, string displayName, string value,
			int maxlength)
		{
			var newErrors = new List<Error>();

			newErrors.AddRange(ValidateRequired(fieldName, displayName, value));
			newErrors.AddRange(ValidateMaxLength(fieldName, displayName, value, maxlength));

			return newErrors;
		}

		public static List<Error> ValidateRequired(string fieldName, string displayName, string value)
		{
			var newErrors = new List<Error>();

			if (string.IsNullOrEmpty(value))
			{
				newErrors.Add(new RequiredFieldMissingError(fieldName, $"{displayName} cannot be empty."));
			}

			return newErrors;
		}

		public static List<Error> ValidateMaxLength(string fieldName, string displayName, string value, int maxlength)
		{
			var newErrors = new List<Error>();

			if (!string.IsNullOrEmpty(value) && value.Length > maxlength)
			{
				newErrors.Add(new InvalidStringMaxLengthError(fieldName, displayName, maxlength));
			}

			return newErrors;
		}
	}
}