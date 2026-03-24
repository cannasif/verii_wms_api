using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Repositories
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        IQueryable<T> Query(bool tracking = false, bool ignoreQueryFilters = false);
        Task<T?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
        Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
        Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
        Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
        void Update(T entity);
        Task SoftDelete(long id, CancellationToken cancellationToken = default);
        void SoftDeleteRange(IEnumerable<long> ids);
        Task<bool> ExistsAsync(long id, CancellationToken cancellationToken = default);
        Task<int> CountAsync(CancellationToken cancellationToken = default);
    }
}
