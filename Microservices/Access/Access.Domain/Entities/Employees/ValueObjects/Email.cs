using Access.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Access.Domain.Entities.Employees.ValueObjects
{
    public sealed record Email
    {
        public const int MaxLength = 150;

        private static readonly Regex Pattern = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

        public string Value { get; private set; } = null!;

        private Email()
        {

        }

        public Email(string value)
        {
            ApplyValueRules(value);

            // Se guarda en minúsculas para que el mismo correo no se registre dos veces.
            Value = value.Trim().ToLowerInvariant();
        }

        private static void ApplyValueRules(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new BussinesRuleException("El correo electrónico es requerido.");
            }

            if (value.Trim().Length > MaxLength)
            {
                throw new BussinesRuleException($"El correo electrónico debe tener máximo {MaxLength} caracteres.");
            }

            if (!Pattern.IsMatch(value.Trim()))
            {
                throw new BussinesRuleException("El correo electrónico no tiene un formato válido.");
            }
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
