using MediatR;

namespace VacationRental.Application.Services.Bookings.Queries.GetBooking
{
    public class GetBookingQuery : IRequest<GetBookingResponseDto>
    {
        public int BookingId { get; set; }
    }
}
