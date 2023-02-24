using AutoMapper;
using MediatR;
using VacationRental.Application.Common.Entities;
using VacationRental.Application.Common.Helpers;
using VacationRental.Application.Services.Bookings.Queries.GetBookingsByRental;
using VacationRental.Application.Services.Calendar.Entities;
using VacationRental.Application.Services.Rentals.Queries.GetRental;

namespace VacationRental.Application.Services.Calendar.Queries.GetCalendarBookingsSimulationByPreparationDays
{
    public class GetCalendarBookingsSimulationRequestHandler : IRequestHandler<GetCalendarBookingsSimulationQuery, GetCalendarBookingsSimulationResponseDto>
    {
        private readonly IMediator _mediator;

        public GetCalendarBookingsSimulationRequestHandler(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<GetCalendarBookingsSimulationResponseDto> Handle(GetCalendarBookingsSimulationQuery query, CancellationToken cancellationToken)
        {
            var rental = await _mediator.Send(new GetRentalQuery { RentalId = query.RentalId }, cancellationToken);
            var rentalBookings = await _mediator.Send(new GetBookingsByRentalQuery { RentalId = rental.Id, Start = query.Start }, cancellationToken);

            var response = new GetCalendarBookingsSimulationResponseDto
            {
                RentalId = rental.Id,
                Dates = new List<CalendarRentalBookingsDto>()
            };

            var unitsInPreparation = new Dictionary<int, int>();
            var bookingOccupiedUnits = new Dictionary<int, int>();

            for (var i = 0; i < query.Nights; i++)
            {
                var date = new CalendarRentalBookingsDto
                {
                    Date = query.Start.Date.AddDays(i),
                    Bookings = new List<EntityDto>(),
                    PreparationTimes = new List<CalendarRentalPreparationTime>()
                };

                // We have no bookings so we skip
                if (rentalBookings == null || rentalBookings.Bookings.Count == 0)
                    continue;

                var currentUnit = 1;

                foreach (var booking in rentalBookings.Bookings)
                {
                    // If we haven't reached the booking date yet we skip
                    if (booking.Start > date.Date)
                        continue;

                    // If we still booked for the current day then we add the booking date and mark the unit as occupied
                    if (booking.Start <= date.Date &&
                        booking.Start.AddDays(booking.Nights) > date.Date)
                    {
                        // If we already have an occupied unit associated with this booking then we reuse it
                        if (bookingOccupiedUnits.ContainsKey(booking.Id))
                            currentUnit = bookingOccupiedUnits[booking.Id];
                        else
                        {
                            // If it doesn't exist we mark this unit as occupied and associate this booking
                            bookingOccupiedUnits.Add(booking.Id, currentUnit);
                        }

                        // Add booking to calendar date
                        date.Bookings.Add(new EntityDto { Id = booking.Id, Unit = currentUnit });
                    }
                    // If the booking date is within preparation date we set it to be locked for preparation
                    else if (booking.Start.AddDays(booking.Nights + query.PreparationTimeInDays) >= date.Date)
                    {
                        // If this unit was not previously marked for preparation
                        if (!unitsInPreparation.ContainsKey(currentUnit))
                        {
                            // Calculate the number of remaining days of preparation based on the number of nights that have passed since the current date
                            var preparationDays = query.PreparationTimeInDays - ArithmeticHelpers.ConvertToPositive((booking.Start.AddDays(booking.Nights) - date.Date).TotalDays);

                            // We set the unit in preparation based on the previous calculation
                            unitsInPreparation.Add(currentUnit, preparationDays);
                        }
                    }

                    // Set a new unit number
                    currentUnit++;
                    // We cannot go higher number than the available rental units. So we rotate it if it goes above
                    if (currentUnit > query.Units)
                        currentUnit = 1;
                }

                // If we have units in preparation
                if (unitsInPreparation.Count > 0)
                    foreach (var unit in unitsInPreparation.ToList())
                    {
                        // If we already reached the last day of preparation for that unit than we remove it
                        if (unit.Value == 0)
                        {
                            unitsInPreparation.Remove(unit.Key);
                            continue;
                        }

                        // Add the preparation date to the calendar for this unit
                        date.PreparationTimes.Add(new CalendarRentalPreparationTime { Unit = unit.Key });
                        // Reduce the days needed for the preparation
                        unitsInPreparation[unit.Key] -= 1;
                    }

                response.Dates.Add(date);
            }

            return response;
        }

    }
}
