using System;
using System.Collections.Generic;
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

        public AuthenticationService(IUserRepository userRepository, IJwtService jwtService, IHashService hashService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _hashService = hashService;
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
            var refreshToken = user.CreateRefreshToken(3600);
            var dateNow = DateTime.Now;
            var token = _jwtService.Generate("", "", dateNow, dateNow.AddHours(1), GenerateClaims());

            return new AuthenticationResponse(token, refreshToken.Token.ToString(), 3600, dateNow);
        }

        private IEnumerable<Claim> GenerateClaims()
        {
            return new List<Claim>();
        }
    }
}