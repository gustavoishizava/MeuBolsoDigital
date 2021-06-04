using System.Threading.Tasks;
using MBD.Identity.Domain.DTO;
using MBD.Identity.Domain.Entities;

namespace MBD.Identity.Domain.Interfaces.Services
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResponse> AuthenticateAsync(string email, string password);
    }
}