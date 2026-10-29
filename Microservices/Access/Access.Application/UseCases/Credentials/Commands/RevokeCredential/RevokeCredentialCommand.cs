using Access.Application.Utilities.Mediator;
using Access.Application.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace Access.Application.UseCases.Credentials.Commands.RevokeCredential
{
    public sealed class RevokeCredentialCommand : IRequest<Result<Guid>>
    {
        public string Code { get; set; } = string.Empty;
    }
}
