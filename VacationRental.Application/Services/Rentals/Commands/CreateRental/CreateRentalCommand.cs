using MediatR;

namespace VacationRental.Application.Services.Rentals.Commands.CreateRental
{
    public class CreateRentalCommand : IRequest<CreateRentalResponse>
    {
        public int Units { get; set; }

        public int PreparationTimeInDays { get; set; }
    }
}
