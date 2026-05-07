using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Kizu.Services
{
    /// <summary>
    /// Abstract <see cref="DbContext"/> operation service interface.
    /// </summary>
    /// <typeparam name="TContext">The type of the <see cref="DbContext"/>.</typeparam>
    public interface IDatabaseService<TContext>
        where TContext : DbContext
    {
        /// <summary>
        /// Asynchronously retrieves the entities from the selected <see cref="DbSet{T}"/>.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="selector">The selector function.</param>
        /// <returns>The task which returns the list of the retrieved entities.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="selector"/> is null.</exception>
        Task<IEnumerable<T>> GetEntitiesAsync<T>(Func<TContext, DbSet<T>> selector)
            where T : class;

        /// <summary>
        /// Asynchronously deletes the selected entity.
        /// </summary>
        /// <typeparam name="T">The type of entity to be removed.</typeparam>
        /// <param name="entity">The entity to be removed.</param>
        /// <returns>The task representing the asynchronous operation.</returns>
        Task RemoveAsync<T>(T entity)
            where T : class;

        /// <summary>
        /// Asynchronously adds the entity you selected.
        /// </summary>
        /// <typeparam name="T">The type of entity to be added.</typeparam>
        /// <param name="entity">The entity to be added.</param>
        /// <returns>The <see cref="Task"/> which represents the asynchronous operation.</returns>
        Task AddAsync<T>(T entity)
            where T : class;

        /// <summary>
        /// Asynchronously adds the entities you defined.
        /// </summary>
        /// <typeparam name="T">The type of entities to be added.</typeparam>
        /// <param name="entities">The entities to be added.</param>
        /// <returns>The <see cref="Task"/> which represents the asynchronous operation.</returns>
        Task AddAsync<T>(IEnumerable<T> entities)
            where T : class;

        /// <summary>
        /// Asynchronously updates the entity.
        /// </summary>
        /// <typeparam name="T">The type of entity to be updated.</typeparam>
        /// <param name="entity">The entity to be updated.</param>
        /// <returns>The <see cref="Task"/> which represents the asynchronous operation.</returns>
        Task UpdateAsync<T>(T entity)
            where T : class;

        /// <summary>
        /// Checks if <paramref name="entity"/> exists.
        /// </summary>
        /// <typeparam name="T">The type of the entity to be tested.</typeparam>
        /// <param name="entity">The entity to be tested if it exists.</param>
        /// <returns>true if exists; otherwise returns false.</returns>
        bool Exists<T>(T entity)
            where T : class;

        /// <summary>
        /// Synchronously gets the entities in the type of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of entities.</typeparam>
        /// <returns>The list of the entities.</returns>
        List<T> GetEntities<T>()
            where T : class;

        /// <summary>
        /// Gets the entities selected by the <paramref name="selector"/>.
        /// </summary>
        /// <typeparam name="T">The type of entities.</typeparam>
        /// <param name="selector">The selector function.</param>
        /// <returns>The entities.</returns>
        List<T> GetEntities<T>(Expression<Func<TContext, DbSet<T>>> selector)
            where T : class;

        /// <summary>
        /// Asynchronously loads the entities of the related records.
        /// </summary>
        /// <typeparam name="TEntity">The relation parent entity type.</typeparam>
        /// <typeparam name="TRelated">The child entity type.</typeparam>
        /// <param name="entity">The entity to load the children.</param>
        /// <param name="selector">The function to select the relation.</param>
        /// <returns>The task which represents the asynchronous operation.</returns>
        Task LoadCollectionAsync<TEntity, TRelated>(TEntity entity, Expression<Func<TEntity, IEnumerable<TRelated>>> selector)
            where TEntity : class
            where TRelated : class;

        /// <summary>
        /// Asynchronously loads the entity related to the <paramref name="entity"/> selected by <paramref name="selector"/>.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TRelated">The related entity type.</typeparam>
        /// <param name="entity">The entity itself.</param>
        /// <param name="selector">The selector function to get the entity of the related entity.</param>
        /// <returns>The <see cref="Task"/> to represent the asynchronous operation.</returns>
        Task LoadReferenceAsync<TEntity, TRelated>(TEntity entity, Expression<Func<TEntity, TRelated?>> selector)
            where TEntity : class
            where TRelated: class;
    }
}
