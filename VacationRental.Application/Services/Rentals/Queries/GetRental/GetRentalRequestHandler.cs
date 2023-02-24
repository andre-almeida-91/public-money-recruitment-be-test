using AutoMapper;
using MediatR;
using VacationRental.Application.Common.Interfaces.Repositories.Entities;

namespace VacationRental.Application.Services.Rentals.Queries.GetRental
{
    public class GetRentalRequestHandler : IRequestHandler<GetRentalQuery, GetRentalResponseDto>
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly IMapper _mapper;

        public GetRentalRequestHandler(IRentalRepository rentalRepository,
                                       IMapper mapper)
        {
            _rentalRepository = rentalRepository ?? throw new ArgumentNullException(nameof(rentalRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        public async Task<GetRentalResponseDto> Handle(GetRentalQuery query, CancellationToken cancellationToken)
        {
            var rental = await _rentalRepository.GetAsync(query.RentalId, cancellationToken);

            if (rental == null || rental.Id == 0)
                throw new NullReferenceException("Rental doesn't exist");

            return _mapper.Map<GetRentalResponseDto>(rental);
        }
    }
}
