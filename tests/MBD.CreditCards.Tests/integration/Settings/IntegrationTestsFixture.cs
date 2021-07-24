using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MBD.CreditCards.API;
using MBD.CreditCards.Tests.integration.Settings.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace MBD.CreditCards.Tests.integration.Settings
{
    [CollectionDefinition(nameof(IntegrationTestsFixtureCollection))]
    public class IntegrationTestsFixtureCollection : ICollectionFixture<IntegrationTestsFixture<StartupTests>>
    {

    }

    public class IntegrationTestsFixture<TStartup> : IDisposable where TStartup : class
    {
        public HttpClient _client;
        private HttpClient _bankAccountClient;
        public readonly AppFactory<TStartup> _appFactory;
        private readonly string baseUrl;
        private readonly string bankAccountUrl = "https://localhost:5102";
        private readonly string identityUrl = "https://localhost:5101";

        public readonly string _creditCardsApi = "/api/credit-cards";

        public IntegrationTestsFixture()
        {
            baseUrl = "https://localhost:5001";

            _appFactory = new AppFactory<TStartup>();
            _client = _appFactory.CreateClient(new WebApplicationFactoryClientOptions
            {
                BaseAddress = new Uri(baseUrl)
            });

            _bankAccountClient = new HttpClient
            {
                BaseAddress = new Uri(bankAccountUrl)
            };
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
            var password = "T3st123@";

            var httpClient = new HttpClient();
            var response = await httpClient.PostAsJsonAsync($"{identityUrl}/api/authentication/auth", new { Email = email, Password = password });
            response.EnsureSuccessStatusCode();
            httpClient.Dispose();

            var accessToken = await DeserializeObjectReponseAsync<AccessTokenResponse>(response);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.AccessToken);
            _bankAccountClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.AccessToken);
        }

        public async Task<Guid> CreateBankAccountAsync()
        {
            var createResponse = await _bankAccountClient.PostAsJsonAsync($"{bankAccountUrl}/api/accounts",
                            new { Description = "Credit Card Test", InitialBalance = 1000, Type = "CheckingAccount" });
            createResponse.EnsureSuccessStatusCode();

            var createResult = await DeserializeObjectReponseAsync<AccountResponse>(createResponse);
            return createResult.Id;
        }

        public async Task<Guid> GetFirstBankAccountIdAsync()
        {
            var getResponse = await _bankAccountClient.GetAsync($"{bankAccountUrl}/api/accounts");
            getResponse.EnsureSuccessStatusCode();

            if (getResponse.StatusCode == HttpStatusCode.NoContent)
                throw new Exception("Nenhuma conta bancária encontrada.");

            var result = await DeserializeObjectReponseAsync<List<AccountResponse>>(getResponse);
            return result.First().Id;
        }

        public void Dispose()
        {
            _client?.Dispose();
            _bankAccountClient?.Dispose();
            _appFactory?.Dispose();
        }
    }
}