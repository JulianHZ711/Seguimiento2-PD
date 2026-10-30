using Access.Domain.Entities.AccessLogs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Access.Application.UseCases.AccessLogs.Queries.GetAccessHistory
{
    internal static class MapperExtensions
    {
        public static AccessLogListItemDTO ToListItemDTO(this AccessLog accessLog)
        {
            return new AccessLogListItemDTO
            {
                Id = accessLog.Id,
                // SQL Server no guarda el tipo de fecha; se almacena en UTC.
                OccurredAt = DateTime.SpecifyKind(accessLog.OccurredAt, DateTimeKind.Utc),
                MovementType = accessLog.MovementType.ToString(),
                Result = accessLog.Result.ToString(),
                RejectionReason = accessLog.RejectionReason,
                CredentialCode = accessLog.CredentialCode,
                EmployeeId = accessLog.EmployeeId,
                EmployeeFullName = accessLog.Employee is null ? null : $"{accessLog.Employee.FirstName} {accessLog.Employee.LastName}",
                AccessPointId = accessLog.AccessPointId,
                AccessPointName = accessLog.AccessPoint.Name,
            };
        }
    }
}
