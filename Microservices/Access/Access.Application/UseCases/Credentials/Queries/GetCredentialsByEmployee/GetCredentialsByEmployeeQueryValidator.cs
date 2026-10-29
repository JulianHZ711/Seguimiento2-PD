using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Access.Application.UseCases.Credentials.Queries.GetCredentialsByEmployee
{
    public sealed class GetCredentialsByEmployeeQueryValidator : AbstractValidator<GetCredentialsByEmployeeQuery>
    {
        public GetCredentialsByEmployeeQueryValidator()
        {
            RuleFor(query => query.EmployeeId).NotEmpty().WithMessage("El empleado es requerido.");
        }
    }
}
