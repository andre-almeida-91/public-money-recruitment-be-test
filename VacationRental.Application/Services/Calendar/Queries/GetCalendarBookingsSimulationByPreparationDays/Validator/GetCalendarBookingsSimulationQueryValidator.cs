using FluentValidation;

namespace VacationRental.Application.Services.Calendar.Queries.GetCalendarBookingsSimulationByPreparationDays.Validator
{
    public class GetCalendarBookingsSimulationQueryValidator : AbstractValidator<GetCalendarBookingsSimulationQuery>
    {
        public GetCalendarBookingsSimulationQueryValidator()
        {
            // Nights
            RuleFor(m => m.Nights)
                .NotNull().WithMessage("Nights cannot be null")
                .Must(m => m > 0).WithMessage("Nights must be greater than 0.");

            // Rental Id
            RuleFor(m => m.RentalId)
                .NotNull().WithMessage("RentalId cannot be null")
                .Must(m => m > 0).WithMessage("RentalId must be greater than 0.");

            // Rental PreparationTimeInDays
            RuleFor(m => m.PreparationTimeInDays)
                .NotNull().WithMessage("PreparationTimeInDays cannot be null")
                .Must(m => m > 0).WithMessage("PreparationTimeInDays must be greater than 0.");

            // Rental Units
            RuleFor(m => m.Units)
                .NotNull().WithMessage("Units cannot be null")
                .Must(m => m > 0).WithMessage("Units must be greater than 0.");
        }
    }
}
