using Access.Application.Contracts.Repositories;
using Access.Application.UseCases.Common;
using Access.Application.Utilities.Mediator;
using Access.Application.Utilities.Pagination;
using Access.Application.Utilities.Results;
using Access.Domain.Entities.Employees;

namespace Access.Application.UseCases.Employees.Queries.GetEmployeesList
{
    public sealed class GetEmployeesListUseCase :
        BaseHandler,
        IRequestHandler<GetEmployeesListQuery, Result<PaginationResponse<EmployeeListItemDTO>>>
    {
        private readonly IEmployeesRepository _employeesRepository;

        public GetEmployeesListUseCase(IEmployeesRepository employeesRepository)
        {
            _employeesRepository = employeesRepository;
        }

        public async Task<Result<PaginationResponse<EmployeeListItemDTO>>> Handle(GetEmployeesListQuery query)
        {
            return await ExecuteAsync(async () =>
            {
                PaginationRequest pagination = query.Pagination;

                PaginationResponse<Employee> response = await _employeesRepository.GetPagedListAsync(pagination,
                                                                                                    query.AreaId,
                                                                                                    query.Status);

                List<EmployeeListItemDTO> items = response.Items.Select(e => e.ToListItemDTO())
                                                                .ToList();

                return PaginationResponse<EmployeeListItemDTO>.Create(items, response.TotalCount, pagination);
            });
        }
    }
}
