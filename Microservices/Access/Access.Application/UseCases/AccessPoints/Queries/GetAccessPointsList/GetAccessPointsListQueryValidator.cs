using FluentValidation;
using Access.Application.Utilities.Pagination;
using System;
using System.Collections.Generic;
using System.Text;

namespace Access.Application.UseCases.AccessPoints.Queries.GetAccessPointsList
{
    public sealed class GetAccessPointsListQueryValidator : AbstractValidator<GetAccessPointsListQuery>
    {
        public GetAccessPointsListQueryValidator()
        {
            RuleFor(query => query.Pagination).SetValidator(new PaginationRequestValidator());

            RuleFor(query => query.Type).IsInEnum().WithMessage("El tipo de punto de acceso no es válido.");
        }
    }
}
