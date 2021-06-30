using System.Threading.Tasks;
using MBD.Application.Core.Response;
using MBD.Identity.Application.Requests;

namespace MBD.Identity.Application.Interfaces
{
    public interface IUserAppService
    {
        Task<IResult> CreateAsync(CreateUserRequest request);
    }
}