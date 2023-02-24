using FluentValidation;

namespace VacationRental.Application.Services.Calendar.Queries.GetCalendarBookings.Validator
{
    public class GetCalendarBookingsQueryValidator : AbstractValidator<GetCalendarBookingsQuery>
    {
        public GetCalendarBookingsQueryValidator()
        {
            // Nights
            RuleFor(m => m.Nights)
                .NotNull().WithMessage("Nights cannot be null")
                .Must(m => m > 0).WithMessage("Nights must be greater than 0.");

            // Rental Id
            RuleFor(m => m.RentalId)
                .NotNull().WithMessage("RentalId cannot be null")
                .Must(m => m > 0).WithMessage("RentalId must be greater than 0.");
        }
    }
}
