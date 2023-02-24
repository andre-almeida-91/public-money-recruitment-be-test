using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using VacationRental.Application.Services.Bookings.Commands.CreateBooking;
using VacationRental.Application.Services.Bookings.Entities;
using VacationRental.Application.Services.Calendar.Queries.GetCalendarBookings;
using VacationRental.Application.Services.Rentals.Commands.CreateRental;
using VacationRental.Application.Services.Rentals.Entities.Request;
using Xunit;

namespace VacationRental.Api.Tests
{
    [Collection("Integration")]
    public class GetCalendarQueryTest
    {
        private readonly HttpClient _httpClient;

        public GetCalendarQueryTest(IntegrationFixture fixture)
        {
            _httpClient = fixture._client;
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenGetCalendar_ThenAGetReturnsTheCalculatedCalendar()
        {
            var createRentalInput = new CreateRentalInput
            {
                Units = 2,
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
                Nights = 2,
                Start = DateTime.Today.AddDays(1)
            };

            CreateBookingResponseDto createBookingResponse_1 = default;
            using (var createBookingRequest_1 = await _httpClient.PostAsJsonAsync($"/api/v1/bookings", createBookingInput_1))
            {
                Assert.True(createBookingRequest_1.IsSuccessStatusCode);

                var json = await createBookingRequest_1.Content.ReadAsStringAsync();
                createBookingResponse_1 = JsonSerializer.Deserialize<CreateBookingResponseDto>(json);
            }

            var createBookingInput_2 = new CreateBookingInput
            {
                RentalId = createRentalResponse.Id,
                Nights = 2,
                Start = DateTime.Today.AddDays(2)
            };

            CreateBookingResponseDto createBookingResponse_2 = default;
            using (var createBookingRequest_2 = await _httpClient.PostAsJsonAsync($"/api/v1/bookings", createBookingInput_2))
            {
                Assert.True(createBookingRequest_2.IsSuccessStatusCode);

                var json = await createBookingRequest_2.Content.ReadAsStringAsync();
                createBookingResponse_2 = JsonSerializer.Deserialize<CreateBookingResponseDto>(json);
            }

            var firstDay = DateTime.Today;
            using (var getCalendarRequest = await _httpClient.GetAsync($"/api/v1/calendar?rentalId={createRentalResponse.Id}&start={firstDay.Year}-{firstDay.Month}-{firstDay.Day}&nights=5"))
            {
                Assert.True(getCalendarRequest.IsSuccessStatusCode);

                var json = await getCalendarRequest.Content.ReadAsStringAsync();
                var getCalendarResponse = JsonSerializer.Deserialize<GetCalendarBookingsResponseDto>(json);

                Assert.Equal(createRentalResponse.Id, getCalendarResponse.RentalId);
                Assert.Equal(5, getCalendarResponse.Dates.Count);

                Assert.Equal(new DateTime(firstDay.Year, firstDay.Month, firstDay.Day), getCalendarResponse.Dates[0].Date);
                Assert.Empty(getCalendarResponse.Dates[0].Bookings);

                var secondNight = DateTime.Today.AddDays(1);
                Assert.Equal(new DateTime(secondNight.Year, secondNight.Month, secondNight.Day), getCalendarResponse.Dates[1].Date);
                Assert.Single(getCalendarResponse.Dates[1].Bookings);
                Assert.Contains(getCalendarResponse.Dates[1].Bookings, x => x.Id == createBookingResponse_1.Id);

                var thirdNight = DateTime.Today.AddDays(2);
                Assert.Equal(new DateTime(thirdNight.Year, thirdNight.Month, thirdNight.Day), getCalendarResponse.Dates[2].Date);
                Assert.Equal(2, getCalendarResponse.Dates[2].Bookings.Count);
                Assert.Contains(getCalendarResponse.Dates[2].Bookings, x => x.Id == createBookingResponse_1.Id);
                Assert.Contains(getCalendarResponse.Dates[2].Bookings, x => x.Id == createBookingResponse_2.Id);

                var forthNight = DateTime.Today.AddDays(3);
                Assert.Equal(new DateTime(forthNight.Year, forthNight.Month, forthNight.Day), getCalendarResponse.Dates[3].Date);
                Assert.Single(getCalendarResponse.Dates[3].Bookings);
                Assert.Single(getCalendarResponse.Dates[3].PreparationTimes);
                Assert.Contains(getCalendarResponse.Dates[3].Bookings, x => x.Id == createBookingResponse_2.Id);

                var fifthNight = DateTime.Today.AddDays(4);
                Assert.Equal(new DateTime(fifthNight.Year, fifthNight.Month, fifthNight.Day), getCalendarResponse.Dates[4].Date);
                Assert.Empty(getCalendarResponse.Dates[4].Bookings);
                Assert.Single(getCalendarResponse.Dates[4].PreparationTimes);
            }
        }
    }
}
