using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProducerConsumer.Data.IRepository
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        #region Method Declaration

        int Count { get; }
        Task<TEntity?> Get(Expression<Func<TEntity, Boolean>> where);
        Task<IEnumerable<TEntity>> GetManyAsync(Expression<Func<TEntity, bool>> where);
        Task<IEnumerable<TEntity>> GetManyAsync(Expression<Func<TEntity, bool>> where, int pageSize, int totalCount);
        Task<IEnumerable<TEntity>> GetManyAsyncNoTracking(Expression<Func<TEntity, bool>> where);
        Task<IEnumerable<TEntity>> GetAll(Expression<Func<TEntity, bool>> where, string sortColumn, int pageSize, int totalCount);
        Task<IEnumerable<TEntity>> GetAll();
        Task Insert(TEntity entity);
        Task Insert(List<TEntity> entities);
        Task Update(TEntity entityToUpdate);
        Task Update(List<TEntity> entities);
        Task<bool> Exists(object primaryKey);
        Task Delete(TEntity entityToDelete);
        Task DeleteWhere(Expression<Func<TEntity, bool>> predicate);
        Task Remove(TEntity entity);
        Task Remove(List<TEntity> entities);
        Task<IDbContextTransaction> StartTransaction();

        #endregion
    }
}
