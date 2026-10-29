using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Access.Application.UseCases.AccessPoints.Commands.DisableAccessPoint
{
    public sealed class DisableAccessPointCommandValidator : AbstractValidator<DisableAccessPointCommand>
    {
        public DisableAccessPointCommandValidator()
        {
            RuleFor(cmd => cmd.Id).NotEmpty().WithMessage("El ID del punto de acceso es requerido.");
        }
    }
}
