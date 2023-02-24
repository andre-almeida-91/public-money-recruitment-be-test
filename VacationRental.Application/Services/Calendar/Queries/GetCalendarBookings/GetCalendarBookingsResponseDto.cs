using VacationRental.Application.Services.Calendar.Entities;

namespace VacationRental.Application.Services.Calendar.Queries.GetCalendarBookings
{
    public class GetCalendarBookingsResponseDto
    {
        public int RentalId { get; set; }

        public List<CalendarRentalBookingsDto> Dates { get; set; }
    }
}
