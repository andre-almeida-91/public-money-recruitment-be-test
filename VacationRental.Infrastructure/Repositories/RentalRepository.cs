using Microsoft.EntityFrameworkCore;
using VacationRental.Application.Common.Interfaces.Persistence;
using VacationRental.Application.Common.Interfaces.Repositories.Entities;
using VacationRental.Domain;

namespace VacationRental.Infrastructure.Repositories
{
    public class RentalRepository : IRentalRepository
    {
        private readonly IAppDbContext _appDbContext;

        public RentalRepository(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
        }

        public async Task<Rental> GetAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _appDbContext
                            .Rentals
                            .AsNoTracking()
                            .Where(x => x.Id == id)
                            .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<bool> Exists(int id, CancellationToken cancellationToken = default)
        {
            return await _appDbContext
                            .Rentals
                            .AsNoTracking()
                            .AnyAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<int> InsertAsync(Rental entity, CancellationToken cancellationToken = default)
            => await _appDbContext.InsertAsync(entity, cancellationToken);

        public async Task<int> UpdateAsync(Rental entity, CancellationToken cancellationToken = default)
            => await _appDbContext.UpdateAsync(entity, cancellationToken);

        public async Task<int> DeleteAsync(Rental entity, CancellationToken cancellationToken = default)
            => await _appDbContext.DeleteAsync(entity, cancellationToken);
    }
}
