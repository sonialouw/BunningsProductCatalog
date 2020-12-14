using BunningsProductCatalog.Domain.Repository;
using Microsoft.Extensions.Logging;

namespace BunningsProductCatalog.Services.Common
{
	public abstract class BaseService
	{
		protected BaseService(IUnitOfWork uow, ILogger logger)
		{
			UoW = uow;
			Logger = logger;
		}

		protected IUnitOfWork UoW { get; }
		protected ILogger Logger { get; }

	}
}
