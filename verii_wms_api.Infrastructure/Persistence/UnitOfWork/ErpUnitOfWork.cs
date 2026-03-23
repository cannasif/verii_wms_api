using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.Data;
using WMS_WEBAPI.Interfaces;

namespace WMS_WEBAPI.UnitOfWork
{
    public class ErpUnitOfWork : IErpUnitOfWork
    {
        private readonly ErpDbContext _context;

        public ErpUnitOfWork(ErpDbContext context)
        {
            _context = context;
        }

        public IQueryable<TEntity> Query<TEntity>(bool tracking = false) where TEntity : class
        {
            var query = _context.Set<TEntity>().AsQueryable();
            return tracking ? query : query.AsNoTracking();
        }

        public IQueryable<TEntity> SqlQuery<TEntity>(string sql, params object[] parameters) where TEntity : class
        {
            return _context.Set<TEntity>()
                .FromSqlRaw(sql, parameters)
                .AsNoTracking();
        }

        public string? GetConnectionString()
        {
            return _context.Database.GetConnectionString();
        }

        public DbConnection GetDbConnection()
        {
            return _context.Database.GetDbConnection();
        }
    }
}
