using FluentValidation;

namespace VacationRental.Application.Services.Bookings.Queries.GetBooking.Validator
{
    public class GetBookingQueryValidator : AbstractValidator<GetBookingQuery>
    {
        public GetBookingQueryValidator()
        {
            // Booking Id
            RuleFor(m => m.BookingId)
                .NotNull().WithMessage("BookingId cannot be null")
                .Must(m => m > 0).WithMessage("BookingId must be greater than 0.");
        }
    }
}
