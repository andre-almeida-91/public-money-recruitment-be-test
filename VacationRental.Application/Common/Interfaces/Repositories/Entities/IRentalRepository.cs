using VacationRental.Domain;

namespace VacationRental.Application.Common.Interfaces.Repositories.Entities
{
    public interface IRentalRepository : IBaseRepository<Rental>
    {
        /// <summary>
        /// Gets an entity of type Rental
        /// </summary>
        /// <returns></returns>
        Task<Rental> GetAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Check if a Rental exists in database
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> Exists(int id, CancellationToken cancellationToken = default);
    }
}
