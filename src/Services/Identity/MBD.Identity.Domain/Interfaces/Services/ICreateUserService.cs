using System.Threading.Tasks;
using MBD.Identity.Domain.ValueObjects;

namespace MBD.Identity.Domain.Interfaces.Services
{
    public interface ICreateUserService
    {
        Task<CreateUserResult> CreateAsync(string name, string email, string password);
    }
}