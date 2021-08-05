using System.Net.Http;
using System;
using System.Threading.Tasks;
using MBD.Transactions.Domain.Interfaces.Services;
using MBD.Transactions.Domain.ValueObjects;
using System.Net;
using System.Text.Json;

namespace MBD.Transactions.Infrastructure.Services
{
    public class BankAccountService : IBankAccountService
    {
        private readonly HttpClient _httpClient;

        public BankAccountService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<BankAccount> GetByIdAsync(Guid id)
        {
            var response = await _httpClient.GetAsync($"/api/accounts/{id}");
            if (response.StatusCode != HttpStatusCode.OK)
                return null;

            var bankAccount = JsonSerializer.Deserialize<BankAccount>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return bankAccount;
        }
    }
}