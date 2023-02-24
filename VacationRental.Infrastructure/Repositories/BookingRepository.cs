using Microsoft.EntityFrameworkCore;
using VacationRental.Application.Common.Interfaces.Persistence;
using VacationRental.Application.Common.Interfaces.Repositories.Entities;
using VacationRental.Domain;

namespace VacationRental.Infrastructure.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly IAppDbContext _appDbContext;

        public BookingRepository(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Booking> GetAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _appDbContext
                        .Bookings
                        .AsNoTracking()
                        .Where(b => b.Id == id)
                        .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IEnumerable<Booking>> GetBookingsByRental(int rentalId, DateTime? start = null, CancellationToken cancellationToken = default)
        {
            var rental = await _appDbContext
                                .Rentals
                                .AsNoTracking()
                                .Where(m => m.Id == rentalId)
                                .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            var query = _appDbContext
                            .Bookings
                            .AsNoTracking()
                            .Where(b => b.RentalId == rentalId);

            if (start.HasValue)
            {
                var preparationTimeInDays = rental != null ? rental.PreparationTimeInDays : 0;

                query = query.Where(b => b.Start.Date.AddDays((b.Nights + preparationTimeInDays) - 1) >= start.Value.Date);
            }

            return await query.ToListAsync(cancellationToken);
        }

        public async Task<int> InsertAsync(Booking entity, CancellationToken cancellationToken = default)
            => await _appDbContext.InsertAsync(entity, cancellationToken);

        public async Task<int> UpdateAsync(Booking entity, CancellationToken cancellationToken = default)
            => await _appDbContext.InsertAsync(entity, cancellationToken);

        public async Task<int> DeleteAsync(Booking entity, CancellationToken cancellationToken = default)
            => await _appDbContext.UpdateAsync(entity, cancellationToken);
    }
}
