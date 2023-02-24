using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using VacationRental.Application.Services.Rentals.Commands.CreateRental;
using VacationRental.Application.Services.Rentals.Entities.Request;
using VacationRental.Application.Services.Rentals.Queries.GetRental;
using Xunit;

namespace VacationRental.Api.Tests
{
    [Collection("Integration")]
    public class CreateRentalCommandTest
    {
        private readonly HttpClient _httpClient;

        public CreateRentalCommandTest(IntegrationFixture fixture)
        {
            _httpClient = fixture._client;
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPostRental_ThenAGetReturnsTheCreatedRental()
        {
            var createRentalInput = new CreateRentalInput
            {
                Units = 25,
                PreparationTimeInDays = 1
            };

            CreateRentalResponse createRentalResponse;
            using (var createRentalRequest = await _httpClient.PostAsJsonAsync($"/api/v1/rentals", createRentalInput))
            {
                var json = await createRentalRequest.Content.ReadAsStringAsync();
                createRentalResponse = JsonSerializer.Deserialize<CreateRentalResponse>(json);
            }

            using (var getRentalRequest = await _httpClient.GetAsync($"/api/v1/rentals/{createRentalResponse.Id}"))
            {
                Assert.True(getRentalRequest.IsSuccessStatusCode);

                var json = await getRentalRequest.Content.ReadAsStringAsync();
                var getRentalResponse = JsonSerializer.Deserialize<GetRentalResponseDto>(json);

                Assert.Equal(createRentalResponse.Units, getRentalResponse.Units);
                Assert.Equal(createRentalResponse.PreparationTimeInDays, getRentalResponse.PreparationTimeInDays);
            }
        }
    }
}
