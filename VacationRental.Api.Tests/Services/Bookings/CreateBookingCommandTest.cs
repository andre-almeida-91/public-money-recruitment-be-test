using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using VacationRental.Application.Services.Bookings.Commands.CreateBooking;
using VacationRental.Application.Services.Bookings.Entities;
using VacationRental.Application.Services.Bookings.Queries.GetBooking;
using VacationRental.Application.Services.Rentals.Commands.CreateRental;
using VacationRental.Application.Services.Rentals.Entities.Request;
using Xunit;

namespace VacationRental.Api.Tests
{
    [Collection("Integration")]
    public class CreateBookingCommandTest
    {
        private readonly HttpClient _httpClient;

        public CreateBookingCommandTest(IntegrationFixture fixture)
        {
            _httpClient = fixture._client;
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPostBooking_ThenAGetReturnsTheCreatedBooking()
        {
            var createRentalInput = new CreateRentalInput
            {
                Units = 4,
                PreparationTimeInDays = 2
            };

            CreateRentalResponse createRentalResponse = default;
            using (var createRentalRequest = await _httpClient.PostAsJsonAsync($"/api/v1/rentals", createRentalInput))
            {
                Assert.True(createRentalRequest.IsSuccessStatusCode);

                var json = await createRentalRequest.Content.ReadAsStringAsync();
                createRentalResponse = JsonSerializer.Deserialize<CreateRentalResponse>(json);
            }

            var createBookingInput = new CreateBookingInput
            {
                RentalId = createRentalResponse.Id,
                Nights = 3,
                Start = new DateTime(2001, 01, 01)
            };

            CreateBookingResponseDto createBookingResponse = default;
            using (var createBookingRequest = await _httpClient.PostAsJsonAsync($"/api/v1/bookings", createBookingInput))
            {
                Assert.True(createBookingRequest.IsSuccessStatusCode);

                var json = await createBookingRequest.Content.ReadAsStringAsync();
                createBookingResponse = JsonSerializer.Deserialize<CreateBookingResponseDto>(json);
            }

            using (var getBookingRequest = await _httpClient.GetAsync($"/api/v1/bookings/{createBookingResponse.Id}"))
            {
                Assert.True(getBookingRequest.IsSuccessStatusCode);

                var json = await getBookingRequest.Content.ReadAsStringAsync();
                var getBookingResponse = JsonSerializer.Deserialize<GetBookingResponseDto>(json);
                Assert.Equal(createBookingResponse.RentalId, getBookingResponse.RentalId);
                Assert.Equal(createBookingResponse.Nights, getBookingResponse.Nights);
                Assert.Equal(createBookingResponse.Start, getBookingResponse.Start);
            }
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPostBooking_ThenAPostReturnsErrorWhenThereIsOverbooking()
        {
            var createRentalInput = new CreateRentalInput
            {
                Units = 1,
                PreparationTimeInDays = 1
            };

            CreateRentalResponse createRentalResponse = default;
            using (var createRentalRequest = await _httpClient.PostAsJsonAsync($"/api/v1/rentals", createRentalInput))
            {
                Assert.True(createRentalRequest.IsSuccessStatusCode);

                var json = await createRentalRequest.Content.ReadAsStringAsync();
                createRentalResponse = JsonSerializer.Deserialize<CreateRentalResponse>(json);
            }

            var createBookingInput_1 = new CreateBookingInput
            {
                RentalId = createRentalResponse.Id,
                Nights = 3,
                Start = new DateTime(2002, 01, 01)
            };

            using (var postBookingRequest_1 = await _httpClient.PostAsJsonAsync($"/api/v1/bookings", createBookingInput_1))
            {
                Assert.True(postBookingRequest_1.IsSuccessStatusCode);
            }

            var createBookingInput_2 = new CreateBookingInput
            {
                RentalId = createRentalResponse.Id,
                Nights = 1,
                Start = new DateTime(2002, 01, 02)
            };

            await Assert.ThrowsAsync<ApplicationException>(async () =>
            {
                using var postBookingRequest_2 = await _httpClient.PostAsJsonAsync($"/api/v1/bookings", createBookingInput_2);
            });
        }
    }
}
