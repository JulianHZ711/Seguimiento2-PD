using Access.Application.Contracts.Repositories;
using Access.Application.Exceptions;
using Access.Application.UseCases.Common;
using Access.Application.Utilities.Mediator;
using Access.Application.Utilities.Results;
using Access.Domain.Entities.Credentials;

namespace Access.Application.UseCases.Credentials.Queries.GetCredentialsByEmployee
{
    public sealed class GetCredentialsByEmployeeUseCase :
        BaseHandler,
        IRequestHandler<GetCredentialsByEmployeeQuery, Result<List<CredentialListItemDTO>>>
    {
        private readonly ICredentialsRepository _credentialsRepository;
        private readonly IEmployeesRepository _employeesRepository;

        public GetCredentialsByEmployeeUseCase(ICredentialsRepository credentialsRepository,
                                               IEmployeesRepository employeesRepository)
        {
            _credentialsRepository = credentialsRepository;
            _employeesRepository = employeesRepository;
        }

        public async Task<Result<List<CredentialListItemDTO>>> Handle(GetCredentialsByEmployeeQuery query)
        {
            return await ExecuteAsync(async () =>
            {
                if (await _employeesRepository.GetByIdAsync(query.EmployeeId) is null)
                {
                    throw new NotFoundException($"No existe un empleado con el ID {query.EmployeeId}.");
                }

                IEnumerable<Credential> credentials = await _credentialsRepository.GetByEmployeeIdAsync(query.EmployeeId);

                return credentials.Select(c => new CredentialListItemDTO
                                  {
                                      Id = c.Id,
                                      Code = c.Code,
                                      Status = c.Status.ToString(),
                                      CanBeUsed = c.CanBeUsed,
                                      CreatedAt = c.CreatedAt,
                                      BlockedAt = c.BlockedAt,
                                      RevokedAt = c.RevokedAt,
                                  })
                                  .ToList();
            });
        }
    }
}
