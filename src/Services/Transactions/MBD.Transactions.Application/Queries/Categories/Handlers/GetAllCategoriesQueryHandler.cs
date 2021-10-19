using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MBD.Transactions.Application.Queries.Categories.Queries;
using MBD.Transactions.Application.Response;
using MBD.Transactions.Domain.Enumerations;
using MBD.Transactions.Domain.Interfaces.Repositories;
using MediatR;

namespace MBD.Transactions.Application.Queries.Categories.Handlers
{
    public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, CategoryByTypeResponse>
    {
        private readonly ICategoryRepository _repository;
        private readonly IMapper _mapper;

        public GetAllCategoriesQueryHandler(ICategoryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<CategoryByTypeResponse> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            var allCategories = _mapper.Map<List<CategoryWithSubCategoriesResponse>>(await _repository.GetAllAsync());
            var incomeCategories = allCategories.Where(x => x.Type == TransactionType.Income).ToList();
            var expenseCategories = allCategories.Where(x => x.Type == TransactionType.Expense).ToList();

            return new CategoryByTypeResponse(incomeCategories, expenseCategories);
        }
    }
}