using Access.Application.Contracts.Repositories;
using Access.Application.Exceptions;
using Access.Application.UseCases.Common;
using Access.Application.Utilities.Mediator;
using Access.Application.Utilities.Results;
using Access.Domain.Entities.Credentials;

namespace Access.Application.UseCases.Credentials.Queries.GetCredentialByCode
{
    public sealed class GetCredentialByCodeUseCase :
        BaseHandler,
        IRequestHandler<GetCredentialByCodeQuery, Result<CredentialDetailsDTO>>
    {
        private readonly ICredentialsRepository _credentialsRepository;

        public GetCredentialByCodeUseCase(ICredentialsRepository credentialsRepository)
        {
            _credentialsRepository = credentialsRepository;
        }

        public async Task<Result<CredentialDetailsDTO>> Handle(GetCredentialByCodeQuery query)
        {
            return await ExecuteAsync(async () =>
            {
                Credential? credential = await _credentialsRepository.GetByCodeWithEmployeeAsync(query.Code);

                if (credential is null)
                {
                    throw new NotFoundException($"No existe una credencial con el código {query.Code.Trim().ToUpperInvariant()}.");
                }

                return new CredentialDetailsDTO
                {
                    Id = credential.Id,
                    Code = credential.Code,
                    Status = credential.Status.ToString(),
                    CanBeUsed = credential.CanBeUsed,
                    EmployeeId = credential.EmployeeId,
                    EmployeeFullName = $"{credential.Employee.FirstName} {credential.Employee.LastName}",
                    EmployeeIsActive = credential.Employee.IsActive,
                    CreatedAt = credential.CreatedAt,
                    BlockedAt = credential.BlockedAt,
                    RevokedAt = credential.RevokedAt,
                };
            });
        }
    }
}
