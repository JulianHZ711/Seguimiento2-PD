using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Access.Application.UseCases.AccessPoints.Commands.EnableAccessPoint
{
    public sealed class EnableAccessPointCommandValidator : AbstractValidator<EnableAccessPointCommand>
    {
        public EnableAccessPointCommandValidator()
        {
            RuleFor(cmd => cmd.Id).NotEmpty().WithMessage("El ID del punto de acceso es requerido.");
        }
    }
}
