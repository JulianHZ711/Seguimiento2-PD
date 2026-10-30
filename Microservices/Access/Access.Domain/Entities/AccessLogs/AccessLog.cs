using Access.Domain.Entities.AccessLogs.ValueObjects;
using Access.Domain.Entities.AccessPoints;
using Access.Domain.Entities.Credentials;
using Access.Domain.Entities.Employees;
using Access.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Access.Domain.Entities.AccessLogs
{
    // Registro inmutable de cada intento de entrada o salida, autorizado o rechazado.
    public sealed class AccessLog
    {
        public const int CredentialCodeMaxLength = 100;
        public const int RejectionReasonMaxLength = 250;

        public Guid Id { get; private set; }
        public string CredentialCode { get; private set; } = null!;
        public Guid? CredentialId { get; private set; }
        public Credential? Credential { get; private set; }
        public Guid? EmployeeId { get; private set; }
        public Employee? Employee { get; private set; }
        public Guid AccessPointId { get; private set; }
        public AccessPoint AccessPoint { get; private set; } = null!;
        public MovementType MovementType { get; private set; }
        public AccessResult Result { get; private set; }
        public string? RejectionReason { get; private set; }
        public DateTime OccurredAt { get; private set; }

        private AccessLog()
        {

        }

        private AccessLog(string credentialCode,
                          Guid? credentialId,
                          Guid? employeeId,
                          Guid accessPointId,
                          MovementType movementType,
                          AccessResult result,
                          string? rejectionReason)
        {
            ApplyCredentialCodeRules(credentialCode);
            ApplyAccessPointRules(accessPointId);
            ApplyMovementTypeRules(movementType);

            Id = Guid.CreateVersion7();
            CredentialCode = credentialCode.Trim().ToUpperInvariant();
            CredentialId = credentialId;
            EmployeeId = employeeId;
            AccessPointId = accessPointId;
            MovementType = movementType;
            Result = result;
            RejectionReason = rejectionReason?.Trim();
            OccurredAt = DateTime.UtcNow;
        }

        public static AccessLog Authorized(Credential credential,
                                           Guid accessPointId,
                                           MovementType movementType)
        {
            if (credential is null)
            {
                throw new BussinesRuleException("Un acceso autorizado requiere la credencial.");
            }

            if (!credential.CanBeUsed)
            {
                throw new BussinesRuleException("No se puede autorizar un acceso con una credencial que no está vigente.");
            }

            return new AccessLog(credential.Code,
                                 credential.Id,
                                 credential.EmployeeId,
                                 accessPointId,
                                 movementType,
                                 AccessResult.Authorized,
                                 rejectionReason: null);
        }

        // La credencial y el empleado son opcionales: el código recibido puede no existir.
        public static AccessLog Rejected(string credentialCode,
                                         Guid accessPointId,
                                         MovementType movementType,
                                         string rejectionReason,
                                         Guid? credentialId = null,
                                         Guid? employeeId = null)
        {
            ApplyRejectionReasonRules(rejectionReason);

            return new AccessLog(credentialCode,
                                 credentialId,
                                 employeeId,
                                 accessPointId,
                                 movementType,
                                 AccessResult.Rejected,
                                 rejectionReason);
        }

        private static void ApplyCredentialCodeRules(string credentialCode)
        {
            if (string.IsNullOrWhiteSpace(credentialCode))
            {
                throw new BussinesRuleException("El código de la credencial es requerido.");
            }

            if (credentialCode.Trim().Length > CredentialCodeMaxLength)
            {
                throw new BussinesRuleException($"El código de la credencial debe tener máximo {CredentialCodeMaxLength} caracteres.");
            }
        }

        private static void ApplyAccessPointRules(Guid accessPointId)
        {
            if (accessPointId == Guid.Empty)
            {
                throw new BussinesRuleException("El punto de acceso es requerido.");
            }
        }

        private static void ApplyMovementTypeRules(MovementType movementType)
        {
            if (!Enum.IsDefined(movementType))
            {
                throw new BussinesRuleException("El tipo de movimiento no es válido.");
            }
        }

        private static void ApplyRejectionReasonRules(string rejectionReason)
        {
            if (string.IsNullOrWhiteSpace(rejectionReason))
            {
                throw new BussinesRuleException("El motivo del rechazo es requerido.");
            }

            if (rejectionReason.Trim().Length > RejectionReasonMaxLength)
            {
                throw new BussinesRuleException($"El motivo del rechazo debe tener máximo {RejectionReasonMaxLength} caracteres.");
            }
        }
    }
}
