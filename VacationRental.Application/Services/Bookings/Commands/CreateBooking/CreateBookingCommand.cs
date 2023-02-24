using MediatR;

namespace VacationRental.Application.Services.Bookings.Commands.CreateBooking
{
    public class CreateBookingCommand : IRequest<CreateBookingResponseDto>
    {
        public int RentalId { get; set; }

        public DateTime Start
        {
            get => _startIgnoreTime;
            set => _startIgnoreTime = value.Date;
        }

        private DateTime _startIgnoreTime;

        public int Nights { get; set; }
    }
}
