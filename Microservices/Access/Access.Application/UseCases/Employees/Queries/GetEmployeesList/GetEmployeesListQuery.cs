using Access.Application.Utilities.Mediator;
using Access.Application.Utilities.Pagination;
using Access.Application.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace Access.Application.UseCases.Employees.Queries.GetEmployeesList
{
    public sealed class GetEmployeesListQuery : IRequest<Result<PaginationResponse<EmployeeListItemDTO>>>
    {
        public PaginationRequest Pagination { get; set; } = PaginationRequest.Standart();
    }
}
