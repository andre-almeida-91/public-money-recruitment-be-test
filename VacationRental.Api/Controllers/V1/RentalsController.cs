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
using VacationRental.Application.Services.Rentals.Commands.CreateRental;
using VacationRental.Application.Services.Rentals.Commands.UpdateRental;
using VacationRental.Application.Services.Rentals.Entities.Request;
using VacationRental.Application.Services.Rentals.Queries.GetRental;

namespace VacationRental.Api.Controllers.V1
{
    /// <summary>
    /// Rental services API
    /// </summary>
    [Route("api/v1/rentals")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly ILogger<RentalsController> _logger;
        private readonly IMediator _mediator;

        public RentalsController(ILogger<RentalsController> logger,
                                 IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        /// <summary>
        /// Get a Rental by Id
        /// </summary>
        /// <param name="rentalId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{rentalId:int}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(GetRentalResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get([FromRoute] int rentalId, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _mediator.Send(new GetRentalQuery() { RentalId = rentalId }, cancellationToken);

                return Ok(response);
            }
            catch (NullReferenceException ex)
            {
                _logger.LogInformation($"[Rental::Get::NullReferenceException] Message => {ex.Message}", ex);
                return NotFound(ex.Message);
            }
            catch (ValidationException ex)
            {
                _logger.LogInformation($"[Rental::Get::ValidationException] Message => {ex.Message}", ex);
                return BadRequest(ex.Serialize());
            }
            catch (CustomValidationException ex)
            {
                _logger.LogInformation($"[Rental::Get::ValidationException] Message => {ex.Message}", ex);
                return BadRequest(ex.Serialize());
            }
            catch (Exception ex)
            {
                _logger.LogError($"[Rental::Get::Exception] An unexpected error has occurred. Message => {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Creates a new Rental
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(GetRentalResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] CreateRentalInput input, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _mediator.Send(new CreateRentalCommand
                {
                    Units = input.Units,
                    PreparationTimeInDays = input.PreparationTimeInDays
                }, cancellationToken);

                return Ok(response);
            }
            catch (ValidationException ex)
            {
                _logger.LogInformation($"[Rental::Post::ValidationException] Message => {ex.Message}", ex);
                return BadRequest(ex.Serialize());
            }
            catch (CustomValidationException ex)
            {
                _logger.LogInformation($"[Rental::Post::ValidationException] Message => {ex.Message}", ex);
                return BadRequest(ex.Serialize());
            }
            catch (Exception ex)
            {
                _logger.LogError($"[Rental::Post::Exception] An unexpected error has occurred. Message => {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Update an existing Rental
        /// </summary>
        /// <param name="input"></param>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("{id:int}")]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(GetRentalResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put([FromBody] UpdateRentalInput input, [FromRoute] int id, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _mediator.Send(new UpdateRentalCommand
                {
                    Id = id,
                    Units = input.Units,
                    PreparationTimeInDays = input.PreparationTimeInDays
                }, cancellationToken);

                return Ok(response);
            }
            catch (ValidationException ex)
            {
                _logger.LogInformation($"[Rental::Put::ValidationException] Message => {ex.Message}", ex);
                return BadRequest(ex.Serialize());
            }
            catch (CustomValidationException ex)
            {
                _logger.LogInformation($"[Rental::Put::ValidationException] Message => {ex.Message}", ex);
                return BadRequest(ex.Serialize());
            }
            catch (Exception ex)
            {
                _logger.LogError($"[Rental::Put::Exception] An unexpected error has occurred. Message => {ex.Message}", ex);
                throw;
            }
        }
    }
}
