using Access.Application.Contracts.Repositories;
using Access.Application.Exceptions;
using Access.Application.UseCases.Common;
using Access.Application.Utilities.Mediator;
using Access.Application.Utilities.Results;
using Access.Domain.Entities.AccessPoints;

namespace Access.Application.UseCases.AccessPoints.Queries.GetAccessPointById
{
    public sealed class GetAccessPointByIdUseCase :
        BaseHandler,
        IRequestHandler<GetAccessPointByIdQuery, Result<AccessPointDetailsDTO>>
    {
        private readonly IAccessPointsRepository _accessPointsRepository;

        public GetAccessPointByIdUseCase(IAccessPointsRepository accessPointsRepository)
        {
            _accessPointsRepository = accessPointsRepository;
        }

        public async Task<Result<AccessPointDetailsDTO>> Handle(GetAccessPointByIdQuery query)
        {
            return await ExecuteAsync(async () =>
            {
                AccessPoint? accessPoint = await _accessPointsRepository.GetByIdAsync(query.Id);

                if (accessPoint is null)
                {
                    throw new NotFoundException($"No existe un punto de acceso con el ID {query.Id}.");
                }

                return new AccessPointDetailsDTO
                {
                    Id = accessPoint.Id,
                    Name = accessPoint.Name,
                    Type = accessPoint.Type.ToString(),
                    IsEnabled = accessPoint.IsEnabled,
                    CreatedAt = accessPoint.CreatedAt,
                };
            });
        }
    }
}
