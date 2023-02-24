using AutoMapper;
using MediatR;
using VacationRental.Application.Common.Interfaces.Repositories.Entities;

namespace VacationRental.Application.Services.Bookings.Queries.GetBooking
{
    public class GetBookingRequestHandler : IRequestHandler<GetBookingQuery, GetBookingResponseDto>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IMapper _mapper;

        public GetBookingRequestHandler(IBookingRepository bookingRepository,
                                       IMapper mapper)
        {
            _bookingRepository = bookingRepository ?? throw new ArgumentNullException(nameof(bookingRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<GetBookingResponseDto> Handle(GetBookingQuery query, CancellationToken cancellationToken)
        {
            var booking = await _bookingRepository.GetAsync(query.BookingId, cancellationToken);

            if (booking == null || booking.Id == 0)
                throw new NullReferenceException("Booking doesn't exist");

            return _mapper.Map<GetBookingResponseDto>(booking);
        }
    }
}
