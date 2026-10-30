using Access.Application.Utilities.Mediator;
using Access.Application.Utilities.Results;
using Access.Domain.Entities.AccessLogs.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Access.Application.UseCases.AccessLogs.Commands.RegisterAccess
{
    public sealed class RegisterAccessCommand : IRequest<Result<RegisterAccessResultDTO>>
    {
        public string CredentialCode { get; set; } = string.Empty;
        public Guid AccessPointId { get; set; }
        public MovementType MovementType { get; set; }
    }
}
