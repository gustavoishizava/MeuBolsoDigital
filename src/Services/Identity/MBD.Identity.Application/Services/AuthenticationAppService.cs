using System.Threading.Tasks;
using MBD.Application.Core.Response;
using MBD.Identity.Application.Interfaces;
using MBD.Identity.Application.Requests;
using MBD.Identity.Application.Responses;
using MBD.Identity.Domain.Interfaces.Services;

namespace MBD.Identity.Application.Services
{
    public class AuthenticationAppService : IAuthenticationAppService
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationAppService(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public async Task<IResult<AccessTokenResponse>> AuthenticateAsync(AuthenticateRequest request)
        {
            var requestValidation = request.Validate();
            if (!requestValidation.IsValid)
                return Result<AccessTokenResponse>.Fail(requestValidation.ToString());

            return ValidateAndReturnAccessTokenResponse(await _authenticationService.AuthenticateAsync(request.Email, request.Password));
        }

        public async Task<IResult<AccessTokenResponse>> RefreshTokenAsync(RefreshTokenRequest request)
        {
            var requestValidation = request.Validate();
            if (!requestValidation.IsValid)
                return Result<AccessTokenResponse>.Success(requestValidation.ToString());

            return ValidateAndReturnAccessTokenResponse(await _authenticationService.AuthenticateByRefreshTokenAsync(request.RefreshToken));
        }

        private static IResult<AccessTokenResponse> ValidateAndReturnAccessTokenResponse(Domain.ValueObjects.AccessTokenResponse response)
        {
            if (response.HasErrors)
                return Result<AccessTokenResponse>.Fail(response.Error);

            return Result<AccessTokenResponse>.Success(new AccessTokenResponse
            {
                AccessToken = response.AccessToken,
                RefreshToken = response.RefreshToken,
                CreatedAt = response.CreatedAt,
                ExpiresIn = response.ExpiresIn
            });
        }
    }
}