using Access.Application.Utilities.Mediator;
using Access.Application.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace Access.Application.UseCases.Credentials.Commands.BlockCredential
{
    public sealed class BlockCredentialCommand : IRequest<Result<Guid>>
    {
        public string Code { get; set; } = string.Empty;
    }
}
