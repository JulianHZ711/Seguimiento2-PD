using System;
using System.Collections.Generic;
using System.Text;

namespace Access.Application.UseCases.Credentials.Queries.GetCredentialByCode
{
    public sealed class CredentialDetailsDTO
    {
        public Guid Id { get; init; }
        public string Code { get; init; } = null!;
        public string Status { get; init; } = null!;
        public bool CanBeUsed { get; init; }
        public Guid EmployeeId { get; init; }
        public string EmployeeFullName { get; init; } = null!;
        public bool EmployeeIsActive { get; init; }
        public DateTime CreatedAt { get; init; }
        public DateTime? BlockedAt { get; init; }
        public DateTime? RevokedAt { get; init; }
    }
}
