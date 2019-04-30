using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Internal;
using CustomExceptions;
using Microsoft.EntityFrameworkCore;

namespace Api.DataAccessLayer.Repositories
{
    /// <summary>
    /// This class is a generic repository which contains methods that all repositories often use.
    /// </summary>
    /// <remarks>
    /// See https://docs.microsoft.com/en-us/aspnet/mvc/overview/older-versions/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application
    /// </remarks>
    /// <typeparam name="TEntity"></typeparam>
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        internal ApplicationContext _context;
        internal DbSet<TEntity> _dbSet;

        /// <summary>
        /// Constructor for Generic repository, make the Dbset the class works on.
        /// </summary>
        /// <param name="context"></param>
        public GenericRepository(ApplicationContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        /// <summary>
        /// Returns all elements of the given entity
        /// </summary>
        /// <returns></returns>
        public virtual Task<List<TEntity>> AllAsync()
        {
            return FindAsync();
        }

        /// <summary>
        /// Filters all entites, if null parameter returns all.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public virtual Task<List<TEntity>> FindAsync(
            Expression<Func<TEntity, bool>> filter = null)
        {
            IQueryable<TEntity> query = _dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return query.ToListAsync();
        }

        /// <summary>
        /// Returns if only one element matching the filter
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        /// <exception cref="UserIdInvalidException">Filter did not result in a unique match</exception>
        public virtual Task<TEntity> FindOnlyOneAsync(Expression<Func<TEntity, bool>> filter)
        {
            IQueryable<TEntity> query = _dbSet;

            query = query.Where(filter);
            if (query.IsNullOrEmpty() || query.Count() >= 2)
            {
                throw new UserIdInvalidException("Filter did not result in a unique match");
            }

            return query.FirstAsync();
        }

        /// <summary>
        /// Returns the entity with the given primary id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="UserIdInvalidException">No entity with given id</exception> 
        public virtual async Task<TEntity> FindByIDAsync(object id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
            {
                throw new UserIdInvalidException("No entity with given id");
            }

            return entity;
        }

        /// <summary>
        /// Adds the given entity. 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>Returns the added entity</returns>
        public virtual TEntity Add(TEntity entity)
        {
            _dbSet.Add(entity);
            return entity;
        }

        /// <summary>
        /// Deletes the given entity based on Id
        /// </summary>
        /// <param name="id"></param>
        public virtual async Task DeleteAsync(object id)
        {
            TEntity entityToDelete = await FindByIDAsync(id);
            Delete(entityToDelete);
        }

        /// <summary>
        /// Deletes the given entity based on object
        /// </summary>
        /// <param name="entityToDelete"></param>
        public virtual void Delete(TEntity entityToDelete)
        {
            if (_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToDelete);
            }
            _dbSet.Remove(entityToDelete);
        }

        /// <summary>
        /// Updates the given entity
        /// </summary>
        /// <param name="entityToUpdate"></param>
        /// <returns>Returns the entity</returns>
        public virtual TEntity Update(TEntity entityToUpdate)
        {
            if (_context.Entry(entityToUpdate).State == EntityState.Added)
            {
                _dbSet.Remove(entityToUpdate);
                _dbSet.Add(entityToUpdate);
                return entityToUpdate;
            }
            _dbSet.Update(entityToUpdate);
            return entityToUpdate;
        }
    }
}
