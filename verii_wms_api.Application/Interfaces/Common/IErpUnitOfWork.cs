using System.Data.Common;
using System.Linq;

namespace WMS_WEBAPI.Interfaces
{
    public interface IErpUnitOfWork
    {
        IQueryable<TEntity> Query<TEntity>(bool tracking = false) where TEntity : class;
        IQueryable<TEntity> SqlQuery<TEntity>(string sql, params object[] parameters) where TEntity : class;
        string? GetConnectionString();
        DbConnection GetDbConnection();
    }
}
