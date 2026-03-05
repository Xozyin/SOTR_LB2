using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using SportsNewsAPI.Models;

namespace SportsNewsAPI.Tests
{
    public class SportsNewsApiTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly WebApplicationFactory<Program> _factory;

        public SportsNewsApiTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task TestAdd100Elements()
        {
            // Подсчитываем кол-во до
            var countBeforeResponse = await _client.GetAsync("/api/SportsNews/count");
            var countBefore = await countBeforeResponse.Content.ReadFromJsonAsync<long>();

            const int N = 100;
            long count = 0;

            for (int i = 0; i < N; i++) {
                var sportsNew = GenerateSportsNew(i);
                var response = await _client.PostAsJsonAsync("/api/SportsNews", sportsNew);

                if (response.IsSuccessStatusCode)
                {
                    count++;
                }
            }

            // Подсчитываем кол-во после
            var countAfterResponse = await _client.GetAsync("/api/SportsNews/count");
            var countAfter = await countAfterResponse.Content.ReadFromJsonAsync<long>();

            Assert.Equal(countAfter - countBefore, count);
        }

        [Fact]
        public async Task TestAdd10000Elements()
        {
            // Подсчитываем кол-во до
            var countBeforeResponse = await _client.GetAsync("/api/SportsNews/count");
            var countBefore = await countBeforeResponse.Content.ReadFromJsonAsync<long>();

            const int N = 10000;
            long count = 0;

            for (int i = 0; i < N; i++)
            {
                var sportsNew = GenerateSportsNew(i);
                var response = await _client.PostAsJsonAsync("/api/SportsNews", sportsNew);

                if (response.IsSuccessStatusCode)
                {
                    count++;
                }
            }

            // Подсчитываем кол-во после
            var countAfterResponse = await _client.GetAsync("/api/SportsNews/count");
            var countAfter = await countAfterResponse.Content.ReadFromJsonAsync<long>();

            Assert.Equal(countAfter - countBefore, count);
        }

        [Fact]
        public async Task TestDeleteAll()
        {
            var deleteResponse = await _client.DeleteAsync("/api/SportsNews");
            Assert.Equal(HttpStatusCode.OK, deleteResponse.StatusCode);

            // Подсчитываем кол-во после
            var countAfterResponse = await _client.GetAsync("/api/SportsNews/count");
            var countAfter = await countAfterResponse.Content.ReadFromJsonAsync<long>();
            Assert.Equal(0, countAfter);
        }

        private SportsNewsDtoWithoutID GenerateSportsNew(int i)
        {
            var sources = new[] { "Lenta.RU", "Pervii", "Vestnik Bombasa", "Match TV", "BBC Sport" };
            var random = new Random();

            return (new SportsNewsDtoWithoutID
            {
                Title = $"SportsNews Title {i}",
                Content = $"Test content for Title {i}",
                Date = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-random.Next(1, 30))),
                Source = sources[random.Next(sources.Length)]
            });
        }
    }
}