using Access.Domain.Entities.AccessPoints.ValueObjects;
using Access.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Access.Domain.Entities.AccessPoints
{
    public sealed class AccessPoint
    {
        public const int NameMinLength = 3;
        public const int NameMaxLength = 100;

        public Guid Id { get; private set; }
        public string Name { get; private set; } = null!;
        public AccessPointType Type { get; private set; }
        public bool IsEnabled { get; private set; }
        public DateTime CreatedAt { get; private set; }

        private AccessPoint()
        {

        }

        public AccessPoint(string name, AccessPointType type, bool isEnabled = true)
        {
            ApplyNameRules(name);
            ApplyTypeRules(type);

            Id = Guid.CreateVersion7();
            Name = name.Trim();
            Type = type;
            IsEnabled = isEnabled;
            CreatedAt = DateTime.UtcNow;
        }

        public void Enable()
        {
            if (IsEnabled)
            {
                throw new BussinesRuleException("El punto de acceso ya está habilitado.");
            }

            IsEnabled = true;
        }

        public void Disable()
        {
            if (!IsEnabled)
            {
                throw new BussinesRuleException("El punto de acceso ya está deshabilitado.");
            }

            IsEnabled = false;
        }

        private static void ApplyNameRules(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new BussinesRuleException("El nombre del punto de acceso es requerido.");
            }

            if (name.Trim().Length < NameMinLength || name.Trim().Length > NameMaxLength)
            {
                throw new BussinesRuleException($"El nombre del punto de acceso debe tener entre {NameMinLength} y {NameMaxLength} caracteres.");
            }
        }

        private static void ApplyTypeRules(AccessPointType type)
        {
            if (!Enum.IsDefined(type))
            {
                throw new BussinesRuleException("El tipo de punto de acceso no es válido.");
            }
        }
    }
}
