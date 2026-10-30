using FluentValidation;
using Access.Domain.Entities.Credentials;
using System;
using System.Collections.Generic;
using System.Text;

namespace Access.Application.UseCases.AccessLogs.Commands.RegisterAccess
{
    public sealed class RegisterAccessCommandValidator : AbstractValidator<RegisterAccessCommand>
    {
        public RegisterAccessCommandValidator()
        {
            // El formato del código no se valida aquí: un código desconocido debe quedar registrado como rechazo.
            RuleFor(cmd => cmd.CredentialCode).NotEmpty().WithMessage("El código de la credencial es requerido.")
                                              .MaximumLength(Credential.CodeMaxLength).WithMessage($"El código de la credencial debe tener máximo {Credential.CodeMaxLength} caracteres.");

            RuleFor(cmd => cmd.AccessPointId).NotEmpty().WithMessage("El punto de acceso es requerido.");

            RuleFor(cmd => cmd.MovementType).IsInEnum().WithMessage("El tipo de movimiento no es válido. Use 1 (Entrada) o 2 (Salida).");
        }
    }
}
