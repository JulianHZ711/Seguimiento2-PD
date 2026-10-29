using Access.Application.Utilities.Mediator;
using Access.Application.Utilities.Pagination;
using Access.Application.Utilities.Results;
using Access.Domain.Entities.Employees.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Access.Application.UseCases.Employees.Queries.GetEmployeesList
{
    public sealed class GetEmployeesListQuery : IRequest<Result<PaginationResponse<EmployeeListItemDTO>>>
    {
        public PaginationRequest Pagination { get; set; } = PaginationRequest.Standart();

        public Guid? AreaId { get; set; }
        public EmploymentStatus? Status { get; set; }
    }
}
