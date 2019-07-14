using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Api.DataAccessLayer.Repositories
{
    /// <summary>
    /// See https://docs.microsoft.com/en-us/aspnet/mvc/overview/older-versions/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<List<TEntity>> AllAsync();

        Task<List<TEntity>> FindAsync(
            Expression<Func<TEntity, bool>> filter = null);

        Task<TEntity> FindOnlyOneAsync(
            Expression<Func<TEntity, bool>> filter);

        Task<TEntity> FindByIDAsync(object id);

        TEntity Add(TEntity entity);

        Task DeleteAsync(object id);

        void Delete(TEntity entityToDelete);

        TEntity Update(TEntity entityToUpdate);
    }
}
