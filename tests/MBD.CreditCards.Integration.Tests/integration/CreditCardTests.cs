using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using MBD.Core.Enumerations;
using MBD.CreditCards.API;
using MBD.CreditCards.API.Models;
using MBD.CreditCards.Application.Requests;
using MBD.CreditCards.Application.Responses;
using MBD.CreditCards.Domain.Enumerations;
using MBD.CreditCards.Integration.Tests.integration.Settings;
using Xunit;

namespace MBD.CreditCards.Integration.Tests.integration
{
    [Collection(nameof(IntegrationTestsFixtureCollection))]
    public class CreditCardTests
    {
        private readonly IntegrationTestsFixture<StartupTests> _testsFixture;

        public CreditCardTests(IntegrationTestsFixture<StartupTests> testsFixture)
        {
            _testsFixture = testsFixture;
        }

        [Fact(DisplayName = "Criar novo cartão de crédito com conta bancária válida deve retornar sucesso.")]
        public async Task ValidCreditCardAndBankAccount_CreateNew_ReturnSuccess()
        {
            // Arrange
            await _testsFixture.AuthenticateAsync();

            var random = new Random();
            var bankAccountId = await _testsFixture.GetOrCreateBankAccountAsync();
            var name = "Nubank";
            var closingDay = random.Next(1, 32);
            var dayOfPayment = random.Next(1, 32);
            var limit = random.Next(1, 10000);
            var brand = Brand.VISA;

            var request = new CreateCreditCardRequest
            {
                BankAccountId = bankAccountId,
                Brand = brand,
                Name = name,
                ClosingDay = closingDay,
                DayOfPayment = dayOfPayment,
                Limit = limit
            };

            CreditCardResponse result = null;

            // Act
            var response = await _testsFixture._client.PostAsJsonAsync("/api/credit-cards", request);
            result = await _testsFixture.DeserializeObjectReponseAsync<CreditCardResponse>(response);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.NotNull(result);
            Assert.Equal(bankAccountId, result.BankAccountId);
            Assert.Equal(name, result.Name);
            Assert.Equal(brand, result.Brand);
            Assert.Equal(closingDay, result.ClosingDay);
            Assert.Equal(dayOfPayment, result.DayOfPayment);
            Assert.Equal(limit, result.Limit);
        }

        [Fact(DisplayName = "Criar novo cartão de crédito com conta bancária inválida deve retornar erro.")]
        public async Task InvalidBankAccountId_NewCreditCard_ReturnError()
        {
            // Arrange
            await _testsFixture.AuthenticateAsync();

            var request = new CreateCreditCardRequest
            {
                BankAccountId = Guid.NewGuid(),
                Brand = Brand.MASTERCARD,
                ClosingDay = 1,
                DayOfPayment = 5,
                Limit = 100,
                Name = "Invalid Bank Account"
            };

            ErrorModel result = null;

            // Act
            var response = await _testsFixture._client.PostAsJsonAsync("/api/credit-cards", request);
            result = await _testsFixture.DeserializeObjectReponseAsync<ErrorModel>(response);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotNull(result);
            Assert.Single(result.Errors);
            Assert.Equal("Conta bancária inválida.", result.Errors.First());
        }

        [Fact(DisplayName = "Criar cartão de crédito com dados inválidos deve retornar error.")]
        public async Task InvalidData_NewCreditCard_ReturnErrors()
        {
            // Arrange
            await _testsFixture.AuthenticateAsync();

            var request = new CreateCreditCardRequest
            {
                BankAccountId = Guid.NewGuid(),
                Brand = Brand.VISA,
                ClosingDay = 0,
                DayOfPayment = 32,
                Limit = -100,
                Name = ""
            };

            var numberOfErrors = 5;

            ErrorModel result = null;

            // Act
            var response = await _testsFixture._client.PostAsJsonAsync("/api/credit-cards", request);
            result = await _testsFixture.DeserializeObjectReponseAsync<ErrorModel>(response);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotNull(result);
            Assert.Equal(numberOfErrors, result.Errors.Count());
        }

        [Theory(DisplayName = "Atualizar cartão de crédito com informações válidas deve retornar sucesso.")]
        [InlineData("Santander", 5, 10, 5000, Status.Active, Brand.AMERICANEXPRESS)]
        [InlineData("Itaú", 25, 1, 2000, Status.Inactive, Brand.ELO)]
        public async Task ValidCreditCard_Update_ReturnSucess(string name, int closingDay, int dayOfPayment, decimal limit, Status status, Brand brand)
        {
            // Arrange
            await _testsFixture.AuthenticateAsync();
            var creditCard = await _testsFixture.CreateCreditCardAsync();
            var bankAccountId = await _testsFixture.GetOrCreateBankAccountAsync();

            var request = new UpdateCreditCardRequest
            {
                Id = creditCard.Id,
                BankAccountId = bankAccountId,
                Brand = brand,
                Name = name,
                ClosingDay = closingDay,
                DayOfPayment = dayOfPayment,
                Limit = limit,
                Status = status
            };

            CreditCardResponse result = null;

            // Act
            var response = await _testsFixture._client.PutAsJsonAsync("/api/credit-cards", request);
            result = await _testsFixture.GetCreditCardByIdAsync(creditCard.Id);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.NotNull(result);
            Assert.Equal(bankAccountId, result.BankAccountId);
            Assert.Equal(name, result.Name);
            Assert.Equal(closingDay, result.ClosingDay);
            Assert.Equal(dayOfPayment, result.DayOfPayment);
            Assert.Equal(limit, result.Limit);
            Assert.Equal(status, result.Status);
            Assert.Equal(brand, result.Brand);
        }

