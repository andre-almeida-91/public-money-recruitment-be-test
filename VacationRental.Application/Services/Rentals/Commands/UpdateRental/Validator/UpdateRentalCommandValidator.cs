using FluentValidation;

namespace VacationRental.Application.Services.Rentals.Commands.UpdateRental.Validator
{
    public class UpdateRentalCommandValidator : AbstractValidator<UpdateRentalCommand>
    {
        public UpdateRentalCommandValidator()
        {
            // Rental Units
            RuleFor(m => m.Units)
                .NotNull().WithMessage("Units cannot be null")
                .Must(m => m > 0).WithMessage("Units must be greater than 0.");

            // Rental PreparationTimeInDays
            RuleFor(m => m.PreparationTimeInDays)
                .NotNull().WithMessage("PreparationTimeInDays cannot be null")
                .Must(m => m > 0).WithMessage("PreparationTimeInDays must be greater than 0.");
        }
    }
}
