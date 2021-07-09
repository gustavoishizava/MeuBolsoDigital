using System.Threading.Tasks;
using MBD.Application.Core.Response;
using MBD.BankAccounts.Application.Request;

namespace MBD.BankAccounts.Application.Interfaces
{
    public interface IAccountAppService
    {
        Task<IResult> CreateAsync(CreateAccountRequest request);
    }
}