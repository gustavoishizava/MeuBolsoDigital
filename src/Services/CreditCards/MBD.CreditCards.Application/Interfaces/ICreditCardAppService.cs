using System;
using System.Threading.Tasks;
using MBD.Application.Core.Response;
using MBD.CreditCards.Application.Requests;

namespace MBD.CreditCards.Application.Interfaces
{
    public interface ICreditCardAppService
    {
        Task<IResult<Guid>> CreateAsync(CreateCreditCardRequest request);
    }
}