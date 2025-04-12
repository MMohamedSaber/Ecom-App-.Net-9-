
using System.Linq.Expressions;

namespace Ecom.Core.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<IReadOnlyList<T>> GetAllAsync(params Expression<Func<T, object>>[] Include );
        Task<T> GetByIdAsync(int Id);
        Task<T> GetByIdAsync(int Id, params Expression<Func<T, object>>[] Include);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);

        Task<int> CountAsync();



    }
}
