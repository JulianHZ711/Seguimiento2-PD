using FluentValidation;
using Access.Application.Utilities.Pagination;
using System;
using System.Collections.Generic;
using System.Text;

namespace Access.Application.UseCases.Employees.Queries.GetEmployeesList
{
    public sealed class GetEmployeesListQueryValidator : AbstractValidator<GetEmployeesListQuery>
    {
        public GetEmployeesListQueryValidator()
        {
            RuleFor(query => query.Pagination).SetValidator(new PaginationRequestValidator());
        }
    }
}
