using Access.Domain.Entities.AccessPoints.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace Access.Api.DTOs.AccessPoints
{
    public class CreateAccessPointDTO
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; } = string.Empty;

        // 1 = Recepción, 2 = Parqueadero, 3 = Bodega, 4 = Área restringida
        public AccessPointType Type { get; set; }

        public bool IsEnabled { get; set; } = true;
    }
}
