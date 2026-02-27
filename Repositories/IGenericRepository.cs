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
        Task<T?> GetByIdAsync(long id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
        Task<T> AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        void Update(T entity);
        Task SoftDelete(long id);
        void SoftDeleteRange(IEnumerable<long> ids);
        Task<bool> ExistsAsync(long id);
        Task<int> CountAsync();
        IQueryable<T> AsQueryable();
    }
}