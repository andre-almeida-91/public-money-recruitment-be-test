using AutoMapper;
using MediatR;
using VacationRental.Application.Common.Interfaces.Repositories.Entities;
using VacationRental.Domain;

namespace VacationRental.Application.Services.Rentals.Commands.CreateRental
{
    public class CreateRentalRequestHandler : IRequestHandler<CreateRentalCommand, CreateRentalResponse>
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly IMapper _mapper;

        public CreateRentalRequestHandler(IRentalRepository rentalRepository, IMapper mapper)
        {
            _rentalRepository = rentalRepository ?? throw new ArgumentNullException(nameof(rentalRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<CreateRentalResponse> Handle(CreateRentalCommand command, CancellationToken cancellationToken)
        {
            var newRental = _mapper.Map<Rental>(command);

            var result = await _rentalRepository.InsertAsync(newRental, cancellationToken);
            if (result == 0)
                throw new InvalidOperationException("Could not create new Rental. Something went wrong");

            return _mapper.Map<CreateRentalResponse>(newRental);
        }
    }
}