        [Fact(DisplayName = "Atualizar cartão de crédito com conta bancária inválida deve retornar erro.")]
        public async Task InvalidBankAccountId_Update_ReturnError()
        {
            // Arrange
            await _testsFixture.AuthenticateAsync();
            var creditCard = await _testsFixture.CreateCreditCardAsync();

            var request = new UpdateCreditCardRequest
            {
                Id = creditCard.Id,
                BankAccountId = Guid.NewGuid(),
                Brand = creditCard.Brand,
                Name = creditCard.Name,
                ClosingDay = creditCard.ClosingDay,
                DayOfPayment = creditCard.DayOfPayment,
                Limit = creditCard.Limit,
                Status = creditCard.Status
            };

            ErrorModel result = null;

            // Act
            var response = await _testsFixture._client.PutAsJsonAsync("/api/credit-cards", request);
            result = await _testsFixture.DeserializeObjectReponseAsync<ErrorModel>(response);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotNull(result);
            Assert.Single(result.Errors);
            Assert.Equal("Conta bancária inválida.", result.Errors.First());
        }

        [Fact(DisplayName = "Atualizar cartão de crédito válido com dados inválidos deve retornar erros.")]
        public async Task InvalidData_Update_ReturnErrors()
        {
            // Arrange
            await _testsFixture.AuthenticateAsync();
            var creditCard = await _testsFixture.CreateCreditCardAsync();

            var request = new UpdateCreditCardRequest
            {
                Id = creditCard.Id,
                BankAccountId = creditCard.BankAccountId,
                Brand = Brand.VISA,
                Name = "",
                ClosingDay = 0,
                DayOfPayment = 35,
                Limit = -200,
                Status = Status.Active
            };

            var numberOfErrors = 5;

            ErrorModel result = null;

            // Act
            var response = await _testsFixture._client.PutAsJsonAsync("/api/credit-cards", request);
            result = await _testsFixture.DeserializeObjectReponseAsync<ErrorModel>(response);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotNull(result);
            Assert.Equal(numberOfErrors, result.Errors.Count());
        }

        [Fact(DisplayName = "Atualizar cartão de crédito inválido deve retornar erro.")]
        public async Task InvalidCreditCardId_Update_ReturnNotFound()
        {
            // Arrange
            await _testsFixture.AuthenticateAsync();

            var request = new UpdateCreditCardRequest
            {
                Id = Guid.NewGuid(),
                BankAccountId = Guid.NewGuid(),
                Brand = Brand.VISA,
                Name = "Test",
                ClosingDay = 1,
                DayOfPayment = 5,
                Limit = 200,
                Status = Status.Active
            };

            // Act
            var response = await _testsFixture._client.PutAsJsonAsync("/api/credit-cards", request);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact(DisplayName = "Deletar cartão de crédito existente deve retornar sucesso.")]
        public async Task ValidCreditCard_Delete_ReturnSuccess()
        {
            // Arrange
            await _testsFixture.AuthenticateAsync();
            var creditCard = await _testsFixture.CreateCreditCardAsync();

            // Act
            var response = await _testsFixture._client.DeleteAsync($"/api/credit-cards/{creditCard.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact(DisplayName = "Deletar cartão de crédito inexistente deve retornar erro.")]
        public async Task InvalidCreditCard_Delete_ReturnError()
        {
            // Arrange
            await _testsFixture.AuthenticateAsync();

            // Act
            var response = await _testsFixture._client.DeleteAsync($"/api/credit-cards/{Guid.NewGuid()}");

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact(DisplayName = "Obter cartão de crédito válido por Id deve retornar sucesso.")]
        public async Task ValidCreditCard_GetById_ReturnSuccess()
        {
            // Arrange
            await _testsFixture.AuthenticateAsync();
            var creditCard = await _testsFixture.CreateCreditCardAsync();
            CreditCardResponse result = null;

            // Act
            var response = await _testsFixture._client.GetAsync($"/api/credit-cards/{creditCard.Id}");
            result = await _testsFixture.DeserializeObjectReponseAsync<CreditCardResponse>(response);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(creditCard.Id, result.Id);
            Assert.Equal(creditCard.Name, result.Name);
            Assert.Equal(creditCard.BankAccountId, result.BankAccountId);
            Assert.Equal(creditCard.ClosingDay, result.ClosingDay);
            Assert.Equal(creditCard.DayOfPayment, result.DayOfPayment);
            Assert.Equal(creditCard.Limit, result.Limit);
            Assert.Equal(creditCard.Status, result.Status);
            Assert.Equal(creditCard.Brand, result.Brand);
        }

        [Fact(DisplayName = "Obter cartão de crédito inválido por Id deve retornar status NotFound.")]
        public async Task InvalidCreditCard_GetById_ReturnNotFound()
        {
            // Arrange
            await _testsFixture.AuthenticateAsync();

            // Act
            var response = await _testsFixture._client.GetAsync($"/api/credit-cards/{Guid.NewGuid()}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact(DisplayName = "Obter todos os cartões de crédito quando existente, deve retornar status OK com a lista dos cartões.")]
        public async Task GetAll_ReturnSuccess()
        {
            // Arrange
            await _testsFixture.AuthenticateAsync();
            await _testsFixture.CreateCreditCardAsync();

            List<CreditCardResponse> list = null;

            // Act
            var response = await _testsFixture._client.GetAsync("/api/credit-cards");
            list = await _testsFixture.DeserializeObjectReponseAsync<List<CreditCardResponse>>(response);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.NotNull(list);
            Assert.NotEmpty(list);
        }
    }
}