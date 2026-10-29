using FluentValidation;
using Access.Domain.Entities.AccessPoints;
using System;
using System.Collections.Generic;
using System.Text;

namespace Access.Application.UseCases.AccessPoints.Commands.CreateAccessPoint
{
    public sealed class CreateAccessPointCommandValidator : AbstractValidator<CreateAccessPointCommand>
    {
        public CreateAccessPointCommandValidator()
        {
            RuleFor(cmd => cmd.Name).NotEmpty().WithMessage("El nombre del punto de acceso es requerido.")
                                    .Length(AccessPoint.NameMinLength, AccessPoint.NameMaxLength).WithMessage($"El nombre del punto de acceso debe tener entre {AccessPoint.NameMinLength} y {AccessPoint.NameMaxLength} caracteres.");

            RuleFor(cmd => cmd.Type).IsInEnum().WithMessage("El tipo de punto de acceso no es válido. Use 1 (Recepción), 2 (Parqueadero), 3 (Bodega) o 4 (Área restringida).");
        }
    }
}
