using Access.Application.Contracts.Persistence;
using Access.Application.Contracts.Repositories;
using Access.Application.Exceptions;
using Access.Application.UseCases.Common;
using Access.Application.Utilities.Mediator;
using Access.Application.Utilities.Results;
using Access.Domain.Entities.Credentials;
using Access.Domain.Entities.Employees;
using Access.Domain.Exceptions;

namespace Access.Application.UseCases.Credentials.Commands.AssignCredential
{
    public sealed class AssignCredentialUseCase :
        BaseHandler,
        IRequestHandler<AssignCredentialCommand, Result<Guid>>
    {
        private readonly ICredentialsRepository _credentialsRepository;
        private readonly IEmployeesRepository _employeesRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AssignCredentialUseCase(ICredentialsRepository credentialsRepository,
                                       IEmployeesRepository employeesRepository,
                                       IUnitOfWork unitOfWork)
        {
            _credentialsRepository = credentialsRepository;
            _employeesRepository = employeesRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(AssignCredentialCommand command)
        {
            return await ExecuteAsync(async () =>
            {
                string code = Credential.NormalizeCode(command.Code);

                Employee? employee = await _employeesRepository.GetByIdAsync(command.EmployeeId);

                if (employee is null)
                {
                    throw new NotFoundException($"No existe un empleado con el ID {command.EmployeeId}.");
                }

                if (!employee.IsActive)
                {
                    throw new BussinesRuleException("No se puede asignar una credencial a un empleado inactivo.");
                }

                if (await _credentialsRepository.ExistsByCodeAsync(code))
                {
                    throw new BussinesRuleException($"Ya existe una credencial con el código {code}.");
                }

                Credential credential = new(code, employee.Id);

                await _credentialsRepository.CreateAsync(credential);
                await _unitOfWork.CommitAsync();

                return credential.Id;
            });
        }
    }
}
