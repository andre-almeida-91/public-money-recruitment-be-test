using VacationRental.Domain;
using VacationRental.Infrastructure.Persistence;

namespace VacationRental.Infrastructure.Configuration
{
    public class DatabaseSeeder
    {
        private readonly AppDbContext _appDbContext;

        public DatabaseSeeder(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
        }

        public async Task Seed()
        {
            if (_appDbContext.Rentals.Any() && _appDbContext.Bookings.Any()) return;

            await _appDbContext.AddRangeAsync(GetRentalsList());
            await _appDbContext.AddRangeAsync(GetBookingsList());

            await _appDbContext.SaveChangesAsync();
        }

        public List<Booking> GetBookingsList()
        {
            return new List<Booking> {
                new Booking { Id = 1, RentalId = 1, Start = new DateTime(2023, 2, 22), Nights = 1, CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now },
                new Booking { Id = 2, RentalId = 1, Start = new DateTime(2023, 2, 22), Nights = 2, CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now },
                new Booking { Id = 3, RentalId = 1, Start = new DateTime(2023, 2, 25), Nights = 2, CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now }
            };
        }

        public List<Rental> GetRentalsList()
        {
            return new List<Rental> {
                new Rental { Id = 1, Units = 2, PreparationTimeInDays = 2, CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now },
                new Rental { Id = 2, Units = 4, PreparationTimeInDays = 2, CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now }
            };
        }
    }
}
