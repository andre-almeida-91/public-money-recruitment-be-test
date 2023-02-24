using VacationRental.Domain.Common.Base;

namespace VacationRental.Application.Common.Interfaces.Repositories
{
    public interface IBaseRepository<T> where T : class, IEntity
    {
        /// <summary>
        /// Creates a new entity of type <see cref="{T}" />
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> InsertAsync(T entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Modifies an existing entity of type <see cref="{T}" />
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(T entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes an entity of type <see cref="{T}" />
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(T entity, CancellationToken cancellationToken = default);
    }
}
