using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Security.Claims;
using WMS_WEBAPI.Data;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        protected readonly WmsDbContext _context;
        protected readonly DbSet<T> _dbSet;
        protected readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRequestCancellationAccessor _requestCancellationAccessor;

        public GenericRepository(
            WmsDbContext context,
            IHttpContextAccessor httpContextAccessor,
            IRequestCancellationAccessor requestCancellationAccessor)
        {
            _context = context;
            _dbSet = context.Set<T>();
            _httpContextAccessor = httpContextAccessor;
            _requestCancellationAccessor = requestCancellationAccessor;
        }

        private CancellationToken ResolveCancellationToken(CancellationToken cancellationToken = default)
        {
            return _requestCancellationAccessor.Get(cancellationToken);
        }
        private long? GetCurrentUserId()
        {
            // Prefer standard NameIdentifier claim; fallback to custom "UserId" if present
            var httpUser = _httpContextAccessor.HttpContext?.User;
            var userIdClaim = httpUser?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                              ?? httpUser?.FindFirst("UserId")?.Value;
            return long.TryParse(userIdClaim, out var userId) ? userId : null;
        }

        public IQueryable<T> Query(bool tracking = false, bool ignoreQueryFilters = false)
        {
            IQueryable<T> query = _dbSet;

            if (!ignoreQueryFilters)
            {
                query = query.Where(e => !e.IsDeleted);
            }

            if (!tracking)
            {
                query = query.AsNoTracking();
            }

            return query;
        }
        
        public async Task<T?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            cancellationToken = ResolveCancellationToken(cancellationToken);

            return await _dbSet
                .Include(e => e.CreatedByUser)
                .Include(e => e.UpdatedByUser)
                .Include(e => e.DeletedByUser)
                .AsNoTracking()
                .Where(e => e.Id == id && !e.IsDeleted)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken = ResolveCancellationToken(cancellationToken);

            return await _dbSet
                .Where(e => !e.IsDeleted)
                .Include(e => e.CreatedByUser)
                .Include(e => e.UpdatedByUser)
                .Include(e => e.DeletedByUser)
                .Where(e => !e.IsDeleted)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default)
        {
            cancellationToken = ResolveCancellationToken(cancellationToken);

            return await _dbSet
                .Include(e => e.CreatedByUser)
                .Include(e => e.UpdatedByUser)
                .Include(e => e.DeletedByUser)
                .Where(expression)
                .Where(e => !e.IsDeleted)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default)
        {
            cancellationToken = ResolveCancellationToken(cancellationToken);

            return await _dbSet
                .Include(e => e.CreatedByUser)
                .Include(e => e.UpdatedByUser)
                .Include(e => e.DeletedByUser)
                .Where(expression)
                .Where(e => !e.IsDeleted)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            cancellationToken = ResolveCancellationToken(cancellationToken);
            entity.CreatedDate = DateTimeProvider.Now;
            var userId = GetCurrentUserId();
            entity.CreatedBy = userId;
            entity.IsDeleted = false;

            await _dbSet.AddAsync(entity, cancellationToken);
            return entity;
        }

        public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            cancellationToken = ResolveCancellationToken(cancellationToken);
            foreach (var entity in entities)
            {
                entity.CreatedDate = DateTimeProvider.Now;
                var userId = GetCurrentUserId();
                entity.CreatedBy = userId;
                entity.IsDeleted = false;
            }
            await _dbSet.AddRangeAsync(entities, cancellationToken);
        }

        public async Task SoftDelete(long id, CancellationToken cancellationToken = default)
        {
            cancellationToken = ResolveCancellationToken(cancellationToken);

            var entity = await _dbSet.FindAsync(new object[] { id }, cancellationToken);
            if (entity != null)
            {
                entity.IsDeleted = true;
                entity.DeletedDate = DateTimeProvider.Now;
                var userId = GetCurrentUserId();
                entity.DeletedBy = userId;
                
                _dbSet.Update(entity);
            }
        }


        public void SoftDeleteRange(IEnumerable<long> ids)
        {
            var userId = GetCurrentUserId();
            var entities = _dbSet.Where(e => ids.Contains(e.Id)).ToList();
            
            foreach (var entity in entities)
            {
                entity.IsDeleted = true;
                entity.DeletedDate = DateTimeProvider.Now;
                entity.DeletedBy = userId;
            }
        }

        public void Update(T entity)
        {
            entity.UpdatedDate = DateTimeProvider.Now;
            var userId = GetCurrentUserId();
            entity.UpdatedBy = userId;
            _dbSet.Update(entity);
        }

        public async Task<bool> ExistsAsync(long id, CancellationToken cancellationToken = default)
        {
            cancellationToken = ResolveCancellationToken(cancellationToken);

            return await _dbSet.Where(e => e.Id == id && !e.IsDeleted)
                .AnyAsync(cancellationToken);
        }

        public async Task<int> CountAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken = ResolveCancellationToken(cancellationToken);

            return await _dbSet.Where(e => !e.IsDeleted)
                .CountAsync(cancellationToken);
        }

    }
}
