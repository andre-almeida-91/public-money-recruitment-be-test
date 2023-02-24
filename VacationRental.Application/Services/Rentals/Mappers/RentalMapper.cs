using AutoMapper;
using VacationRental.Application.Services.Rentals.Commands.CreateRental;
using VacationRental.Application.Services.Rentals.Commands.UpdateRental;
using VacationRental.Application.Services.Rentals.Queries.GetRental;
using VacationRental.Domain;

namespace VacationRental.Application.Services.Rentals.Mappers
{
    public class RentalMapper : Profile
    {
        public RentalMapper()
        {
            CreateMap<Rental, GetRentalResponseDto>()
                .ReverseMap()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Rental, CreateRentalCommand>()
                .ReverseMap()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Rental, CreateRentalResponse>()
                .ReverseMap()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Rental, UpdateRentalResponse>()
                .ReverseMap()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
