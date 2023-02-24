namespace VacationRental.Application.Services.Bookings.Entities
{
    public class CreateBookingInput
    {
        public int RentalId { get; set; }

        public DateTime Start { get; set; }

        public int Nights { get; set; }
    }
}
