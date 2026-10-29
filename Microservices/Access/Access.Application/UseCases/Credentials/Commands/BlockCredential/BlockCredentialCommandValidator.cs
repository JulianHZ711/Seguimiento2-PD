using FluentValidation;
using Access.Domain.Entities.Credentials;
using System;
using System.Collections.Generic;
using System.Text;

namespace Access.Application.UseCases.Credentials.Commands.BlockCredential
{
    public sealed class BlockCredentialCommandValidator : AbstractValidator<BlockCredentialCommand>
    {
        public BlockCredentialCommandValidator()
        {
            RuleFor(cmd => cmd.Code).NotEmpty().WithMessage("El código de la credencial es requerido.")
                                    .Length(Credential.CodeMinLength, Credential.CodeMaxLength).WithMessage($"El código de la credencial debe tener entre {Credential.CodeMinLength} y {Credential.CodeMaxLength} caracteres.")
                                    .Matches("^[A-Za-z0-9-]+$").WithMessage("El código de la credencial solo puede contener letras, números y guiones.");
        }
    }
}
