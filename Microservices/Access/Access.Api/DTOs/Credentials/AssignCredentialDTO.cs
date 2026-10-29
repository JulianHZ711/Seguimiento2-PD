using Access.Api.DTOs.Common;
using System.ComponentModel.DataAnnotations;

namespace Access.Api.DTOs.Credentials
{
    public class AssignCredentialDTO
    {
        [Required]
        [StringLength(50, MinimumLength = 4)]
        public string Code { get; set; } = string.Empty;

        [NotEmptyGuid]
        public Guid EmployeeId { get; set; }
    }
}
