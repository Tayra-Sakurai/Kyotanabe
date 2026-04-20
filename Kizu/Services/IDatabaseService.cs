using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
