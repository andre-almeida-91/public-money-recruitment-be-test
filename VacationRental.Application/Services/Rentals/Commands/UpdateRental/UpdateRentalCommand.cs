using MediatR;

namespace VacationRental.Application.Services.Rentals.Commands.UpdateRental
{
    public class UpdateRentalCommand : IRequest<UpdateRentalResponse>
    {
        public int Id { get; set; }

        public int Units { get; set; }

        public int PreparationTimeInDays { get; set; }
    }
}
