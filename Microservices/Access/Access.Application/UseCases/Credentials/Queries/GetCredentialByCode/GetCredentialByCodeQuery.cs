using Access.Application.Utilities.Mediator;
using Access.Application.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace Access.Application.UseCases.Credentials.Queries.GetCredentialByCode
{
    public sealed class GetCredentialByCodeQuery : IRequest<Result<CredentialDetailsDTO>>
    {
        public string Code { get; set; } = string.Empty;
    }
}
