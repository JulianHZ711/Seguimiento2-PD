using Access.Application.Utilities.Mediator;
using Access.Application.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace Access.Application.UseCases.AccessPoints.Commands.DisableAccessPoint
{
    public sealed class DisableAccessPointCommand : IRequest<Result<Guid>>
    {
        public Guid Id { get; set; }
    }
}
