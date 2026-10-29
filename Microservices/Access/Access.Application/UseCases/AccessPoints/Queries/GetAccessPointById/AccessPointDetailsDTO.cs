using System;
using System.Collections.Generic;
using System.Text;

namespace Access.Application.UseCases.AccessPoints.Queries.GetAccessPointById
{
    public sealed class AccessPointDetailsDTO
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = null!;
        public string Type { get; init; } = null!;
        public bool IsEnabled { get; init; }
        public DateTime CreatedAt { get; init; }
    }
}
