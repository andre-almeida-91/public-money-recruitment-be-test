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
using VacationRental.Application.Services.Calendar.Queries.GetCalendarBookings;

namespace VacationRental.Api.Controllers.V1
{
    [Route("api/v1/calendar")]
    [ApiController]
    public class CalendarController : ControllerBase
    {
        private readonly ILogger<RentalsController> _logger;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public CalendarController(ILogger<RentalsController> logger,
                                  IMediator mediator,
                                  IMapper mapper)
        {
            _logger = logger;
            _mediator = mediator;
            _mapper = mapper;
        }

        /// <summary>
        /// Get the calendar bookings for a Rental
        /// </summary>
        /// <param name="rentalId"></param>
        /// <param name="start"></param>
        /// <param name="nights"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(GetCalendarBookingsResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get(int rentalId, DateTime start, int nights, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _mediator.Send(
                    new GetCalendarBookingsQuery
                    {
                        RentalId = rentalId,
                        Start = start,
                        Nights = nights
                    }, cancellationToken);

                return Ok(response);
            }
            catch (NullReferenceException ex)
            {
                _logger.LogInformation($"[Calendar::Get::NullReferenceException] Message => {ex.Message}", ex);
                return NotFound(ex.Message);
            }
            catch (ValidationException ex)
            {
                _logger.LogInformation($"[Calendar::Get::ValidationException] Message => {ex.Message}", ex);
                return BadRequest(ex.Serialize());
            }
            catch (CustomValidationException ex)
            {
                _logger.LogInformation($"[Calendar::Get::ValidationException] Message => {ex.Message}", ex);
                return BadRequest(ex.Serialize());
            }
            catch (Exception ex)
            {
                _logger.LogError($"[Calendar::Get::Exception] An unexpected error has occurred. Message => {ex.Message}", ex);
                throw;
            }
        }
    }
}
