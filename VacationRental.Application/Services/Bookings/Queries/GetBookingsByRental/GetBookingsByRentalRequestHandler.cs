using AutoMapper;
using MediatR;
using VacationRental.Application.Common.Interfaces.Repositories.Entities;
using VacationRental.Application.Services.Bookings.Entities;

namespace VacationRental.Application.Services.Bookings.Queries.GetBookingsByRental
{
    public class GetBookingsByRentalRequestHandler : IRequestHandler<GetBookingsByRentalQuery, GetBookingsByRentalResponseDto>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IMapper _mapper;

        public GetBookingsByRentalRequestHandler(IBookingRepository bookingRepository,
                                                 IMapper mapper)
        {
            _bookingRepository = bookingRepository ?? throw new ArgumentNullException(nameof(bookingRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        public async Task<GetBookingsByRentalResponseDto> Handle(GetBookingsByRentalQuery query, CancellationToken cancellationToken)
        {
            var bookings = await _bookingRepository.GetBookingsByRental(query.RentalId, query.Start, cancellationToken);

            var response = new GetBookingsByRentalResponseDto
            {
                Bookings = _mapper.Map<List<BookingDto>>(bookings)
            };

            return response;
        }
    }
}
