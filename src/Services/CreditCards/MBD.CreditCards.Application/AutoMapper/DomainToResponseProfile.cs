using AutoMapper;
using MBD.CreditCards.Application.Responses;
using MBD.CreditCards.Domain.Entities;

namespace MBD.CreditCards.Application.AutoMapper
{
    public class DomainToResponseProfile : Profile
    {
        public DomainToResponseProfile()
        {
            CreateMap<CreditCard, CreditCardResponse>(MemberList.Destination);
        }
    }
}