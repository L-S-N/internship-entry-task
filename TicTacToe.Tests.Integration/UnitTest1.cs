using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using TicTacToe.Api;
using TicTacToe.Api.Models;
using Xunit;

namespace TicTacToe.Tests.Integration
{
    public class GamesControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public GamesControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task CreateGame_ShouldReturn201AndGameId()
        {
            var requestBody = new
            {
                boardSize = 3,
                winCondition = 3
            };

            var response = await _client.PostAsJsonAsync("/games", requestBody);
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var json = await response.Content.ReadAsStringAsync();
            json.Should().Contain("id");
        }

        [Fact]
        public async Task MakeMove_ShouldUpdateGameState()
        {
            // Arrange
            var requestBody = new { boardSize = 3, winCondition = 3 };
            var gameResponse = await _client.PostAsJsonAsync("/games", requestBody);
            gameResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            var gameJson = await gameResponse.Content.ReadAsStringAsync();
            Console.WriteLine(gameJson);

            var doc = JsonDocument.Parse(gameJson);

            var gameId = doc.RootElement.GetProperty("id").GetString();
            gameId.Should().NotBeNullOrEmpty();

            var playerXStr = doc.RootElement.GetProperty("playerX").GetString();
            var playerX = Guid.Parse(playerXStr);

            // Act
            var movePayload = new MakeMoveRequest
            {
                PlayerId = playerX,
                X = 0,
                Y = 0
            };

            var moveResponse = await _client.PostAsJsonAsync($"games/{gameId}/moves", movePayload);

            // Assert
            moveResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        }


        [Fact]
        public async Task InvalidJson_ShouldReturn400()
        {
            var gameId = Guid.NewGuid();

            var invalidJson = "{ broken json ";
            var content = new StringContent(invalidJson, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync($"/games/{gameId}/moves", content);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            response.Content.Headers.ContentType!.MediaType.Should().Be("application/problem+json");

            var body = await response.Content.ReadAsStringAsync();
            body.Should().Contain("is an invalid start of a property name").And.Contain("\"status\":400");
        }
    }
}