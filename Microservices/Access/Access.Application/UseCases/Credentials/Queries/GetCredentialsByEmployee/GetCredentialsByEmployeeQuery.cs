using Access.Application.Utilities.Mediator;
using Access.Application.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace Access.Application.UseCases.Credentials.Queries.GetCredentialsByEmployee
{
    public sealed class GetCredentialsByEmployeeQuery : IRequest<Result<List<CredentialListItemDTO>>>
    {
        public Guid EmployeeId { get; set; }
    }
}
