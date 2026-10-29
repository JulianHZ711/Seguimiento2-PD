using Access.Application.Contracts.Repositories;
using Access.Application.UseCases.Common;
using Access.Application.Utilities.Mediator;
using Access.Application.Utilities.Pagination;
using Access.Application.Utilities.Results;
using Access.Domain.Entities.AccessPoints;

namespace Access.Application.UseCases.AccessPoints.Queries.GetAccessPointsList
{
    public sealed class GetAccessPointsListUseCase :
        BaseHandler,
        IRequestHandler<GetAccessPointsListQuery, Result<PaginationResponse<AccessPointListItemDTO>>>
    {
        private readonly IAccessPointsRepository _accessPointsRepository;

        public GetAccessPointsListUseCase(IAccessPointsRepository accessPointsRepository)
        {
            _accessPointsRepository = accessPointsRepository;
        }

        public async Task<Result<PaginationResponse<AccessPointListItemDTO>>> Handle(GetAccessPointsListQuery query)
        {
            return await ExecuteAsync(async () =>
            {
                PaginationRequest pagination = query.Pagination;

                PaginationResponse<AccessPoint> response = await _accessPointsRepository.GetPagedListAsync(pagination,
                                                                                                          query.IsEnabled,
                                                                                                          query.Type);

                List<AccessPointListItemDTO> items = response.Items.Select(p => new AccessPointListItemDTO
                                                                   {
                                                                       Id = p.Id,
                                                                       Name = p.Name,
                                                                       Type = p.Type.ToString(),
                                                                       IsEnabled = p.IsEnabled,
                                                                   })
                                                                   .ToList();

                return PaginationResponse<AccessPointListItemDTO>.Create(items, response.TotalCount, pagination);
            });
        }
    }
}
