using Access.Application.Utilities.Mediator;
using Access.Application.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace Access.Application.UseCases.Credentials.Commands.AssignCredential
{
    public sealed class AssignCredentialCommand : IRequest<Result<Guid>>
    {
        public string Code { get; set; } = string.Empty;
        public Guid EmployeeId { get; set; }
    }
}
