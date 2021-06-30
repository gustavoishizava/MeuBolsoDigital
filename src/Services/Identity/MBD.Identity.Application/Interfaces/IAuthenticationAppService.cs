using System.Threading.Tasks;
using MBD.Application.Core.Response;
using MBD.Identity.Application.Requests;
using MBD.Identity.Application.Responses;

namespace MBD.Identity.Application.Interfaces
{
    public interface IAuthenticationAppService
    {
        Task<IResult<AccessTokenResponse>> AuthenticateAsync(AuthenticateRequest request);
        Task<IResult<AccessTokenResponse>> RefreshTokenAsync(RefreshTokenRequest request);
    }
}