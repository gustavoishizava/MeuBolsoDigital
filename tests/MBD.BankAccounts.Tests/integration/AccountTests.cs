using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using MBD.BankAccounts.API;
using MBD.BankAccounts.API.Models;
using MBD.BankAccounts.Application.Request;
using MBD.BankAccounts.Application.Response;
using MBD.BankAccounts.Domain.Enumerations;
using MBD.BankAccounts.Tests.integration.Settings;
using MBD.Core.Enumerations;
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

        [Fact(DisplayName = "Criar uma conta bancária com sucesso.")]
        public async Task ValidData_CreateBankAccount_ReturnSuccess()
        {
            // Arrange
            await _testsFixture.AuthenticateAsync();
            var request = new CreateAccountRequest
            {
                Description = "Conta teste",
                InitialBalance = 1000,
                Type = AccountType.SavingsAccount
            };

            SuccessModel<Guid> result = null;

            // Act
            var response = await _testsFixture._client.PostAsJsonAsync(_testsFixture._accountsApi, request);
            result = await _testsFixture.DeserializeObjectReponseAsync<SuccessModel<Guid>>(response);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.NotNull(result);
        }

        [Theory(DisplayName = "Criar uma conta bancária com dados inválidos deve retornar erros.")]
        [InlineData("", -1, 2)]
        [InlineData("NuBank", -10, 1)]
        public async Task InvalidData_CreateBankAccount_ReturnErrors(string description, decimal initialBalance, int numberOfErros)
        {
            // Arrange
            await _testsFixture.AuthenticateAsync();
            var request = new CreateAccountRequest
            {
                Description = description,
                InitialBalance = initialBalance,
                Type = AccountType.Money
            };
            ErrorModel result = null;

            // Act
            var response = await _testsFixture._client.PostAsJsonAsync(_testsFixture._accountsApi, request);
            result = await _testsFixture.DeserializeObjectReponseAsync<ErrorModel>(response);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotNull(result);
            Assert.Equal(numberOfErros, result.Errors.Count);
        }

        [Fact(DisplayName = "Alterar uma conta bancária existente com sucesso.")]
        public async Task ValidData_UpdateAccount_ReturnSuccess()
        {
            // Arrange
            await _testsFixture.AuthenticateAsync();

            var id = await _testsFixture.CreateAccountAsync();

            var request = new UpdateAccountRequest
            {
                Id = id,
                Description = "Conta teste update",
                Type = AccountType.SavingsAccount,
                Status = Status.Active
            };

            // Act
            var response = await _testsFixture._client.PutAsJsonAsync(_testsFixture._accountsApi, request);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Theory(DisplayName = "Alterar uma conta bancária existente com dados inválidos deve retornar erro.")]
        [InlineData(true)]
        [InlineData(false)]
        public async Task InvalidData_UpdateAccount_ReturnError(bool invalidId)
        {
            // Arrange
            await _testsFixture.AuthenticateAsync();
            var id = Guid.NewGuid();

            if (!invalidId)
                id = await _testsFixture.CreateAccountAsync();

            var request = new UpdateAccountRequest
            {
                Id = id,
                Description = "",
                Type = Domain.Enumerations.AccountType.SavingsAccount,
                Status = Status.Active
            };

            ErrorModel result = null;

            // Act
            var response = await _testsFixture._client.PutAsJsonAsync(_testsFixture._accountsApi, request);
            result = await _testsFixture.DeserializeObjectReponseAsync<ErrorModel>(response);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotNull(result);
            Assert.Single(result.Errors);
        }

        [Fact(DisplayName = "Deletar conta com Id válido deve retornar sucesso.")]
        public async Task ValidId_DeleteAccount_ReturnSuccess()
        {
            // Arrange
            await _testsFixture.AuthenticateAsync();
            var id = await _testsFixture.CreateAccountAsync();

            // Act
            var response = await _testsFixture._client.DeleteAsync($"{_testsFixture._accountsApi}/{id}");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact(DisplayName = "Deletar conta com Id inválido deve retornar erro.")]
        public async Task InvalidId_DeleteAccount_ReturnError()
        {
            // Act
            var response = await _testsFixture._client.DeleteAsync($"{_testsFixture._accountsApi}/{Guid.NewGuid()}");

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory(DisplayName = "Obter conta bancária com Id válido deve retornar sucesso")]
        [InlineData("NuBank", 150.55, AccountType.CheckingAccount)]
        [InlineData("Banco do Brasil", 555.55, AccountType.Investment)]
        [InlineData("Santander", 249.90, AccountType.Others)]
        [InlineData("Itaú", 10.50, AccountType.SavingsAccount)]
        public async Task ValidId_GetAccount_ReturnSuccess(string description, decimal balance, AccountType type)
        {
            // Arrange
            await _testsFixture.AuthenticateAsync();
            var createResponse = await _testsFixture._client.PostAsJsonAsync(_testsFixture._accountsApi,
                               new CreateAccountRequest { Description = description, InitialBalance = balance, Type = type });
            createResponse.EnsureSuccessStatusCode();

            var createResult = await _testsFixture.DeserializeObjectReponseAsync<SuccessModel<Guid>>(createResponse);
            AccountResponse account = null;

            // Act
            var response = await _testsFixture._client.GetAsync($"{_testsFixture._accountsApi}/{createResult.Data}");
            account = await _testsFixture.DeserializeObjectReponseAsync<AccountResponse>(response);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(createResult.Data, account.Id);
            Assert.Equal(description, account.Description);
            Assert.Equal(balance, account.Balance);
            Assert.Equal(type, account.Type);
            Assert.Equal(Status.Active, account.Status);
        }

        [Fact(DisplayName = "Obter conta bancária com Id inválido deve retornar not found.")]
        public async Task InvalidId_GetAccount_ReturnNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();

            // Act
            var response = await _testsFixture._client.GetAsync($"{_testsFixture._accountsApi}/{id}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact(DisplayName = "Obter todas as contas bancárias deve retornar uma lista com sucesso.")]
        public async Task GetAllAccounts_ReturnSuccess()
        {
            // Arrange
            await _testsFixture.AuthenticateAsync();
            await _testsFixture.CreateAccountAsync();
            List<AccountResponse> accounts = null;

            // Act
            var response = await _testsFixture._client.GetAsync(_testsFixture._accountsApi);
            accounts = await _testsFixture.DeserializeObjectReponseAsync<List<AccountResponse>>(response);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.NotEmpty(accounts);
        }
    }
}