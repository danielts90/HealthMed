using HealthMed.Shared.Entities;
using System.Linq.Expressions;

namespace HealthMed.Shared.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : EntityBase
    {
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindByAsync(Expression<Func<T, bool>> predicate);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
    }
}
