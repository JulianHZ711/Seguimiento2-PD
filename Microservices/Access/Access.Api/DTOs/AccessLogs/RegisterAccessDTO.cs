using Access.Api.DTOs.Common;
using Access.Domain.Entities.AccessLogs.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace Access.Api.DTOs.AccessLogs
{
    public class RegisterAccessDTO
    {
        [Required]
        [StringLength(50)]
        public string CredentialCode { get; set; } = string.Empty;

        [NotEmptyGuid]
        public Guid AccessPointId { get; set; }

        // 1 = Entrada, 2 = Salida
        public MovementType MovementType { get; set; }
    }
}
