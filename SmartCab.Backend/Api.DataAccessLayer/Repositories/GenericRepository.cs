using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Castle.Core.Internal;
using CustomExceptions;
using Microsoft.EntityFrameworkCore;

namespace Api.DataAccessLayer.Repositories
{
    /// <summary>
    /// See https://docs.microsoft.com/en-us/aspnet/mvc/overview/older-versions/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        internal ApplicationContext _context;
        internal DbSet<TEntity> _dbSet;

        public GenericRepository(ApplicationContext context)
        {
            this._context = context;
            this._dbSet = context.Set<TEntity>();
        }

        public virtual IEnumerable<TEntity> All()
        {
            return Find();
        }

        public virtual List<TEntity> Find(
            Expression<Func<TEntity, bool>> filter = null)
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return query.ToList();
        }

        public virtual TEntity FindOnlyOne(
            Expression<Func<TEntity, bool>> filter)
        {
            IQueryable<TEntity> query = _dbSet;

            query = query.Where(filter);
            if (query.IsNullOrEmpty() || query.Count() > 2)
            {
               
            }

            return query.First();
        }

        public virtual TEntity FindByID(object id)
        {
            var entity = _dbSet.Find(id);
            if (entity == null)
            {
                throw new UserIdInvalidException("Filter did not result in a unique match");
            }

            return entity;
        }

        public virtual TEntity Add(TEntity entity)
        {
            _dbSet.Add(entity);
            return entity;
        }

        public virtual void Delete(object id)
        {
            TEntity entityToDelete = _dbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToDelete);
            }
            _dbSet.Remove(entityToDelete);
        }

        public virtual TEntity Update(TEntity entityToUpdate)
        {
            if (_context.Entry(entityToUpdate).State == EntityState.Added)
            {
                _dbSet.Remove(entityToUpdate);
                _dbSet.Add(entityToUpdate);
                return entityToUpdate;
            }
            _dbSet.Attach(entityToUpdate);
            _context.Entry(entityToUpdate).State = EntityState.Modified;
            return entityToUpdate;
        }
    }
}
