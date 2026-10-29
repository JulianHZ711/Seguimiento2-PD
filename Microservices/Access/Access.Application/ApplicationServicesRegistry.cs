using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Access.Application.UseCases.Areas.Queries.GetAreasList;
using Access.Application.UseCases.Employees.Commands.CreateEmployee;
using Access.Application.UseCases.Employees.Commands.DeactivateEmployee;
using Access.Application.UseCases.Employees.Commands.UpdateEmployee;
using Access.Application.UseCases.Employees.Queries.GetEmployeeById;
using Access.Application.UseCases.Employees.Queries.GetEmployeesList;
using Access.Application.Utilities.Mediator;
using Access.Application.Utilities.Pagination;
using Access.Application.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace Access.Application
{
    public static class ApplicationServicesRegistry
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Mediator
            services.AddScoped<IMediator, SimpleMediator>();

            // Use Cases - Áreas
            services.AddScoped<IRequestHandler<GetAreasListQuery, Result<List<AreaDTO>>>, GetAreasListUseCase>();

            // Use Cases - Empleados (RF-01)
            services.AddScoped<IRequestHandler<CreateEmployeeCommand, Result<Guid>>, CreateEmployeeUseCase>();
            services.AddScoped<IRequestHandler<UpdateEmployeeCommand, Result<Guid>>, UpdateEmployeeUseCase>();
            services.AddScoped<IRequestHandler<DeactivateEmployeeCommand, Result<Guid>>, DeactivateEmployeeUseCase>();
            services.AddScoped<IRequestHandler<GetEmployeeByIdQuery, Result<EmployeeDetailsDTO>>, GetEmployeeByIdUseCase>();
            services.AddScoped<IRequestHandler<GetEmployeesListQuery, Result<PaginationResponse<EmployeeListItemDTO>>>, GetEmployeesListUseCase>();

            // Validations
            services.AddValidatorsFromAssembly(typeof(ApplicationServicesRegistry).Assembly);

            return services;
        }
    }
}
