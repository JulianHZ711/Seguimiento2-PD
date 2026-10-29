using System;
using System.Collections.Generic;
using System.Text;

namespace Access.Application.UseCases.Areas.Queries.GetAreasList
{
    public sealed class AreaDTO
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = null!;
        public string? Description { get; init; }
    }
}
