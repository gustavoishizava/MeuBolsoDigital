using AutoMapper;
using MBD.Transactions.Application.Response;
using MBD.Transactions.Domain.Entities;

namespace MBD.Transactions.Application.AutoMapper
{
    public class DomainToReponseProfile : Profile
    {
        public DomainToReponseProfile()
        {
            CreateMap<Category, CategoryResponse>(MemberList.Destination);
        }
    }
}