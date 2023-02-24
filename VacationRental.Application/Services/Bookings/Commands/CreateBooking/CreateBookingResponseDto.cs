namespace VacationRental.Application.Services.Bookings.Commands.CreateBooking
{
    public class CreateBookingResponseDto
    {
        public int Id { get; set; }

        public int RentalId { get; set; }

        public DateTime Start { get; set; }

        public int Nights { get; set; }
    }
}
