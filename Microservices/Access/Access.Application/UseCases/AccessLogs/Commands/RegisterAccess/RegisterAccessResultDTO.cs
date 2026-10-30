using System;
using System.Collections.Generic;
using System.Text;

namespace Access.Application.UseCases.AccessLogs.Commands.RegisterAccess
{
    public sealed class RegisterAccessResultDTO
    {
        public Guid AccessLogId { get; init; }
        public string Result { get; init; } = null!;
        public bool IsAuthorized { get; init; }
        public string? RejectionReason { get; init; }
        public string MovementType { get; init; } = null!;
        public string CredentialCode { get; init; } = null!;
        public Guid? EmployeeId { get; init; }
        public string? EmployeeFullName { get; init; }
        public Guid AccessPointId { get; init; }
        public string AccessPointName { get; init; } = null!;
        public DateTime OccurredAt { get; init; }
    }
}
