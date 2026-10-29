using Access.Application.Contracts.Repositories;
using Access.Application.UseCases.Common;
using Access.Application.Utilities.Mediator;
using Access.Application.Utilities.Results;
using Access.Domain.Entities.Areas;

namespace Access.Application.UseCases.Areas.Queries.GetAreasList
{
    public sealed class GetAreasListUseCase :
        BaseHandler,
        IRequestHandler<GetAreasListQuery, Result<List<AreaDTO>>>
    {
        private readonly IAreasRepository _areasRepository;

        public GetAreasListUseCase(IAreasRepository areasRepository)
        {
            _areasRepository = areasRepository;
        }

        public async Task<Result<List<AreaDTO>>> Handle(GetAreasListQuery query)
        {
            return await ExecuteAsync(async () =>
            {
                IEnumerable<Area> areas = await _areasRepository.GetListAsync();

                return areas.OrderBy(a => a.Name)
                            .Select(a => new AreaDTO
                            {
                                Id = a.Id,
                                Name = a.Name,
                                Description = a.Description,
                            })
                            .ToList();
            });
        }
    }
}
