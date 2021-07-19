using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using MBD.CreditCards.Domain.Interfaces.Services;
using MBD.CreditCards.Domain.ValueObjects;

namespace MBD.CreditCards.Infrastructure.Services
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