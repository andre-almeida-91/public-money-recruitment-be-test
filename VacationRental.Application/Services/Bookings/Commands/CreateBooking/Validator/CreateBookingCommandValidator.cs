using FluentValidation;
using VacationRental.Application.Common.Interfaces.Repositories.Entities;

namespace VacationRental.Application.Services.Bookings.Commands.CreateBooking.Validator
{
    public class CreateBookingCommandValidator : AbstractValidator<CreateBookingCommand>
    {
        public CreateBookingCommandValidator()
        {
            RuleFor(m => m.Nights)
                .NotNull().WithMessage("Nights cannot be null")
                .Must(m => m > 0).WithMessage("Nights must be greater than 0.");

            RuleFor(m => m.RentalId)
                .NotNull().WithMessage("RentalId cannot be null")
                .Must(m => m > 0).WithMessage("RentalId must be greater than 0.");

            RuleFor(m => m.Start)
                .NotNull().WithMessage("Start Date cannot be null")
                .Must(m => m.Date >= DateTime.Now.Date).WithMessage("Start date cannot be a past date.");
        }
    }
}
