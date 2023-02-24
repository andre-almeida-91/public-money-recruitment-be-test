using MediatR;

namespace VacationRental.Application.Services.Rentals.Queries.GetRental
{
    public class GetRentalQuery : IRequest<GetRentalResponseDto>
    {
        public int RentalId { get; set; }
    }
}
