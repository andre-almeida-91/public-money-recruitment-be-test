using FluentValidation;
using VacationRental.Application.Common.Interfaces.Repositories.Entities;

namespace VacationRental.Application.Services.Bookings.Queries.GetBookingsByRental.Validator
{
    public class GetBookingsByRentalQueryValidator : AbstractValidator<GetBookingsByRentalQuery>
    {
        private readonly IRentalRepository _rentalRepository;

        public GetBookingsByRentalQueryValidator(IRentalRepository rentalRepository)
        {
            _rentalRepository = rentalRepository;

            RuleFor(m => m.RentalId)
                .NotNull().WithMessage("BookingId cannot be null")
                .Must(m => m > 0).WithMessage("BookingId must be greater than 0.")
                .MustAsync(ExistsRental).WithMessage("Rental not found.");
        }

        internal async Task<bool> ExistsRental(int rentalId, CancellationToken cancellationToken)
        {
            return await _rentalRepository.Exists(rentalId, cancellationToken);
        }
    }
}
