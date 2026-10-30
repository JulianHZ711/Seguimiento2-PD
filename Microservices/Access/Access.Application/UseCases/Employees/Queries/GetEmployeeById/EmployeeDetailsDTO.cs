using System;
using System.Collections.Generic;
using System.Text;

namespace Access.Application.UseCases.Employees.Queries.GetEmployeeById
{
    public sealed class EmployeeDetailsDTO
    {
        public Guid Id { get; init; }
        public string DocumentNumber { get; init; } = null!;
        public string FirstName { get; init; } = null!;
        public string LastName { get; init; } = null!;
        public string Email { get; init; } = null!;
        public string Position { get; init; } = null!;
        public Guid AreaId { get; init; }
        public string AreaName { get; init; } = null!;
        public string Status { get; init; } = null!;
        public bool IsActive { get; init; }
        public DateTime CreatedAt { get; init; }
        public DateTime? DeactivatedAt { get; init; }
    }
}
