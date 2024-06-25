using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using ProducerConsumer.Data.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using ProducerConsumer.Data.Entities;

namespace ProducerConsumer.Data.Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        protected readonly ProducerConsumerDbContext DbContext;
        protected readonly DbSet<TEntity> DbSet;

        public GenericRepository(ProducerConsumerDbContext dbContext)
        {
            DbContext = dbContext;
            DbSet = DbContext.Set<TEntity>();
        }

        #region Public member methods

        /// <summary>
        /// Generic Get method for Count
        /// </summary>
        public virtual int Count
        {
            get { return DbSet.Count(); }
        }

        /// <summary>
        /// generic get method , fetches data for the entities on the basis of condition.
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public async Task<TEntity?> Get(Expression<Func<TEntity, Boolean>> where)
        {
            return await DbSet.Where(where).FirstOrDefaultAsync<TEntity>();

        }
        /// <summary>
        /// generic get many method , fetches data for the entities on the basis of condition.
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<TEntity>> GetManyAsync(Expression<Func<TEntity, bool>> where)
        {
            return await DbSet.Where(where).ToListAsync();
        }

        /// <summary>
        /// generic get many method , fetches data for the entities on the basis of condition and page number.
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<TEntity>> GetManyAsync(Expression<Func<TEntity, bool>> where, int pageSize, int totalCount)
        {
            return await DbSet.Where(where).Skip(totalCount).Take(pageSize).ToListAsync();
        }

        /// <summary>
        /// generic get many method , fetches data for the entities on the basis of condition.
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<TEntity>> GetManyAsyncNoTracking(Expression<Func<TEntity, bool>> where)
        {
            return await DbSet.Where(where).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Generic Insert method for the entities
        /// </summary>
        /// <param name="entity"></param>
        public virtual async Task Insert(TEntity entity)
        {
            DbSet.Add(entity);
            await DbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Generic Insert range method for the entities
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public virtual async Task Insert(List<TEntity> entities)
        {
            DbSet.AddRange(entities);
            await DbContext.SaveChangesAsync();
        }


        /// <summary>
        /// Generic Delete method for the entities
        /// </summary>
        /// <param name="entityToDelete"></param>
        public virtual async Task Delete(TEntity entityToDelete)
        {
            if (DbContext.Entry(entityToDelete).State == EntityState.Detached)
            {
                DbSet.Attach(entityToDelete);
            }
            DbSet.Remove(entityToDelete);
            await DbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Delete by Property
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual async Task DeleteWhere(Expression<Func<TEntity, bool>> predicate)
        {
            var entitiesToDelete = await DbSet.Where(predicate).ToListAsync();

            foreach (var entityToDelete in entitiesToDelete)
            {
                if (DbContext.Entry(entityToDelete).State == EntityState.Detached)
                {
                    DbSet.Attach(entityToDelete);
                }
                DbSet.Remove(entityToDelete);
            }

            await DbContext.SaveChangesAsync();
        }


        /// <summary>
        /// Generic update method for the entities
        /// </summary>
        /// <param name="entityToUpdate"></param>
        public virtual async Task Update(TEntity entityToUpdate)
        {
            DbSet.Attach(entityToUpdate);
            DbContext.Entry(entityToUpdate).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
        }


        /// <summary>
        /// Generic update method for the entity list
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public virtual async Task Update(List<TEntity> entities)
        {
            DbSet.UpdateRange(entities);
            await DbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Generic method to check if entity exists
        /// </summary>
        /// <param name="primaryKey"></param>
        /// <returns></returns>
        public async Task<bool> Exists(object primaryKey)
        {
            return await DbSet.FindAsync(primaryKey) != null;
        }

        /// <summary>
        /// generic method to fetch all the records from db
        /// </summary>
        /// <returns></returns>
        public virtual async Task<IEnumerable<TEntity>> GetAll()
        {

            return await DbSet.ToListAsync();
        }

        /// <summary>
        /// Generic method to Get List Data based on Filtering (sorting, Searching, Pagination)
        /// </summary>
        /// <param name="where"></param>
        /// <param name="sortColumn"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        public async Task<IEnumerable<TEntity>> GetAll(Expression<Func<TEntity, bool>> where, string sortColumn, int pageSize, int totalCount)
        {
            if (pageSize > 0 || totalCount > 0)
            {
                return await DbSet.Where(where).OrderBy(sortColumn).Skip(totalCount).Take(pageSize).ToListAsync();
            }
            else
            {
                return await DbSet.Where(where).OrderBy(sortColumn).ToListAsync();
            }
        }

        /// <summary>
        /// Generic Remove methods for Entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task Remove(TEntity entity)
        {
            DbSet.Remove(entity);
            await DbContext.SaveChangesAsync();
        }
        /// <summary>
        /// Generic Remove methods for Entities
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public virtual async Task Remove(List<TEntity> entities)
        {
            DbSet.RemoveRange(entities);
            await DbContext.SaveChangesAsync();
        }

        public virtual async Task<IDbContextTransaction> StartTransaction()
        {
            return await DbContext.Database.BeginTransactionAsync();
        }

        #endregion Public member methods
    }
}
