using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VacationRental.Application.Common.Interfaces.Persistence;
using VacationRental.Application.Common.Interfaces.Repositories.Entities;
using VacationRental.Infrastructure.Persistence;
using VacationRental.Infrastructure.Repositories;

namespace VacationRental.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        /// <summary>
        /// Infrastructure layer service injection
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("VacationRental.DB"));

            services.AddScoped<IRentalRepository, RentalRepository>();
            services.AddScoped<IBookingRepository, BookingRepository>();

            services.AddScoped<IAppDbContext>(provider => provider.GetService<AppDbContext>());

            return services;
        }
    }
}