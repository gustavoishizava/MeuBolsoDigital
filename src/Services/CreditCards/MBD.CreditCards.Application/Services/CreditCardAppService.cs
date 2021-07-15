using System;
using System.Threading.Tasks;
using AutoMapper;
using MBD.Application.Core.Response;
using MBD.Core.Identity;
using MBD.CreditCards.Application.Interfaces;
using MBD.CreditCards.Application.Requests;
using MBD.CreditCards.Domain.Entities;
using MBD.CreditCards.Domain.Interfaces.Repositories;

namespace MBD.CreditCards.Application.Services
{
    public class CreditCardAppService : ICreditCardAppService
    {
        private readonly IAspNetUser _aspNetUser;
        private readonly IMapper _mapper;
        private readonly ICreditCardRepository _repository;

        public CreditCardAppService(IAspNetUser aspNetUser, IMapper mapper, ICreditCardRepository repository)
        {
            _aspNetUser = aspNetUser;
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<IResult<Guid>> CreateAsync(CreateCreditCardRequest request)
        {
            var validation = request.Validate();
            if (!validation.IsValid)
                return Result<Guid>.Fail(validation.ToString());

            var creditCard = new CreditCard(_aspNetUser.UserId,
                                            request.BankAccountId,
                                            request.Name,
                                            request.ClosingDay,
                                            request.DayOfPayment,
                                            request.Limit,
                                            request.Brand);

            _repository.Add(creditCard);
            await _repository.SaveChangesAsync();

            return Result<Guid>.Success(creditCard.Id, "Cartão de crédito criado com sucesso.");
        }
    }
}