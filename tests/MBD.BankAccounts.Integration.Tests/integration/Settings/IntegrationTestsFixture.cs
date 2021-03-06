using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MBD.BankAccounts.API;
using MBD.BankAccounts.Application.Request;
using MBD.BankAccounts.Application.Response;
using MBD.BankAccounts.Domain.Enumerations;
using MBD.BankAccounts.Integration.Tests.integration.Models;
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
        private readonly AppFactory<TStartup> _appFactory;
        private HttpClient _identityClient;
        private readonly AppFactory<MBD.Identity.API.StartupTests> _identityAppFactory;
        private readonly string baseUrl;

        public readonly string _accountsApi = "/api/accounts";

        public IntegrationTestsFixture()
        {
            baseUrl = "https://localhost:5001";

            _appFactory = new AppFactory<TStartup>();
            _client = _appFactory.CreateClient(new WebApplicationFactoryClientOptions
            {
                BaseAddress = new Uri(baseUrl)
            });

            _identityAppFactory = new AppFactory<Identity.API.StartupTests>();
            _identityClient = _identityAppFactory.CreateClient(new WebApplicationFactoryClientOptions
            {
                BaseAddress = new Uri("https://localhost:5101")
            });
        }

        public async Task<T> DeserializeObjectReponseAsync<T>(HttpResponseMessage responseMessage)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            options.Converters.Add(new JsonStringEnumConverter());

            return JsonSerializer.Deserialize<T>(await responseMessage.Content.ReadAsStringAsync(), options);
        }

        public async Task AuthenticateAsync()
        {
            var email = "test@test.com";
            var password = "Test3@123";

            var response = await _identityClient.PostAsJsonAsync("/api/authentication/auth", new { Email = email, Password = password });
            response.EnsureSuccessStatusCode();

            var accessToken = await DeserializeObjectReponseAsync<AccessTokenResponse>(response);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.AccessToken);
        }

        public async Task<Guid> CreateAccountAsync()
        {
            var createResponse = await _client.PostAsJsonAsync(_accountsApi,
                            new CreateAccountRequest { Description = "Test", InitialBalance = 100, Type = AccountType.CheckingAccount });
            createResponse.EnsureSuccessStatusCode();

            var createResult = await DeserializeObjectReponseAsync<AccountResponse>(createResponse);
            return createResult.Id;
        }

        public void Dispose()
        {
            _client?.Dispose();
            _appFactory?.Dispose();
        }
    }
}