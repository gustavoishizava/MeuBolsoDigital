using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using MBD.Identity.Domain.DTO;
using MBD.Identity.Domain.Entities;
using MBD.Identity.Domain.Interfaces.Repositories;
using MBD.Identity.Domain.Interfaces.Services;

namespace MBD.Identity.Domain.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        private readonly IHashService _hashService;
        private readonly JwtConfiguration _jwtConfiguration;

        public AuthenticationService(IUserRepository userRepository, IJwtService jwtService, IHashService hashService, JwtConfiguration jwtConfiguration)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _jwtService = jwtService ?? throw new ArgumentNullException(nameof(jwtService));
            _hashService = hashService ?? throw new ArgumentNullException(nameof(hashService));
            _jwtConfiguration = jwtConfiguration ?? throw new ArgumentNullException(nameof(jwtConfiguration));
        }

        public async Task<AuthenticationResponse> AuthenticateAsync(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return new AuthenticationResponse("Informe o e-email e/ou senha");

            var user = await _userRepository.GetByEmailAsync(email.ToLower());
            if (user == null)
                return new AuthenticationResponse("E-mail e/ou senha incorreto(s)");

            if (!_hashService.IsMatch(password, user.Password.PasswordHash))
                return new AuthenticationResponse("E-mail e/ou senha incorreto(s)");

            return GenerateJwt(user);
        }

        private AuthenticationResponse GenerateJwt(User user)
        {
            var refreshToken = user.CreateRefreshToken(_jwtConfiguration.RefreshExpiresInSeconds);
            var issuedAt = DateTime.Now;
            var expiresIn = issuedAt.AddSeconds(_jwtConfiguration.ExpiresInSeconds);
            var claims = GenerateClaims(user.Id, user.Email.NormalizedAddress, issuedAt);
            var token = _jwtService.Generate(_jwtConfiguration.Issuer, _jwtConfiguration.Audience, issuedAt, expiresIn, claims);

            return new AuthenticationResponse(token, refreshToken.Token.ToString(), _jwtConfiguration.RefreshExpiresInSeconds, issuedAt);
        }

        private IEnumerable<Claim> GenerateClaims(Guid userId, string email, DateTime issuedAt)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(issuedAt).ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(issuedAt).ToString())
            };
            return new List<Claim>();
        }

        private static long ToUnixEpochDate(DateTime date)
            => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
                .TotalSeconds);
    }
}