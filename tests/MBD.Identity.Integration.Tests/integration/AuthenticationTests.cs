using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using MBD.Identity.API;
using MBD.Identity.Application.Requests;
using MBD.Identity.Application.Responses;
using MBD.Identity.Integration.Tests.integration.Settings;
using Xunit;

namespace MBD.Identity.Integration.Tests.integration
{
    [Collection(nameof(IntegrationTestsFixtureCollection))]
    public class AuthenticationTests
    {
        private readonly IntegrationTestsFixture<StartupTests> _testsFixture;

        public AuthenticationTests(IntegrationTestsFixture<StartupTests> testsFixture)
        {
            _testsFixture = testsFixture;
        }

        [Fact(DisplayName = "Realizar a autenticação com credenciais válidas deve retornar sucesso.")]
        public async Task ValidCredential_Authenticate_ReturnSuccess()
        {
            // Arrange
            var loginRequest = new AuthenticateRequest
            {
                Email = _testsFixture.emailDefault,
                Password = _testsFixture.passwordDefault
            };

            AccessTokenResponse tokenResponse = null;

            // Act
            var response = await _testsFixture._client.PostAsJsonAsync("api/authentication/auth", loginRequest);
            tokenResponse = await _testsFixture.DeserializeObjectReponseAsync<AccessTokenResponse>(response);

            // Assert     
            response.EnsureSuccessStatusCode();
            Assert.NotNull(tokenResponse);
            Assert.NotEmpty(tokenResponse.AccessToken);
            Assert.NotEmpty(tokenResponse.RefreshToken);
            Assert.True(tokenResponse.CreatedAt < DateTime.Now);
            Assert.True(tokenResponse.ExpiresIn > 0);
        }

        [Fact(DisplayName = "Realizar a autenticação com credenciais inválidas deve retornar erro.")]
        public async Task InvalidCredential_Authenticate_ReturnError()
        {
            // Arrange
            var loginRequest = new AuthenticateRequest
            {
                Email = "email@inexistente.com",
                Password = "12345678"
            };

            // Act
            var response = await _testsFixture._client.PostAsJsonAsync("api/authentication/auth", loginRequest);

            // Assert     
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact(DisplayName = "Realizar refresh com um refresh token válido deve retornar sucesso.")]
        public async Task ValidRefreshToken_Refresh_ReturnSuccess()
        {
            // Arrange
            var loginRequest = new AuthenticateRequest
            {
                Email = _testsFixture.emailDefault,
                Password = _testsFixture.passwordDefault
            };

            AccessTokenResponse tokenResponse = null;
            AccessTokenResponse refreshtokenResponse = null;

            var authResponse = await _testsFixture._client.PostAsJsonAsync("api/authentication/auth", loginRequest);
            tokenResponse = await _testsFixture.DeserializeObjectReponseAsync<AccessTokenResponse>(authResponse);

            var refreshTokenRequest = new RefreshTokenRequest
            {
                RefreshToken = Guid.Parse(tokenResponse.RefreshToken)
            };

            // Act
            var refreshResponse = await _testsFixture._client.PostAsJsonAsync("api/authentication/refresh", refreshTokenRequest);
            refreshtokenResponse = await _testsFixture.DeserializeObjectReponseAsync<AccessTokenResponse>(refreshResponse);

            // Assert     
            refreshResponse.EnsureSuccessStatusCode();
            Assert.NotNull(refreshtokenResponse);
            Assert.NotEmpty(refreshtokenResponse.AccessToken);
            Assert.NotEmpty(refreshtokenResponse.RefreshToken);
            Assert.True(refreshtokenResponse.CreatedAt < DateTime.Now);
            Assert.True(refreshtokenResponse.ExpiresIn > 0);
        }

        [Fact(DisplayName = "Realizar refresh com um refresh token inválido deve retornar erro.")]
        public async Task InvalidRefreshToken_Refresh_ReturnError()
        {
            // Arrange            
            var refreshTokenRequest = new RefreshTokenRequest
            {
                RefreshToken = Guid.NewGuid()
            };

            // Act
            var refreshResponse = await _testsFixture._client.PostAsJsonAsync("api/authentication/refresh", refreshTokenRequest);

            // Assert     
            Assert.Equal(HttpStatusCode.BadRequest, refreshResponse.StatusCode);
        }
    }
}