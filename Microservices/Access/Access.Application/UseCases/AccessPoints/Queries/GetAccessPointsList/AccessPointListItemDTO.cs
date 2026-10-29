using System;
using System.Collections.Generic;
using System.Text;

namespace Access.Application.UseCases.AccessPoints.Queries.GetAccessPointsList
{
    public sealed class AccessPointListItemDTO
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = null!;
        public string Type { get; init; } = null!;
        public bool IsEnabled { get; init; }
    }
}
