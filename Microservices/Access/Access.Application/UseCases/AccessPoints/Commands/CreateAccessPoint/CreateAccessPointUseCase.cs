using Access.Application.Contracts.Persistence;
using Access.Application.Contracts.Repositories;
using Access.Application.Exceptions;
using Access.Application.UseCases.Common;
using Access.Application.Utilities.Mediator;
using Access.Application.Utilities.Results;
using Access.Domain.Entities.AccessPoints;
using Access.Domain.Exceptions;

namespace Access.Application.UseCases.AccessPoints.Commands.CreateAccessPoint
{
    public sealed class CreateAccessPointUseCase :
        BaseHandler,
        IRequestHandler<CreateAccessPointCommand, Result<Guid>>
    {
        private readonly IAccessPointsRepository _accessPointsRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateAccessPointUseCase(IAccessPointsRepository accessPointsRepository, IUnitOfWork unitOfWork)
        {
            _accessPointsRepository = accessPointsRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(CreateAccessPointCommand command)
        {
            return await ExecuteAsync(async () =>
            {
                if (await _accessPointsRepository.ExistsByNameAsync(command.Name))
                {
                    throw new BussinesRuleException($"Ya existe un punto de acceso llamado {command.Name.Trim()}.");
                }

                AccessPoint accessPoint = new(command.Name, command.Type, command.IsEnabled);

                await _accessPointsRepository.CreateAsync(accessPoint);
                await _unitOfWork.CommitAsync();

                return accessPoint.Id;
            });
        }
    }
}
