using Access.Application.Contracts.Persistence;
using Access.Application.Contracts.Repositories;
using Access.Application.Exceptions;
using Access.Application.UseCases.Common;
using Access.Application.Utilities.Mediator;
using Access.Application.Utilities.Results;
using Access.Domain.Entities.AccessLogs;
using Access.Domain.Entities.AccessLogs.ValueObjects;
using Access.Domain.Entities.AccessPoints;
using Access.Domain.Entities.Credentials;
using Access.Domain.Entities.Credentials.ValueObjects;

namespace Access.Application.UseCases.AccessLogs.Commands.RegisterAccess
{
    // Registra cada intento de entrada o salida. Un intento rechazado no es un error de la API:
    // queda guardado en el historial con su motivo y se responde con el resultado.
    public sealed class RegisterAccessUseCase :
        BaseHandler,
        IRequestHandler<RegisterAccessCommand, Result<RegisterAccessResultDTO>>
    {
        private readonly IAccessLogsRepository _accessLogsRepository;
        private readonly ICredentialsRepository _credentialsRepository;
        private readonly IAccessPointsRepository _accessPointsRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterAccessUseCase(IAccessLogsRepository accessLogsRepository,
                                     ICredentialsRepository credentialsRepository,
                                     IAccessPointsRepository accessPointsRepository,
                                     IUnitOfWork unitOfWork)
        {
            _accessLogsRepository = accessLogsRepository;
            _credentialsRepository = credentialsRepository;
            _accessPointsRepository = accessPointsRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<RegisterAccessResultDTO>> Handle(RegisterAccessCommand command)
        {
            return await ExecuteAsync(async () =>
            {
                AccessPoint? accessPoint = await _accessPointsRepository.GetByIdAsync(command.AccessPointId);

                if (accessPoint is null)
                {
                    throw new NotFoundException($"No existe un punto de acceso con el ID {command.AccessPointId}.");
                }

                Credential? credential = await _credentialsRepository.GetByCodeWithEmployeeAsync(command.CredentialCode);

                string? rejectionReason = GetRejectionReason(credential, accessPoint);

                AccessLog accessLog = rejectionReason is null
                    ? AccessLog.Authorized(credential!, accessPoint.Id, command.MovementType)
                    : AccessLog.Rejected(command.CredentialCode,
                                         accessPoint.Id,
                                         command.MovementType,
                                         rejectionReason,
                                         credential?.Id,
                                         credential?.EmployeeId);

                await _accessLogsRepository.CreateAsync(accessLog);
                await _unitOfWork.CommitAsync();

                return new RegisterAccessResultDTO
                {
                    AccessLogId = accessLog.Id,
                    Result = accessLog.Result.ToString(),
                    IsAuthorized = accessLog.Result == AccessResult.Authorized,
                    RejectionReason = accessLog.RejectionReason,
                    MovementType = accessLog.MovementType.ToString(),
                    CredentialCode = accessLog.CredentialCode,
                    EmployeeId = accessLog.EmployeeId,
                    EmployeeFullName = credential is null ? null : $"{credential.Employee.FirstName} {credential.Employee.LastName}",
                    AccessPointId = accessPoint.Id,
                    AccessPointName = accessPoint.Name,
                    OccurredAt = DateTime.SpecifyKind(accessLog.OccurredAt, DateTimeKind.Utc),
                };
            });
        }

        // Devuelve el motivo del rechazo, o null si el acceso se puede autorizar.
        private static string? GetRejectionReason(Credential? credential, AccessPoint accessPoint)
        {
            if (credential is null)
            {
                return "La credencial no está registrada.";
            }

            if (credential.Status == CredentialStatus.Blocked)
            {
                return "La credencial está bloqueada.";
            }

            if (credential.Status == CredentialStatus.Revoked)
            {
                return "La credencial está revocada.";
            }

            if (!credential.Employee.IsActive)
            {
                return "El empleado no tiene un vínculo laboral activo.";
            }

            if (!accessPoint.IsEnabled)
            {
                return "El punto de acceso está deshabilitado.";
            }

            return null;
        }
    }
}
