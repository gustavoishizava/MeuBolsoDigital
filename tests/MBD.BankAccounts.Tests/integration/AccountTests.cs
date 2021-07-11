using System.Net.Http.Json;
using System.Threading.Tasks;
using MBD.BankAccounts.API;
using MBD.BankAccounts.Application.Request;
using MBD.BankAccounts.Tests.integration.Settings;
using Xunit;

namespace MBD.BankAccounts.Tests.integration
{
    [Collection(nameof(IntegrationTestsFixtureCollection))]
    public class AccountTests
    {
        private readonly IntegrationTestsFixture<StartupTests> _testsFixture;

        public AccountTests(IntegrationTestsFixture<StartupTests> testsFixture)
        {
            _testsFixture = testsFixture;
        }

        [Fact(DisplayName = "Criar uma conta bancária com sucesso."), TestPriority(1)]
        public async Task ValidData_CreateBankAccount_ReturnSuccess()
        {
            // Arrange
            await _testsFixture.AuthenticateAsync();
            var request = new CreateAccountRequest
            {
                Description = "Conta teste",
                InitialBalance = 1000,
                Type = Domain.Enumerations.AccountType.SavingsAccount
            };

            // Act
            var response = await _testsFixture._client.PostAsJsonAsync("/api/accounts", request);

            // Assert
            response.EnsureSuccessStatusCode();
        }
    }
}