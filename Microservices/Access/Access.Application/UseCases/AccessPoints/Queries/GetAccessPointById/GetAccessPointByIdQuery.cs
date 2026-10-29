using Access.Application.Utilities.Mediator;
using Access.Application.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace Access.Application.UseCases.AccessPoints.Queries.GetAccessPointById
{
    public sealed class GetAccessPointByIdQuery : IRequest<Result<AccessPointDetailsDTO>>
    {
        public Guid Id { get; set; }
    }
}
