using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Net.Http;
using Xunit;

namespace VacationRental.Api.Tests
{
    [CollectionDefinition("Integration")]
    public sealed class IntegrationFixture : IDisposable, ICollectionFixture<IntegrationFixture>
    {
        private readonly TestServer _server;

        public HttpClient _client { get; }

        public IntegrationFixture()
        {
            _server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            _client = _server.CreateClient();
        }

        public void Dispose()
        {
            _client.Dispose();
            _server.Dispose();
        }
    }
}
