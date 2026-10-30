using System;
using System.Collections.Generic;
using System.Text;

namespace Access.Application.UseCases.Employees.Queries.GetEmployeesList
{
    public sealed class EmployeeListItemDTO
    {
        public Guid Id { get; init; }
        public string DocumentNumber { get; init; } = null!;
        public string FullName { get; init; } = null!;
        public string Email { get; init; } = null!;
        public string Position { get; init; } = null!;
        public string AreaName { get; init; } = null!;
        public string Status { get; init; } = null!;
        public bool IsActive { get; init; }
    }
}
