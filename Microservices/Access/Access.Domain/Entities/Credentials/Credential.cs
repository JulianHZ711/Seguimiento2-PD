using Access.Domain.Entities.Credentials.ValueObjects;
using Access.Domain.Entities.Employees;
using Access.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Access.Domain.Entities.Credentials
{
    public sealed class Credential
    {
        public const int CodeMinLength = 4;
        public const int CodeMaxLength = 50;

        // Letras, números y guiones. Ej: EMP-0001
        private static readonly Regex CodePattern = new(@"^[A-Z0-9-]+$", RegexOptions.Compiled);

        public Guid Id { get; private set; }
        public string Code { get; private set; } = null!;
        public Guid EmployeeId { get; private set; }
        public Employee Employee { get; private set; } = null!;
        public CredentialStatus Status { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? BlockedAt { get; private set; }
        public DateTime? RevokedAt { get; private set; }

        // Solo una credencial activa permite registrar entradas y salidas.
        public bool CanBeUsed => Status == CredentialStatus.Active;

        private Credential()
        {

        }

        public Credential(string code, Guid employeeId)
        {
            string normalizedCode = NormalizeCode(code);
            ApplyEmployeeRules(employeeId);

            Id = Guid.CreateVersion7();
            Code = normalizedCode;
            EmployeeId = employeeId;
            Status = CredentialStatus.Active;
            CreatedAt = DateTime.UtcNow;
        }

        public void Block()
        {
            if (Status != CredentialStatus.Active)
            {
                throw new BussinesRuleException("Solo se puede bloquear una credencial vigente.");
            }

            Status = CredentialStatus.Blocked;
            BlockedAt = DateTime.UtcNow;
        }

        // Una credencial bloqueada o vigente se puede revocar; la revocación es definitiva.
        public void Revoke()
        {
            if (Status == CredentialStatus.Revoked)
            {
                throw new BussinesRuleException("La credencial ya se encuentra revocada.");
            }

            Status = CredentialStatus.Revoked;
            RevokedAt = DateTime.UtcNow;
        }

        // Deja el código sin espacios y en mayúsculas; se usa también al buscar y al registrar accesos.
        public static string NormalizeCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new BussinesRuleException("El código de la credencial es requerido.");
            }

            string normalizedCode = code.Trim().ToUpperInvariant();

            if (normalizedCode.Length < CodeMinLength || normalizedCode.Length > CodeMaxLength)
            {
                throw new BussinesRuleException($"El código de la credencial debe tener entre {CodeMinLength} y {CodeMaxLength} caracteres.");
            }

            if (!CodePattern.IsMatch(normalizedCode))
            {
                throw new BussinesRuleException("El código de la credencial solo puede contener letras, números y guiones.");
            }

            return normalizedCode;
        }

        private static void ApplyEmployeeRules(Guid employeeId)
        {
            if (employeeId == Guid.Empty)
            {
                throw new BussinesRuleException("El empleado de la credencial es requerido.");
            }
        }
    }
}
