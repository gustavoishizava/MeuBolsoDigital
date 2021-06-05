using System.Threading.Tasks;
using MBD.Identity.Domain.DTO;

namespace MBD.Identity.Domain.Interfaces.Services
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResponse> AuthenticateAsync(string email, string password);
    }
}