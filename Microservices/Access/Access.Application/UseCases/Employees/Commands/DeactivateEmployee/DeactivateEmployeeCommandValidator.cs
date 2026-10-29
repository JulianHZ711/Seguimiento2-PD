using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Access.Application.UseCases.Employees.Commands.DeactivateEmployee
{
    public sealed class DeactivateEmployeeCommandValidator : AbstractValidator<DeactivateEmployeeCommand>
    {
        public DeactivateEmployeeCommandValidator()
        {
            RuleFor(cmd => cmd.Id).NotEmpty().WithMessage("El ID del empleado es requerido.");
        }
    }
}
