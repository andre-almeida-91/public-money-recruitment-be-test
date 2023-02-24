using AutoMapper;
using MediatR;
using VacationRental.Application.Common.Helpers;
using VacationRental.Application.Common.Interfaces.Repositories.Entities;
using VacationRental.Application.Services.Bookings.Queries.GetBookingsByRental;
using VacationRental.Application.Services.Calendar.Queries.GetCalendarBookingsSimulationByPreparationDays;

namespace VacationRental.Application.Services.Rentals.Commands.UpdateRental
{
    public class UpdateRentalRequestHandler : IRequestHandler<UpdateRentalCommand, UpdateRentalResponse>
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public UpdateRentalRequestHandler(IRentalRepository rentalRepository,
                                          IMapper mapper,
                                          IMediator mediator)
        {
            _rentalRepository = rentalRepository ?? throw new ArgumentNullException(nameof(rentalRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<UpdateRentalResponse> Handle(UpdateRentalCommand command, CancellationToken cancellationToken)
        {
            var rental = await _rentalRepository.GetAsync(command.Id, cancellationToken);
            if (rental == null || rental.Id == 0)
                throw new NullReferenceException("Rental doesn't exist");

            // If the number of preparation days is higher than we had before we need to check for booking overlapping
            if (command.PreparationTimeInDays != rental.PreparationTimeInDays ||
                command.Units != rental.Units)
            {
                var today = DateTime.Now.Date;

                // We fetch all the booking starting today
                var rentalBookings = await _mediator.Send(new GetBookingsByRentalQuery { RentalId = rental.Id, Start = today }, cancellationToken);
                if (rentalBookings != null && rentalBookings.Bookings.Count > 0)
                {
                    // Get the last booking so we can calculate the number of nights to fetch the calendar
                    var lastBookingByStartDate = rentalBookings.Bookings.OrderByDescending(m => m.Start).FirstOrDefault();
                    if (lastBookingByStartDate == null)
                        throw new NullReferenceException($"Somehow we couldn't get the last booking for Rental {command.Id}.");

                    // Calculate number of nights to calendar
                    var lastDate = lastBookingByStartDate.Start.Date.AddDays((lastBookingByStartDate.Nights - 1) + command.PreparationTimeInDays);
                    var numOfNightsToEvaluate = ArithmeticHelpers.ConvertToPositive((today - lastDate).TotalDays);

                    // Fetch the calendar simulated with the new PreparationTimeInDays value
                    var calendar = await _mediator.Send(new GetCalendarBookingsSimulationQuery
                    {
                        RentalId = command.Id,
                        Start = today.Date,
                        Nights = numOfNightsToEvaluate,
                        PreparationTimeInDays = command.PreparationTimeInDays,
                        Units = command.Units
                    }, cancellationToken);

                    // Max units that we can book for the same days
                    var availableUnits = command.Units;

                    // We iterate all days of the simulated calendar to check for overbooking and overlapping of preparation days
                    for (var i = 0; i < numOfNightsToEvaluate; i++)
                    {
                        // We set the current night for validation
                        var currentDay = today.Date.AddDays(i);
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
                            // If we found an over book on the new simulated calendar for a rental we throw an error and we dont update
                            if (totalUnitsUnavailable > availableUnits)
                                throw new ApplicationException($"Cannot change the number of units to {command.Units} or change the preparation days to {command.PreparationTimeInDays}. There is no available units for Rental {command.Id} on day {currentDay.Date:dd-MM-yyyy}.");
                        }
                    }
                }
            }

            rental.PreparationTimeInDays = command.PreparationTimeInDays;
            rental.Units = command.Units;

            var result = await _rentalRepository.UpdateAsync(rental, cancellationToken);
            if (result == 0)
                throw new InvalidOperationException($"Could not update Rental {command.Id}. Something went wrong");

            return _mapper.Map<UpdateRentalResponse>(rental);
        }
    }
}
