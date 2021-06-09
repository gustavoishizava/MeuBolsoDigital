using System;
using System.Threading.Tasks;
using MBD.Identity.Domain.ValueObjects;

namespace MBD.Identity.Domain.Interfaces.Services
{
    public interface IAuthenticationService
    {
        Task<AccessTokenResponse> AuthenticateAsync(string email, string password);
        Task<AccessTokenResponse> AuthenticateByRefreshTokenAsync(Guid refreshToken);
    }
}