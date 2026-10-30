using System.ComponentModel.DataAnnotations;

namespace Access.Api.DTOs.Common
{
    public sealed class NotEmptyGuidAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value) => value is Guid id && id != Guid.Empty;
    }
}
