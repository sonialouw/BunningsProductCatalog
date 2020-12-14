using System.Collections.Generic;

namespace BunningsProductCatalog.Services.Data.Common
{
	public abstract class BaseResult
	{
		public BaseResult()
		{
			Errors = new List<Error>();
		}

		public bool Success { get; set; }
		public List<Error> Errors { get; set; }
	}
}