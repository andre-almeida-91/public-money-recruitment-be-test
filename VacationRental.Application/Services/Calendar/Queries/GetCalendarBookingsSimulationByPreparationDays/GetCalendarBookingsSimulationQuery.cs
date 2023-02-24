using MediatR;

namespace VacationRental.Application.Services.Calendar.Queries.GetCalendarBookingsSimulationByPreparationDays
{
    public class GetCalendarBookingsSimulationQuery : IRequest<GetCalendarBookingsSimulationResponseDto>
    {
        public int RentalId { get; set; }

        public DateTime Start { get; set; }

        public int Nights { get; set; }

        public int Units { get; set; }

        public int PreparationTimeInDays { get; set; }
    }
}
