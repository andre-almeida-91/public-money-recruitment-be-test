using AutoMapper;
using VacationRental.Application.Services.Bookings.Commands.CreateBooking;
using VacationRental.Application.Services.Bookings.Entities;
using VacationRental.Application.Services.Bookings.Queries.GetBooking;
using VacationRental.Domain;

namespace VacationRental.Application.Services.Bookings.Mappers
{
    public class BookingMapper : Profile
    {
        public BookingMapper()
        {
            CreateMap<CreateBookingInput, CreateBookingCommand>()
                .ReverseMap()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Booking, GetBookingResponseDto>()
                .ReverseMap()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Booking, CreateBookingResponseDto>()
                .ReverseMap()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Booking, BookingDto>()
                .ReverseMap()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
