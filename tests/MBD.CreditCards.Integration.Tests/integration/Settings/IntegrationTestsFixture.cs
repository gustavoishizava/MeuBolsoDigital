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
using MBD.CreditCards.Application.Requests;
using MBD.CreditCards.Application.Responses;
using MBD.CreditCards.Domain.Enumerations;
using MBD.CreditCards.Integration.Tests.integration.Settings.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace MBD.CreditCards.Integration.Tests.integration.Settings
{
    [CollectionDefinition(nameof(IntegrationTestsFixtureCollection))]
    public class IntegrationTestsFixtureCollection : ICollectionFixture<IntegrationTestsFixture<StartupTests>>
    {

    }

    public class IntegrationTestsFixture<TStartup> : IDisposable where TStartup : class
    {
        public HttpClient _client;
        private readonly AppFactory<TStartup> _appFactory;
        private HttpClient _bankAccountClient;
        private readonly AppFactory<MBD.BankAccounts.API.StartupTests> _bankAccountAppFactory;
        private HttpClient _identityClient;
        private readonly AppFactory<MBD.Identity.API.StartupTests> _identityAppFactory;
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

            _bankAccountAppFactory = new AppFactory<MBD.BankAccounts.API.StartupTests>();
            _bankAccountClient = _bankAccountAppFactory.CreateClient(new WebApplicationFactoryClientOptions
            {
                BaseAddress = new Uri(bankAccountUrl)
            });

            _identityAppFactory = new AppFactory<MBD.Identity.API.StartupTests>();
            _identityClient = _identityAppFactory.CreateClient(new WebApplicationFactoryClientOptions
            {
                BaseAddress = new Uri(identityUrl)
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
            _bankAccountClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.AccessToken);
        }

        public async Task<Guid> GetOrCreateBankAccountAsync()
        {
            var id = await GetFirstBankAccountIdAsync();
            if (!id.Equals(Guid.Empty))
                return id;

            var createResponse = await _bankAccountClient.PostAsJsonAsync("/api/accounts",
                            new { Description = "Credit Card Test", InitialBalance = 1000, Type = "CheckingAccount" });
            createResponse.EnsureSuccessStatusCode();

            var createResult = await DeserializeObjectReponseAsync<AccountResponse>(createResponse);
            return createResult.Id;
        }

        private async Task<Guid> GetFirstBankAccountIdAsync()
        {
            var getResponse = await _bankAccountClient.GetAsync("/api/accounts");
            getResponse.EnsureSuccessStatusCode();

            if (getResponse.StatusCode == HttpStatusCode.NoContent)
                return Guid.Empty;

            var result = await DeserializeObjectReponseAsync<List<AccountResponse>>(getResponse);
            return result.First().Id;
        }

        public async Task<CreditCardResponse> CreateCreditCardAsync()
        {
            var request = new CreateCreditCardRequest
            {
                BankAccountId = await GetOrCreateBankAccountAsync(),
                Brand = Brand.VISA,
                Name = "Test",
                ClosingDay = 1,
                DayOfPayment = 5,
                Limit = 1000
            };

            var response = await _client.PostAsJsonAsync("/api/credit-cards", request);
            var result = await DeserializeObjectReponseAsync<CreditCardResponse>(response);

            return result;
        }

        public async Task<CreditCardResponse> GetCreditCardByIdAsync(Guid id)
        {
            var getResponse = await _client.GetAsync($"/api/credit-cards/{id}");
            getResponse.EnsureSuccessStatusCode();

            return await DeserializeObjectReponseAsync<CreditCardResponse>(getResponse);
        }

        public void Dispose()
        {
            _client?.Dispose();
            _bankAccountClient?.Dispose();
            _appFactory?.Dispose();
        }
    }
}