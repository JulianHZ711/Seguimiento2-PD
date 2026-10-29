using Access.Application.Contracts.Persistence;
using Access.Application.Contracts.Repositories;
using Access.Application.Exceptions;
using Access.Application.UseCases.Common;
using Access.Application.Utilities.Mediator;
using Access.Application.Utilities.Results;
using Access.Domain.Entities.Employees;
using Access.Domain.Entities.Employees.ValueObjects;
using Access.Domain.Exceptions;

namespace Access.Application.UseCases.Employees.Commands.CreateEmployee
{
    public sealed class CreateEmployeeUseCase :
        BaseHandler,
        IRequestHandler<CreateEmployeeCommand, Result<Guid>>
    {
        private readonly IEmployeesRepository _employeesRepository;
        private readonly IAreasRepository _areasRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateEmployeeUseCase(IEmployeesRepository employeesRepository,
                                     IAreasRepository areasRepository,
                                     IUnitOfWork unitOfWork)
        {
            _employeesRepository = employeesRepository;
            _areasRepository = areasRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(CreateEmployeeCommand command)
        {
            return await ExecuteAsync(async () =>
            {
                DocumentNumber documentNumber = new(command.DocumentNumber);
                Email email = new(command.Email);

                if (!await _areasRepository.ExistsByIdAsync(command.AreaId))
                {
                    throw new NotFoundException($"No existe un área con el ID {command.AreaId}.");
                }

                if (await _employeesRepository.ExistsByDocumentNumberAsync(documentNumber))
                {
                    throw new BussinesRuleException($"Ya existe un empleado con la identificación {documentNumber}.");
                }

                if (await _employeesRepository.ExistsByEmailAsync(email))
                {
                    throw new BussinesRuleException($"Ya existe un empleado con el correo {email}.");
                }

                Employee employee = new(documentNumber,
                                        command.FirstName,
                                        command.LastName,
                                        email,
                                        command.Position,
                                        command.AreaId);

                await _employeesRepository.CreateAsync(employee);
                await _unitOfWork.CommitAsync();

                return employee.Id;
            });
        }
    }
}
