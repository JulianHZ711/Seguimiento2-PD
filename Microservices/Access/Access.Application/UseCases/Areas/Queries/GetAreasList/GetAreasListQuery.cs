using Access.Application.Utilities.Mediator;
using Access.Application.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace Access.Application.UseCases.Areas.Queries.GetAreasList
{
    public sealed class GetAreasListQuery : IRequest<Result<List<AreaDTO>>>
    {
    }
}
