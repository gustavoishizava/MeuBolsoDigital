using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using MBD.BankAccounts.API;
using MBD.BankAccounts.Tests.integration.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace MBD.BankAccounts.Tests.integration.Settings
{
    [CollectionDefinition(nameof(IntegrationTestsFixtureCollection))]
    public class IntegrationTestsFixtureCollection : ICollectionFixture<IntegrationTestsFixture<StartupTests>>
    {

    }

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

        public async Task AuthenticateAsync()
        {
            var email = "test@test.com";
            var password = "T3st123@";

            var httpClient = new HttpClient();
            var response = await httpClient.PostAsJsonAsync("https://localhost:5101/api/authentication/auth", new { Email = email, Password = password });
            var accessToken = await DeserializeObjectReponseAsync<AccessTokenResponse>(response);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.AccessToken);
        }

        public void Dispose()
        {
            _client?.Dispose();
            _appFactory?.Dispose();
        }
    }
}