using Access.Domain.Entities.Employees;
using System;
using System.Collections.Generic;
using System.Text;

namespace Access.Application.UseCases.Employees.Queries.GetEmployeesList
{
    internal static class MapperExtensions
    {
        public static EmployeeListItemDTO ToListItemDTO(this Employee employee)
        {
            return new EmployeeListItemDTO
            {
                Id = employee.Id,
                DocumentNumber = employee.DocumentNumber.Value,
                FullName = $"{employee.FirstName} {employee.LastName}",
                Email = employee.Email.Value,
                Position = employee.Position,
                AreaName = employee.Area.Name,
                Status = employee.Status.ToString(),
                IsActive = employee.IsActive,
            };
        }
    }
}
