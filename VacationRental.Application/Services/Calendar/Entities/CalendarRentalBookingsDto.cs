using VacationRental.Application.Common.Entities;

namespace VacationRental.Application.Services.Calendar.Entities
{
    public class CalendarRentalBookingsDto
    {
        public DateTime Date { get; set; }

        public List<EntityDto> Bookings { get; set; }

        public List<CalendarRentalPreparationTime> PreparationTimes { get; set; }
    }
}
