using Microsoft.EntityFrameworkCore;
using VacationRental.Domain;
using VacationRental.Domain.Common.Base;

namespace VacationRental.Application.Common.Interfaces.Persistence
{
    public interface IAppDbContext
    {
        /// <summary>
        /// Creates a new entity of type <see cref="{T}" />
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> InsertAsync<T>(T entity, CancellationToken cancellationToken) where T : class, IEntity;

        /// <summary>
        /// Modifies an existing entity of type <see cref="{T}" />
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> UpdateAsync<T>(T entity, CancellationToken cancellationToken) where T : class, IEntity;

        /// <summary>
        /// Deletes an entity of type <see cref="{T}" />
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> DeleteAsync<T>(T entity, CancellationToken cancellationToken) where T : class, IEntity;

        /// <summary>
        /// Persists all changes on database
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Rentals datatable
        /// </summary>
        public DbSet<Rental> Rentals { get; set; }

        /// <summary>
        /// Bookings datatable
        /// </summary>
        public DbSet<Booking> Bookings { get; set; }
    }
}