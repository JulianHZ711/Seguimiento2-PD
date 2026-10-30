using Access.Application.Contracts.Repositories;
using Access.Application.UseCases.Common;
using Access.Application.Utilities.Mediator;
using Access.Application.Utilities.Pagination;
using Access.Application.Utilities.Results;
using Access.Domain.Entities.AccessLogs;

namespace Access.Application.UseCases.AccessLogs.Queries.GetAccessHistory
{
    public sealed class GetAccessHistoryUseCase :
        BaseHandler,
        IRequestHandler<GetAccessHistoryQuery, Result<PaginationResponse<AccessLogListItemDTO>>>
    {
        private readonly IAccessLogsRepository _accessLogsRepository;

        public GetAccessHistoryUseCase(IAccessLogsRepository accessLogsRepository)
        {
            _accessLogsRepository = accessLogsRepository;
        }

        public async Task<Result<PaginationResponse<AccessLogListItemDTO>>> Handle(GetAccessHistoryQuery query)
        {
            return await ExecuteAsync(async () =>
            {
                PaginationRequest pagination = query.Pagination;

                PaginationResponse<AccessLog> response = await _accessLogsRepository.GetPagedListAsync(pagination,
                                                                                                      query.EmployeeId,
                                                                                                      query.From,
                                                                                                      ToEndOfDayIfDateOnly(query.To),
                                                                                                      query.AccessPointId,
                                                                                                      query.Result);

                List<AccessLogListItemDTO> items = response.Items.Select(l => l.ToListItemDTO())
                                                                 .ToList();

                return PaginationResponse<AccessLogListItemDTO>.Create(items, response.TotalCount, pagination);
            });
        }

        // Si la fecha final llega sin hora (ej: 2026-10-29), se incluye todo ese día.
        private static DateTime? ToEndOfDayIfDateOnly(DateTime? to)
        {
            if (to is null || to.Value.TimeOfDay != TimeSpan.Zero)
            {
                return to;
            }

            return to.Value.Date.AddDays(1).AddTicks(-1);
        }
    }
}
