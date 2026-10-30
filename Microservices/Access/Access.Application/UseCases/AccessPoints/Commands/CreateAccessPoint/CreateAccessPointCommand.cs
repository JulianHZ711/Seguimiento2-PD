using Access.Application.Utilities.Mediator;
using Access.Application.Utilities.Results;
using Access.Domain.Entities.AccessPoints.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Access.Application.UseCases.AccessPoints.Commands.CreateAccessPoint
{
    public sealed class CreateAccessPointCommand : IRequest<Result<Guid>>
    {
        public string Name { get; set; } = string.Empty;
        public AccessPointType Type { get; set; }
        public bool IsEnabled { get; set; } = true;
    }
}
