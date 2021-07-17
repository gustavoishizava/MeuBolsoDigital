using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MBD.Application.Core.Response;
using MBD.BankAccounts.Application.Request;
using MBD.BankAccounts.Application.Response;

namespace MBD.BankAccounts.Application.Interfaces
{
    public interface IAccountAppService
    {
        Task<IResult<AccountResponse>> CreateAsync(CreateAccountRequest request);
        Task<IResult> UpdateAsync(UpdateAccountRequest request);
        Task<IResult<AccountResponse>> GetByIdAsync(Guid id);
        Task<IEnumerable<AccountResponse>> GetAllAsync();
        Task<IResult> RemoveAsync(Guid id);
    }
}