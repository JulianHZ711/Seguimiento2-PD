using Access.Application.Contracts.Persistence;
using Access.Application.Contracts.Repositories;
using Access.Application.Exceptions;
using Access.Application.UseCases.Common;
using Access.Application.Utilities.Mediator;
using Access.Application.Utilities.Results;
using Access.Domain.Entities.AccessPoints;

namespace Access.Application.UseCases.AccessPoints.Commands.DisableAccessPoint
{
    public sealed class DisableAccessPointUseCase :
        BaseHandler,
        IRequestHandler<DisableAccessPointCommand, Result<Guid>>
    {
        private readonly IAccessPointsRepository _accessPointsRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DisableAccessPointUseCase(IAccessPointsRepository accessPointsRepository, IUnitOfWork unitOfWork)
        {
            _accessPointsRepository = accessPointsRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(DisableAccessPointCommand command)
        {
            return await ExecuteAsync(async () =>
            {
                AccessPoint? accessPoint = await _accessPointsRepository.GetByIdAsync(command.Id);

                if (accessPoint is null)
                {
                    throw new NotFoundException($"No existe un punto de acceso con el ID {command.Id}.");
                }

                accessPoint.Disable();

                await _accessPointsRepository.UpdateAsync(accessPoint);
                await _unitOfWork.CommitAsync();

                return accessPoint.Id;
            });
        }
    }
}
