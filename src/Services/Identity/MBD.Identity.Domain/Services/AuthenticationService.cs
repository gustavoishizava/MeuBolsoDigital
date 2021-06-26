using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using MBD.Core.DomainObjects;
using MBD.Identity.Domain.Configuration;
using MBD.Identity.Domain.Entities;
using MBD.Identity.Domain.Interfaces.Repositories;
using MBD.Identity.Domain.Interfaces.Services;
using MBD.Identity.Domain.ValueObjects;
using Microsoft.Extensions.Options;
using static MBD.Identity.Domain.ValueObjects.AccessTokenResponse;

namespace MBD.Identity.Domain.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        private readonly IHashService _hashService;
        private readonly JwtConfiguration _jwtConfiguration;

        public AuthenticationService(IUserRepository userRepository, IJwtService jwtService, IHashService hashService, IOptions<JwtConfiguration> jwtConfiguration)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _jwtService = jwtService ?? throw new ArgumentNullException(nameof(jwtService));
            _hashService = hashService ?? throw new ArgumentNullException(nameof(hashService));
            _jwtConfiguration = jwtConfiguration?.Value ?? throw new ArgumentNullException(nameof(jwtConfiguration));
        }

        public async Task<AccessTokenResponse> AuthenticateAsync(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return AccessTokenResponseFactory.Fail("Informe o e-email e/ou senha.");

            var user = await _userRepository.GetByEmailAsync(email.ToLower());
            if (user == null)
                return AccessTokenResponseFactory.Fail("E-mail e/ou senha incorreto(s).");

            if (!_hashService.IsMatch(password, user.Password.PasswordHash))
                return AccessTokenResponseFactory.Fail("E-mail e/ou senha incorreto(s).");

            var authenticationResponse = GenerateJwt(user);
            await _userRepository.SaveChangesAsync();

            return authenticationResponse;
        }

        public async Task<AccessTokenResponse> AuthenticateByRefreshTokenAsync(Guid token)
        {
            if (Guid.Empty.Equals(token))
                return AccessTokenResponseFactory.Fail("Refresh token inválido.");

            var refreshToken = await _userRepository.GetRefreshTokenByToken(token);
            if (refreshToken == null)
                return AccessTokenResponseFactory.Fail("Refresh token inválido.");

            if (!refreshToken.IsValid)
                return AccessTokenResponseFactory.Fail("Este token não está mais válido.");

            var user = await _userRepository.GetByIdAsync(refreshToken.UserId);
            if (user == null)
                throw new DomainException("Token vinculado a usuário inexistente.");

            refreshToken.Revoke();
            var authenticationResponse = GenerateJwt(user);
            await _userRepository.SaveChangesAsync();

            return authenticationResponse;
        }

        private AccessTokenResponse GenerateJwt(User user)
        {
            var refreshToken = user.CreateRefreshToken(_jwtConfiguration.RefreshExpiresInSeconds);
            _userRepository.AddRefreshToken(refreshToken);

            var issuedAt = DateTime.Now;
            var expiresIn = issuedAt.AddSeconds(_jwtConfiguration.ExpiresInSeconds);
            var claims = GenerateClaims(user.Id, user.Email.NormalizedAddress, issuedAt);
            var token = _jwtService.Generate(_jwtConfiguration.Issuer, _jwtConfiguration.Audience, issuedAt, expiresIn, claims);

            return AccessTokenResponseFactory.Success(token, refreshToken.Token.ToString(), _jwtConfiguration.ExpiresInSeconds, issuedAt);
        }

        private static IEnumerable<Claim> GenerateClaims(Guid userId, string email, DateTime issuedAt)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(issuedAt).ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(issuedAt).ToString())
            };
            return claims;
        }

        private static long ToUnixEpochDate(DateTime date)
            => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
                .TotalSeconds);
    }
}