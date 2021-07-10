using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;

namespace MBD.BankAccounts.Tests.integration.Settings
{
    public class IntegrationTestsFixture<TStartup> : IDisposable where TStartup : class
    {
        public HttpClient _client;
        public readonly AppFactory<TStartup> _appFactory;
        private readonly string baseUrl;

        public string emailDefault = "test@test.com";
        public string passwordDefault = "Test3@123";

        public IntegrationTestsFixture()
        {
            baseUrl = "https://localhost:5102";
            
            _appFactory = new AppFactory<TStartup>();
            _client = _appFactory.CreateClient(new WebApplicationFactoryClientOptions
            {
                BaseAddress = new Uri(baseUrl)
            });
        }

        public async Task<T> DeserializeObjectReponseAsync<T>(HttpResponseMessage responseMessage)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<T>(await responseMessage.Content.ReadAsStringAsync(), options);
        }

        public void Dispose()
        {
            _client?.Dispose();
            _appFactory?.Dispose();
        }
    }
}