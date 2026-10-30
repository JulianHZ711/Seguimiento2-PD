using Access.Application.Utilities.Mediator;
using Access.Application.Utilities.Results;
using Access.Application.Utilities.Pagination;
using Access.Domain.Entities.AccessLogs.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Access.Application.UseCases.AccessLogs.Queries.GetAccessHistory
{
    public sealed class GetAccessHistoryQuery : IRequest<Result<PaginationResponse<AccessLogListItemDTO>>>
    {
        public PaginationRequest Pagination { get; set; } = PaginationRequest.Standart();

        public Guid? EmployeeId { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public Guid? AccessPointId { get; set; }
        public AccessResult? Result { get; set; }
    }
}
