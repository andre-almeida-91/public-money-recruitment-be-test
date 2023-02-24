using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VacationRental.Application.Common.Interfaces.Persistence;
using VacationRental.Domain;
using VacationRental.Domain.Common.Base;

namespace VacationRental.Infrastructure.Persistence
{
    public class AppDbContext : DbContext, IAppDbContext
    {
        private readonly ILogger<AppDbContext> _logger;

        public AppDbContext(DbContextOptions options, ILogger<AppDbContext> logger) : base(options)
        {
            _logger = logger;
        }

        public DbSet<Rental> Rentals { get; set; }

        public DbSet<Booking> Bookings { get; set; }

        public async Task<int> InsertAsync<T>(T entity, CancellationToken cancellationToken = default) where T : class, IEntity
        {
            var dbSet = Set<T>();

            var newId = await dbSet.CountAsync(cancellationToken: cancellationToken);

            entity.Id = newId + 1;

            dbSet.Add(entity);

            return await SaveChangesAsync(cancellationToken);
        }

        public async Task<int> UpdateAsync<T>(T entity, CancellationToken cancellationToken = default) where T : class, IEntity
        {
            Entry(entity).State = EntityState.Modified;

            return await SaveChangesAsync(cancellationToken);
        }

        public async Task<int> DeleteAsync<T>(T entity, CancellationToken cancellationToken = default) where T : class, IEntity
        {
            Entry(entity).State = EntityState.Deleted;

            return await SaveChangesAsync(cancellationToken);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                foreach (var entry in ChangeTracker.Entries<IEntity>())
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:

                            entry.Entity.CreatedOn = DateTime.Now;
                            entry.Entity.ModifiedOn = DateTime.Now;

                            break;

                        case EntityState.Modified:
                        case EntityState.Deleted:

                            entry.Entity.ModifiedOn = DateTime.Now;

                            break;

                        default: break;
                    }
                }

                return await base.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException dbException)
            {
                _logger.LogError("[Database::Update::Exception] Could not update the entities. Something went wrong.", dbException);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("[Database::Exception] An unexpected error occurred.", ex);
                throw;
            }
        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);
        //}
    }
}
