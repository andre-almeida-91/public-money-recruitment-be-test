using MediatR;

namespace VacationRental.Application.Services.Calendar.Queries.GetCalendarBookings
{
    public class GetCalendarBookingsQuery : IRequest<GetCalendarBookingsResponseDto>
    {
        public int RentalId { get; set; }

        public DateTime Start { get; set; }

        public int Nights { get; set; }
    }
}
