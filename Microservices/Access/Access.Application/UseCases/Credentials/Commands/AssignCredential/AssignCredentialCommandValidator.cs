using FluentValidation;
using Access.Domain.Entities.Credentials;
using System;
using System.Collections.Generic;
using System.Text;

namespace Access.Application.UseCases.Credentials.Commands.AssignCredential
{
    public sealed class AssignCredentialCommandValidator : AbstractValidator<AssignCredentialCommand>
    {
        public AssignCredentialCommandValidator()
        {
            RuleFor(cmd => cmd.Code).NotEmpty().WithMessage("El código de la credencial es requerido.")
                                    .Length(Credential.CodeMinLength, Credential.CodeMaxLength).WithMessage($"El código de la credencial debe tener entre {Credential.CodeMinLength} y {Credential.CodeMaxLength} caracteres.")
                                    .Matches("^[A-Za-z0-9-]+$").WithMessage("El código de la credencial solo puede contener letras, números y guiones.");

            RuleFor(cmd => cmd.EmployeeId).NotEmpty().WithMessage("El empleado es requerido.");
        }
    }
}
