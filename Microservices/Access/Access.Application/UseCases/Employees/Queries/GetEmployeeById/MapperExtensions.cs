using Access.Domain.Entities.Employees;
using System;
using System.Collections.Generic;
using System.Text;

namespace Access.Application.UseCases.Employees.Queries.GetEmployeeById
{
    internal static class MapperExtensions
    {
        public static EmployeeDetailsDTO ToDetailsDTO(this Employee employee)
        {
            return new EmployeeDetailsDTO
            {
                Id = employee.Id,
                DocumentNumber = employee.DocumentNumber.Value,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email.Value,
                Position = employee.Position,
                AreaId = employee.AreaId,
                AreaName = employee.Area.Name,
                Status = employee.Status.ToString(),
                IsActive = employee.IsActive,
                CreatedAt = employee.CreatedAt,
                DeactivatedAt = employee.DeactivatedAt,
            };
        }
    }
}
