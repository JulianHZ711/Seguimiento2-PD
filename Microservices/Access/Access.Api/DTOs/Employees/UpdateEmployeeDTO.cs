using Access.Api.DTOs.Common;
using System.ComponentModel.DataAnnotations;

namespace Access.Api.DTOs.Employees
{
    public class UpdateEmployeeDTO
    {
        [NotEmptyGuid]
        [Required]
        public Guid Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Position { get; set; } = string.Empty;

        [NotEmptyGuid]
        public Guid AreaId { get; set; }
    }
}
