using System.Linq;

namespace BunningsProductCatalog.Domain.Repository
{
	public interface IRepository<T> where T : class
	{
		IQueryable<T> GetAll();
		T GetById(object id);
		void Add(T entity);
		void Update(T entity);
		void Delete(T entity);
		void Delete(int id);
	}
}