using Access.Application.Contracts.Repositories;
using Access.Application.Exceptions;
using Access.Application.UseCases.Common;
using Access.Application.Utilities.Mediator;
using Access.Application.Utilities.Results;
using Access.Domain.Entities.Employees;

namespace Access.Application.UseCases.Employees.Queries.GetEmployeeById
{
    public sealed class GetEmployeeByIdUseCase :
        BaseHandler,
        IRequestHandler<GetEmployeeByIdQuery, Result<EmployeeDetailsDTO>>
    {
        private readonly IEmployeesRepository _employeesRepository;

        public GetEmployeeByIdUseCase(IEmployeesRepository employeesRepository)
        {
            _employeesRepository = employeesRepository;
        }

        public async Task<Result<EmployeeDetailsDTO>> Handle(GetEmployeeByIdQuery query)
        {
            return await ExecuteAsync(async () =>
            {
                Employee? employee = await _employeesRepository.GetByIdWithDetailsAsync(query.Id);

                if (employee is null)
                {
                    throw new NotFoundException($"No existe un empleado con el ID {query.Id}.");
                }

                return employee.ToDetailsDTO();
            });
        }
    }
}
