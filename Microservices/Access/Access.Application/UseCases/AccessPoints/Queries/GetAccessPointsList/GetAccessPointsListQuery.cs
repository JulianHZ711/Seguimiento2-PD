using Access.Application.Utilities.Mediator;
using Access.Application.Utilities.Results;
using Access.Application.Utilities.Pagination;
using System;
using System.Collections.Generic;
using System.Text;

namespace Access.Application.UseCases.AccessPoints.Queries.GetAccessPointsList
{
    public sealed class GetAccessPointsListQuery : IRequest<Result<PaginationResponse<AccessPointListItemDTO>>>
    {
        public PaginationRequest Pagination { get; set; } = PaginationRequest.Standart();

        public bool? IsEnabled { get; set; }
    }
}
