using Access.Application.Contracts.Persistence;
using Access.Application.Contracts.Repositories;
using Access.Application.Exceptions;
using Access.Application.UseCases.Common;
using Access.Application.Utilities.Mediator;
using Access.Application.Utilities.Results;
using Access.Domain.Entities.Employees;
using Access.Domain.Entities.Employees.ValueObjects;
using Access.Domain.Exceptions;

namespace Access.Application.UseCases.Employees.Commands.UpdateEmployee
{
    public sealed class UpdateEmployeeUseCase :
        BaseHandler,
        IRequestHandler<UpdateEmployeeCommand, Result<Guid>>
    {
        private readonly IEmployeesRepository _employeesRepository;
        private readonly IAreasRepository _areasRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateEmployeeUseCase(IEmployeesRepository employeesRepository,
                                     IAreasRepository areasRepository,
                                     IUnitOfWork unitOfWork)
        {
            _employeesRepository = employeesRepository;
            _areasRepository = areasRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(UpdateEmployeeCommand command)
        {
            return await ExecuteAsync(async () =>
            {
                Employee? employee = await _employeesRepository.GetByIdAsync(command.Id);

                if (employee is null)
                {
                    throw new NotFoundException($"No existe un empleado con el ID {command.Id}.");
                }

                Email email = new(command.Email);

                if (!await _areasRepository.ExistsByIdAsync(command.AreaId))
                {
                    throw new NotFoundException($"No existe un área con el ID {command.AreaId}.");
                }

                if (await _employeesRepository.ExistsByEmailAsync(email, excludeEmployeeId: employee.Id))
                {
                    throw new BussinesRuleException($"Ya existe otro empleado con el correo {email}.");
                }

                employee.Update(command.FirstName,
                                command.LastName,
                                email,
                                command.Position,
                                command.AreaId);

                await _employeesRepository.UpdateAsync(employee);
                await _unitOfWork.CommitAsync();

                return employee.Id;
            });
        }
    }
}
