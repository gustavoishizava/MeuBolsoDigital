using System.Threading.Tasks;
using MBD.Application.Core.Response;
using MBD.Identity.Application.Interfaces;
using MBD.Identity.Application.Requests;
using MBD.Identity.Domain.Interfaces.Services;

namespace MBD.Identity.Application.Services
{
    public class UserAppService : IUserAppService
    {
        private readonly ICreateUserService _createUserService;

        public UserAppService(ICreateUserService createUserService)
        {
            _createUserService = createUserService;
        }

        public async Task<IResult> CreateAsync(CreateUserRequest request)
        {
            var requestValidation = request.Validate();
            if(!requestValidation.IsValid)
                return Result.Fail(requestValidation.ToString());

            var response = await _createUserService.CreateAsync(request.Name, request.Email, request.Password);
            if(!response.IsSuccessful)
                return Result.Fail(response.Message);

            return Result.Success(response.Message);
        }
    }
}