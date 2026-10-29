using Access.Application.Utilities.Mediator;
using Access.Application.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace Access.Application.UseCases.Employees.Queries.GetEmployeeById
{
    public sealed class GetEmployeeByIdQuery : IRequest<Result<EmployeeDetailsDTO>>
    {
        public Guid Id { get; set; }
    }
}
