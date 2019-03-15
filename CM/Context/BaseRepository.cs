using CM.Context.Entities.Base;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CM.Context.Repositories.Base
{
    class BaseRepository<TEntity>
        where TEntity : BaseEntity
    {
        internal DbContext context;
        internal DbSet<TEntity> dbSet;

        public BaseRepository(DbContext context)
        {
            this.context = context;
            dbSet = context.Set<TEntity>();
        }

        public virtual DbSet<TEntity> Get(
            /*Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            bool asNoTracking = true*/
            )
        {
            //IQueryable<TEntity> query = dbSet;

            //if (filter != null)
            //{
            //    query = query.Where(filter);
            //}

            //if (asNoTracking)
            //{
            //    query = query.AsNoTracking();
            //}

            //if (orderBy != null)
            //{
            //    return orderBy(query);
            //}
            //else
            //{
            //    return query;
            //}
            return dbSet;
        }

        //public virtual IQueryable<TEntity> Get<TKey>(
        //    Expression<Func<TEntity, bool>> filter = null,
        //    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        //    bool AsNoTracking = true,
        //    params Expression<Func<TEntity, TKey>>[] includes
        //    )
        //{
        //    var query = Get(filter, orderBy, AsNoTracking);

        //    foreach (var property in includes)
        //    {
        //        query = query.Include(property);
        //    }

        //    return query;
        //}

        public virtual TEntity GetByID(object id)
        {
            return dbSet.Find(id);
        }

        public virtual async Task<TEntity> GetByIdAsync(object id)
        {
            return await dbSet.FindAsync(id);
        }

        public virtual TEntity GetByID<Tkey>(object id, params Expression<Func<TEntity, Tkey>>[] include) where Tkey : class
        {
            var entity = dbSet.Find(id);

            if (entity != null)
            {
                foreach (var property in include)
                    context.Entry(entity).Reference(property).Load();
            }
            return entity;
        }

        public virtual void Insert(TEntity entity)
        {
            entity.RegistrationDate = DateTime.Now;
            dbSet.Add(entity);
        }

        public virtual void Delete(TEntity entity)
        {
            if (context.Entry(entity).State == EntityState.Detached)
                dbSet.Attach(entity);

            dbSet.Remove(entity);
        }

        public virtual void Delete(object id)
        {
            TEntity entity = GetByID(id);
            if (entity != null)
            {
                dbSet.Remove(entity);
            }
        }

        public virtual void Update(TEntity entity)
        {
            if (context.Entry(entity).State == EntityState.Detached)
                dbSet.Attach(entity);

            entity.ModificationDate = DateTime.Now;
            context.Entry(entity).State = EntityState.Modified;
        }
    }
}
