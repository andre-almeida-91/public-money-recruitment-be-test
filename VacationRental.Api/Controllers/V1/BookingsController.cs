using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using VacationRental.Api.Helpers.Exceptions;
using VacationRental.Application.Behaviors.Validation;
using VacationRental.Application.Services.Bookings.Commands.CreateBooking;
using VacationRental.Application.Services.Bookings.Entities;
using VacationRental.Application.Services.Bookings.Queries.GetBooking;
using VacationRental.Application.Services.Calendar.Entities;
using VacationRental.Application.Services.Rentals.Queries.GetRental;

namespace VacationRental.Api.Controllers.V1
{
    /// <summary>
    /// Booking services API
    /// </summary>
    [Route("api/v1/bookings")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly ILogger<RentalsController> _logger;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public BookingsController(ILogger<RentalsController> logger,
                                  IMediator mediator,
                                  IMapper mapper)
        {
            _logger = logger;
            _mediator = mediator;
            _mapper = mapper;
        }

        /// <summary>
        /// Get a Booking by Id
        /// </summary>
        /// <param name="bookingId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{bookingId:int}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(GetRentalResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get(int bookingId, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _mediator.Send(new GetBookingQuery() { BookingId = bookingId }, cancellationToken);

                return Ok(response);
            }
            catch (NullReferenceException ex)
            {
                _logger.LogInformation($"[Booking::Get::NullReferenceException] Message => {ex.Message}", ex);
                return NotFound(ex.Message);
            }
            catch (ValidationException ex)
            {
                _logger.LogInformation($"[Booking::Get::ValidationException] Message => {ex.Message}", ex);
                return BadRequest(ex.Serialize());
            }
            catch (CustomValidationException ex)
            {
                _logger.LogInformation($"[Booking::Get::ValidationException] Message => {ex.Message}", ex);
                return BadRequest(ex.Serialize());
            }
            catch (Exception ex)
            {
                _logger.LogError($"[Booking::Get::Exception] An unexpected error has occurred. Message => {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Create a new booking for a Rental
        /// </summary>
        /// <param name="createBookingInput"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(GetRentalResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] CreateBookingInput createBookingInput, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _mediator.Send(_mapper.Map<CreateBookingCommand>(createBookingInput), cancellationToken);

                return Ok(response);
            }
            catch (NullReferenceException ex)
            {
                _logger.LogInformation($"[Booking::Post::NullReferenceException] Message => {ex.Message}", ex);
                return NotFound(ex.Message);
            }
            catch (ValidationException ex)
            {
                _logger.LogInformation($"[Booking::Post::ValidationException] Message => {ex.Message}", ex);
                return BadRequest(ex.Serialize());
            }
            catch (CustomValidationException ex)
            {
                _logger.LogInformation($"[Booking::Post::ValidationException] Message => {ex.Message}", ex);
                return BadRequest(ex.Serialize());
            }
            catch (Exception ex)
            {
                _logger.LogError($"[Booking::Post::Exception] An unexpected error has occurred. Message => {ex.Message}", ex);
                throw;
            }
        }

        //[HttpPost]
        //public ResourceIdViewModel Post(BookingBindingModel model)
        //{
        //    if (model.Nights <= 0)
        //        throw new ApplicationException("Nigts must be positive");
        //    if (!_rentals.ContainsKey(model.RentalId))
        //        throw new ApplicationException("Rental not found");

        //    for (var i = 0; i < model.Nights; i++)
        //    {
        //        var count = 0;
        //        foreach (var booking in _bookings.Values)
        //        {
        //            if (booking.RentalId == model.RentalId
        //                && 
        //                (booking.Start <= model.Start.Date && booking.Start.AddDays(booking.Nights) > model.Start.Date) || 
        //                (booking.Start < model.Start.AddDays(model.Nights) && booking.Start.AddDays(booking.Nights) >= model.Start.AddDays(model.Nights)) || 
        //                (booking.Start > model.Start && booking.Start.AddDays(booking.Nights) < model.Start.AddDays(model.Nights)))
        //            {
        //                count++;
        //            }
        //        }
        //        if (count >= _rentals[model.RentalId].Units)
        //            throw new ApplicationException("Not available");
        //    }


        //    var key = new ResourceIdViewModel { Id = _bookings.Keys.Count + 1 };

        //    _bookings.Add(key.Id, new BookingViewModel
        //    {
        //        Id = key.Id,
        //        Nights = model.Nights,
        //        RentalId = model.RentalId,
        //        Start = model.Start.Date
        //    });

        //    return key;
        //}
    }
}
