using Access.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Access.Domain.Entities.Employees.ValueObjects
{
    public sealed record DocumentNumber
    {
        public const int MinLength = 5;
        public const int MaxLength = 20;

        // Admite cédula (solo números) y cédula de extranjería o pasaporte (letras y números).
        private static readonly Regex Pattern = new(@"^[A-Za-z0-9]+$", RegexOptions.Compiled);

        public string Value { get; private set; } = null!;

        private DocumentNumber()
        {

        }

        public DocumentNumber(string value)
        {
            ApplyValueRules(value);

            Value = value.Trim().ToUpperInvariant();
        }

        private static void ApplyValueRules(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new BussinesRuleException("La identificación es requerida.");
            }

            string trimmed = value.Trim();

            if (trimmed.Length < MinLength || trimmed.Length > MaxLength)
            {
                throw new BussinesRuleException($"La identificación debe tener entre {MinLength} y {MaxLength} caracteres.");
            }

            if (!Pattern.IsMatch(trimmed))
            {
                throw new BussinesRuleException("La identificación solo puede contener letras y números, sin puntos, guiones ni espacios.");
            }
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
