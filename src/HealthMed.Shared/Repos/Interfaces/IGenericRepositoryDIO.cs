using HealthMed.Shared.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace HealthMed.Shared.Repositories.Interfaces
{
    public interface IGenericRepositoryDIO<T> where T : EntityBase
    {
        T Add(T entity);
        T Update(T entity);
        void Remove(T entity);

        Task<T> GetByIdAsync(int id);
        Task<T> FirstAsync(Expression<Func<T, bool>> expression);
        Task<int> CountAsync(Expression<Func<T, bool>> expression);

        Task<List<T>> GetDataAsync(
            Expression<Func<T, bool>> expression = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            int? skip = null,
            int? take = null);
    }
}
