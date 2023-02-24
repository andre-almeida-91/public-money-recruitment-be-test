using VacationRental.Domain;

namespace VacationRental.Application.Common.Interfaces.Repositories.Entities
{
    public interface IBookingRepository : IBaseRepository<Booking>
    {
        /// <summary>
        /// Gets an entity of type Booking
        /// </summary>
        /// <returns></returns>
        Task<Booking> GetAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get all Bookings of a Rental
        /// </summary>
        /// <param name="rentalId"></param>
        /// <returns></returns>
        Task<IEnumerable<Booking>> GetBookingsByRental(int rentalId, DateTime? start = null, CancellationToken cancellationToken = default);
    }
}
