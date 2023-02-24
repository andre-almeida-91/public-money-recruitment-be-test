using VacationRental.Application.Services.Calendar.Entities;

namespace VacationRental.Application.Services.Calendar.Queries.GetCalendarBookingsSimulationByPreparationDays
{
    public class GetCalendarBookingsSimulationResponseDto
    {
        public int RentalId { get; set; }

        public List<CalendarRentalBookingsDto> Dates { get; set; }
    }
}
