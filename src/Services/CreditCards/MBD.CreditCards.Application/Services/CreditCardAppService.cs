using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MBD.Application.Core.Response;
using MBD.Core.Enumerations;
using MBD.Core.Identity;
using MBD.CreditCards.Application.Interfaces;
using MBD.CreditCards.Application.Requests;
using MBD.CreditCards.Application.Responses;
using MBD.CreditCards.Domain.Entities;
using MBD.CreditCards.Domain.Interfaces.Repositories;
using MBD.CreditCards.Domain.Interfaces.Services;

namespace MBD.CreditCards.Application.Services
{
    public class CreditCardAppService : ICreditCardAppService
    {
        private readonly IAspNetUser _aspNetUser;
        private readonly IMapper _mapper;
        private readonly ICreditCardRepository _repository;
        private readonly IBankAccountService _bankAccountService;

        public CreditCardAppService(IAspNetUser aspNetUser, IMapper mapper, ICreditCardRepository repository, IBankAccountService bankAccountService)
        {
            _aspNetUser = aspNetUser;
            _mapper = mapper;
            _repository = repository;
            _bankAccountService = bankAccountService;
        }

        public async Task<IResult<CreditCardResponse>> CreateAsync(CreateCreditCardRequest request)
        {
            var validation = request.Validate();
            if (!validation.IsValid)
                return Result<CreditCardResponse>.Fail(validation.ToString());

            var bankAccount = await _bankAccountService.GetByIdAsync(request.BankAccountId);
            if (bankAccount == null)
                return Result<CreditCardResponse>.Fail("Conta bancária inválida.");

            var creditCard = new CreditCard(_aspNetUser.UserId,
                                            request.BankAccountId,
                                            request.Name,
                                            request.ClosingDay,
                                            request.DayOfPayment,
                                            request.Limit,
                                            request.Brand);

            _repository.Add(creditCard);
            await _repository.SaveChangesAsync();

            return Result<CreditCardResponse>.Success(_mapper.Map<CreditCardResponse>(creditCard));
        }

        public async Task<IEnumerable<CreditCardResponse>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<CreditCardResponse>>(await _repository.GetAllAsync());
        }

        public async Task<IResult<CreditCardResponse>> GetByIdAsync(Guid id)
        {
            var creditCard = await _repository.GetByIdAsync(id);
            if (creditCard == null)
                return Result<CreditCardResponse>.Fail();

            return Result<CreditCardResponse>.Success(_mapper.Map<CreditCardResponse>(creditCard));
        }

        public async Task<IResult> RemoveAsync(Guid id)
        {
            var creditCard = await _repository.GetByIdAsync(id);
            if (creditCard == null)
                return Result.Fail("Cartão de crédito inválido.");

            _repository.Remove(creditCard);
            await _repository.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<IResult> UpdateAsync(UpdateCreditCardRequest request)
        {
            var validation = request.Validate();
            if (!validation.IsValid)
                return Result<CreditCardResponse>.Fail(validation.ToString());

            var creditCard = await _repository.GetByIdAsync(request.Id);
            if (creditCard == null)
                return Result.Fail("Cartão de crédito inválido.");

            var bankAccount = await _bankAccountService.GetByIdAsync(request.BankAccountId);
            if (bankAccount == null)
                return Result<CreditCardResponse>.Fail("Conta bancária inválida.");

            creditCard.SetName(request.Name);
            creditCard.SetBankAccountId(request.BankAccountId);
            creditCard.SetBrand(request.Brand);
            creditCard.SetClosingDay(request.ClosingDay);
            creditCard.SetDayOfPayment(request.DayOfPayment);
            creditCard.SetLimit(request.Limit);
            if (request.Status == Status.Active)
                creditCard.Activate();
            else
                creditCard.Deactivate();

            await _repository.SaveChangesAsync();

            return Result.Success();
        }
    }
}