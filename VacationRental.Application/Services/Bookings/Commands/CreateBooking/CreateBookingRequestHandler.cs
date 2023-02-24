using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using VacationRental.Application.Common.Interfaces.Repositories.Entities;
using VacationRental.Application.Services.Bookings.Queries.GetBookingsByRental;
using VacationRental.Application.Services.Calendar.Queries.GetCalendarBookings;
using VacationRental.Application.Services.Rentals.Queries.GetRental;
using VacationRental.Domain;

namespace VacationRental.Application.Services.Bookings.Commands.CreateBooking
{
    public class CreateBookingRequestHandler : IRequestHandler<CreateBookingCommand, CreateBookingResponseDto>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateBookingRequestHandler> _logger;

        public CreateBookingRequestHandler(IBookingRepository bookingRepository,
                                           IMediator mediator,
                                           IMapper mapper,
                                           ILogger<CreateBookingRequestHandler> logger)
        {
            _bookingRepository = bookingRepository ?? throw new ArgumentNullException(nameof(bookingRepository));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        public async Task<CreateBookingResponseDto> Handle(CreateBookingCommand command, CancellationToken cancellationToken)
        {
            // We fetch the Rental
            var rental = await _mediator.Send(new GetRentalQuery { RentalId = command.RentalId }, cancellationToken);

            // Fetch the calendar
            var calendar = await _mediator.Send(new GetCalendarBookingsQuery { RentalId = command.RentalId, Start = command.Start, Nights = command.Nights }, cancellationToken);

            // Max units that we can book for the same days
            var availableUnits = rental.Units;

            // Refactored the booking validation to check how many booking are registered per night in the calendar so we dont overbook.
            // This improves performance since we dont need to evaluate all the bookings available in the system like the legacy code was trying to achieve
            for (var i = 0; i < command.Nights + rental.PreparationTimeInDays; i++)
            {
                // We set the current night for validation
                var currentDay = command.Start.Date.AddDays(i);
                // Fetch the bookings registered for that night
                var currentDayBookings = calendar.Dates.Where(m => m.Date.Date == currentDay).FirstOrDefault();

                // Total of units that are booked + the units that are in preparation
                var totalUnitsUnavailable = 0;
                if (currentDayBookings != null)
                    totalUnitsUnavailable = currentDayBookings.Bookings.Count + currentDayBookings.PreparationTimes.Count;

                // If we dont have available units at the current night its not possible to book
                // So we don't need to evaluate any more nights
                if (totalUnitsUnavailable >= availableUnits)
                {
                    // If somehow we were able to over book for a rental e log that information for later analytics
                    if (totalUnitsUnavailable > availableUnits)
                        _logger.LogWarning($"[Rental::Get::OverBooking] Somehow we discovered an over booking for Rental {command.RentalId} on date {currentDay.Date:dd/MMM/yyyy}");

                    throw new ApplicationException($"There is no available units for Rental {command.RentalId} on day {currentDay.Date:dd-MM-yyyy}");
                }
            }

            // If we don't have an overbooking than we create register the booking
            var newBooking = new Booking
            {
                Nights = command.Nights,
                RentalId = command.RentalId,
                Start = command.Start
            };

            var result = await _bookingRepository.InsertAsync(newBooking, cancellationToken);
            if (result == 0)
                throw new InvalidOperationException("Could not create new new Booking. Something went wrong");

            return _mapper.Map<CreateBookingResponseDto>(newBooking);
        }
    }
}
