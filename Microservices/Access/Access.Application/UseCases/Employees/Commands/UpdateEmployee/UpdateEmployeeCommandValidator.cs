using FluentValidation;
using Access.Domain.Entities.Employees;
using Access.Domain.Entities.Employees.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Access.Application.UseCases.Employees.Commands.UpdateEmployee
{
    public sealed class UpdateEmployeeCommandValidator : AbstractValidator<UpdateEmployeeCommand>
    {
        public UpdateEmployeeCommandValidator()
        {
            RuleFor(cmd => cmd.Id).NotEmpty().WithMessage("El ID del empleado es requerido.");

            RuleFor(cmd => cmd.FirstName).NotEmpty().WithMessage("Los nombres son requeridos.")
                                         .Length(Employee.NameMinLength, Employee.NameMaxLength).WithMessage($"Los nombres deben tener entre {Employee.NameMinLength} y {Employee.NameMaxLength} caracteres.");

            RuleFor(cmd => cmd.LastName).NotEmpty().WithMessage("Los apellidos son requeridos.")
                                        .Length(Employee.NameMinLength, Employee.NameMaxLength).WithMessage($"Los apellidos deben tener entre {Employee.NameMinLength} y {Employee.NameMaxLength} caracteres.");

            RuleFor(cmd => cmd.Email).NotEmpty().WithMessage("El correo electrónico es requerido.")
                                     .MaximumLength(Email.MaxLength).WithMessage($"El correo electrónico debe tener máximo {Email.MaxLength} caracteres.")
                                     .EmailAddress().WithMessage("El correo electrónico no tiene un formato válido.");

            RuleFor(cmd => cmd.Position).NotEmpty().WithMessage("El cargo es requerido.")
                                        .Length(Employee.PositionMinLength, Employee.PositionMaxLength).WithMessage($"El cargo debe tener entre {Employee.PositionMinLength} y {Employee.PositionMaxLength} caracteres.");

            RuleFor(cmd => cmd.AreaId).NotEmpty().WithMessage("El área organizacional es requerida.");
        }
    }
}
