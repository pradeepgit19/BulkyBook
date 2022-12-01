using System.Linq.Expressions;

namespace BulkyBook.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll(string? includeProperties = null);
        void Add(T entity);

        T GetFirstOrDefault(Expression<Func<T, bool>> filter, string? includeProperties = null);

        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entity);

    }
}
