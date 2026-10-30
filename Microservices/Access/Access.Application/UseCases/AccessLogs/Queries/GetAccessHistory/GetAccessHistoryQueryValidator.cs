using FluentValidation;
using Access.Application.Utilities.Pagination;
using System;
using System.Collections.Generic;
using System.Text;

namespace Access.Application.UseCases.AccessLogs.Queries.GetAccessHistory
{
    public sealed class GetAccessHistoryQueryValidator : AbstractValidator<GetAccessHistoryQuery>
    {
        public GetAccessHistoryQueryValidator()
        {
            RuleFor(query => query.Pagination).SetValidator(new PaginationRequestValidator());

            RuleFor(query => query.Result).IsInEnum().WithMessage("El resultado no es válido. Use Authorized o Rejected.");

            RuleFor(query => query)
                .Must(query => !query.From.HasValue || !query.To.HasValue || query.From.Value <= query.To.Value)
                .OverridePropertyName("To")
                .WithMessage("La fecha inicial no puede ser mayor que la fecha final.");
        }
    }
}
