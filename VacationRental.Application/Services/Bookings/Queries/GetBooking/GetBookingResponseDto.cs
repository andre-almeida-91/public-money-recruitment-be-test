namespace VacationRental.Application.Services.Bookings.Queries.GetBooking
{
    public class GetBookingResponseDto
    {
        public int Id { get; set; }

        public int RentalId { get; set; }

        public DateTime Start { get; set; }

        public int Nights { get; set; }
    }
}
