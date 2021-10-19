using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MBD.Transactions.Application.Queries.Categories.Queries;
using MBD.Transactions.Application.Response;
using MBD.Transactions.Domain.Interfaces.Repositories;
using MediatR;

namespace MBD.Transactions.Application.Queries.Categories.Handlers
{
    public class GetAllCategoriesByTypeQueryHandler : IRequestHandler<GetAllCategoriesByTypeQuery, IEnumerable<CategoryWithSubCategoriesResponse>>
    {
        private readonly ICategoryRepository _repository;
        private readonly IMapper _mapper;

        public GetAllCategoriesByTypeQueryHandler(ICategoryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryWithSubCategoriesResponse>> Handle(GetAllCategoriesByTypeQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<List<CategoryWithSubCategoriesResponse>>(await _repository.GetByTypeAsync(request.Type));
        }
    }
}