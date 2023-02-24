using VacationRental.Application.Services.Bookings.Entities;

namespace VacationRental.Application.Services.Bookings.Queries.GetBookingsByRental
{
    public class GetBookingsByRentalResponseDto
    {
        public List<BookingDto> Bookings { get; set; }
    }
}