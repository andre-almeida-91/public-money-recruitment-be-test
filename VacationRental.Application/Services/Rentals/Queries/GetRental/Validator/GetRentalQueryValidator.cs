using FluentValidation;

namespace VacationRental.Application.Services.Rentals.Queries.GetRental.Validator
{
    public class GetRentalQueryValidator : AbstractValidator<GetRentalQuery>
    {
        public GetRentalQueryValidator()
        {
            // Rental Id
            RuleFor(m => m.RentalId)
                .NotNull().WithMessage("Id cannot be null")
                .Must(m => m > 0).WithMessage("Id must be greater than 0.");
        }
    }
}
