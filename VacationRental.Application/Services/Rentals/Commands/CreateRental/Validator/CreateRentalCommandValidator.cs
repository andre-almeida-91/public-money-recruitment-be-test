using FluentValidation;

namespace VacationRental.Application.Services.Rentals.Commands.CreateRental.Validator
{
    public class CreateRentalCommandValidator : AbstractValidator<CreateRentalCommand>
    {
        public CreateRentalCommandValidator()
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
