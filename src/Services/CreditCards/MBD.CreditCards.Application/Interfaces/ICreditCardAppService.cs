using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using MBD.Application.Core.Response;
using MBD.CreditCards.Application.Requests;
using MBD.CreditCards.Application.Responses;

namespace MBD.CreditCards.Application.Interfaces
{
    public interface ICreditCardAppService
    {
        Task<IResult<CreditCardResponse>> CreateAsync(CreateCreditCardRequest request);
        Task<IResult> UpdateAsync(UpdateCreditCardRequest request);
        Task<IResult<CreditCardResponse>> GetByIdAsync(Guid id);
        Task<IEnumerable<CreditCardResponse>> GetAllAsync();
        Task<IResult> RemoveAsync(Guid id);
    }
}