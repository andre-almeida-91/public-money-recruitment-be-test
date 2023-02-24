using MediatR;

namespace VacationRental.Application.Services.Bookings.Queries.GetBookingsByRental
{
    public class GetBookingsByRentalQuery : IRequest<GetBookingsByRentalResponseDto>
    {
        public int RentalId { get; set; }

        public DateTime? Start { get; set; }
    }
}
