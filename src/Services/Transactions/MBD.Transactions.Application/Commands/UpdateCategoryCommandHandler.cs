using System.Threading;
using System.Threading.Tasks;
using MBD.Application.Core.Response;
using MBD.Core.Data;
using MBD.Core.Enumerations;
using MBD.Transactions.Domain.Interfaces.Repositories;
using MediatR;

namespace MBD.Transactions.Application.Commands
{
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, IResult>
    {
        private readonly ICategoryRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCategoryCommandHandler(ICategoryRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IResult> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var validation = request.Validate();
            if (!validation.IsValid)
                return Result.Fail(validation.ToString());

            var category = await _repository.GetByIdAsync(request.Id);
            if (category == null)
                return Result.Fail("Categoria inválida.");

            category.SetName(request.Name);
            if (request.Status == Status.Active)
                category.Activate();
            else
                category.Deactivate();

            _repository.Update(category);
            await _unitOfWork.SaveChangesAsync();

            return Result.Success();
        }
    }
}