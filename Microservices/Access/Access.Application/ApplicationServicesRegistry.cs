using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Access.Application.UseCases.AccessLogs.Commands.RegisterAccess;
using Access.Application.UseCases.AccessLogs.Queries.GetAccessHistory;
using Access.Application.UseCases.AccessPoints.Commands.CreateAccessPoint;
using Access.Application.UseCases.AccessPoints.Queries.GetAccessPointById;
using Access.Application.UseCases.AccessPoints.Queries.GetAccessPointsList;
using Access.Application.UseCases.Credentials.Commands.AssignCredential;
using Access.Application.UseCases.Credentials.Commands.BlockCredential;
using Access.Application.UseCases.Credentials.Commands.RevokeCredential;
using Access.Application.UseCases.Credentials.Queries.GetCredentialByCode;
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

            // Use Cases - Empleados (RF-01)
            services.AddScoped<IRequestHandler<CreateEmployeeCommand, Result<Guid>>, CreateEmployeeUseCase>();
            services.AddScoped<IRequestHandler<UpdateEmployeeCommand, Result<Guid>>, UpdateEmployeeUseCase>();
            services.AddScoped<IRequestHandler<DeactivateEmployeeCommand, Result<Guid>>, DeactivateEmployeeUseCase>();
            services.AddScoped<IRequestHandler<GetEmployeeByIdQuery, Result<EmployeeDetailsDTO>>, GetEmployeeByIdUseCase>();
            services.AddScoped<IRequestHandler<GetEmployeesListQuery, Result<PaginationResponse<EmployeeListItemDTO>>>, GetEmployeesListUseCase>();

            // Use Cases - Credenciales (RF-02)
            services.AddScoped<IRequestHandler<AssignCredentialCommand, Result<Guid>>, AssignCredentialUseCase>();
            services.AddScoped<IRequestHandler<BlockCredentialCommand, Result<Guid>>, BlockCredentialUseCase>();
            services.AddScoped<IRequestHandler<RevokeCredentialCommand, Result<Guid>>, RevokeCredentialUseCase>();
            services.AddScoped<IRequestHandler<GetCredentialByCodeQuery, Result<CredentialDetailsDTO>>, GetCredentialByCodeUseCase>();

            // Use Cases - Puntos de acceso (RF-06)
            services.AddScoped<IRequestHandler<CreateAccessPointCommand, Result<Guid>>, CreateAccessPointUseCase>();
            services.AddScoped<IRequestHandler<GetAccessPointByIdQuery, Result<AccessPointDetailsDTO>>, GetAccessPointByIdUseCase>();
            services.AddScoped<IRequestHandler<GetAccessPointsListQuery, Result<PaginationResponse<AccessPointListItemDTO>>>, GetAccessPointsListUseCase>();

            // Use Cases - Registro e historial de accesos (RF-03 y RF-04)
            services.AddScoped<IRequestHandler<RegisterAccessCommand, Result<RegisterAccessResultDTO>>, RegisterAccessUseCase>();
            services.AddScoped<IRequestHandler<GetAccessHistoryQuery, Result<PaginationResponse<AccessLogListItemDTO>>>, GetAccessHistoryUseCase>();

            // Validations
            services.AddValidatorsFromAssembly(typeof(ApplicationServicesRegistry).Assembly);

            return services;
        }
    }
}
