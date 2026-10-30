using Access.Domain.Entities.Areas;
using Access.Domain.Entities.Employees.ValueObjects;
using Access.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Access.Domain.Entities.Employees
{
    public sealed class Employee
    {
        public const int NameMinLength = 2;
        public const int NameMaxLength = 100;
        public const int PositionMinLength = 2;
        public const int PositionMaxLength = 100;

        public Guid Id { get; private set; }
        public DocumentNumber DocumentNumber { get; private set; } = null!;
        public string FirstName { get; private set; } = null!;
        public string LastName { get; private set; } = null!;
        public Email Email { get; private set; } = null!;
        public string Position { get; private set; } = null!;
        public Guid AreaId { get; private set; }
        public Area Area { get; private set; } = null!;
        public EmploymentStatus Status { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? DeactivatedAt { get; private set; }

        public bool IsActive => Status == EmploymentStatus.Active;

        private Employee()
        {

        }

        public Employee(DocumentNumber documentNumber,
                        string firstName,
                        string lastName,
                        Email email,
                        string position,
                        Guid areaId)
        {
            ApplyDocumentNumberRules(documentNumber);
            ApplyFirstNameRules(firstName);
            ApplyLastNameRules(lastName);
            ApplyEmailRules(email);
            ApplyPositionRules(position);
            ApplyAreaRules(areaId);

            Id = Guid.CreateVersion7();
            DocumentNumber = documentNumber;
            FirstName = firstName.Trim();
            LastName = lastName.Trim();
            Email = email;
            Position = position.Trim();
            AreaId = areaId;
            Status = EmploymentStatus.Active;
            CreatedAt = DateTime.UtcNow;
        }

        // La identificación no cambia: es lo que distingue al empleado.
        public void Update(string firstName,
                           string lastName,
                           Email email,
                           string position,
                           Guid areaId)
        {
            ApplyIsActiveRules("No se puede actualizar un empleado inactivo.");
            ApplyFirstNameRules(firstName);
            ApplyLastNameRules(lastName);
            ApplyEmailRules(email);
            ApplyPositionRules(position);
            ApplyAreaRules(areaId);

            FirstName = firstName.Trim();
            LastName = lastName.Trim();
            Email = email;
            Position = position.Trim();
            AreaId = areaId;
        }

        public void Deactivate()
        {
            ApplyIsActiveRules("El empleado ya se encuentra inactivo.");

            Status = EmploymentStatus.Inactive;
            DeactivatedAt = DateTime.UtcNow;
        }

        private void ApplyIsActiveRules(string message)
        {
            if (!IsActive)
            {
                throw new BussinesRuleException(message);
            }
        }

        private static void ApplyDocumentNumberRules(DocumentNumber documentNumber)
        {
            if (documentNumber is null)
            {
                throw new BussinesRuleException("La identificación es requerida.");
            }
        }

        private static void ApplyFirstNameRules(string firstName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
            {
                throw new BussinesRuleException("Los nombres son requeridos.");
            }

            if (firstName.Trim().Length < NameMinLength || firstName.Trim().Length > NameMaxLength)
            {
                throw new BussinesRuleException($"Los nombres deben tener entre {NameMinLength} y {NameMaxLength} caracteres.");
            }
        }

        private static void ApplyLastNameRules(string lastName)
        {
            if (string.IsNullOrWhiteSpace(lastName))
            {
                throw new BussinesRuleException("Los apellidos son requeridos.");
            }

            if (lastName.Trim().Length < NameMinLength || lastName.Trim().Length > NameMaxLength)
            {
                throw new BussinesRuleException($"Los apellidos deben tener entre {NameMinLength} y {NameMaxLength} caracteres.");
            }
        }

        private static void ApplyEmailRules(Email email)
        {
            if (email is null)
            {
                throw new BussinesRuleException("El correo electrónico es requerido.");
            }
        }

        private static void ApplyPositionRules(string position)
        {
            if (string.IsNullOrWhiteSpace(position))
            {
                throw new BussinesRuleException("El cargo es requerido.");
            }

            if (position.Trim().Length < PositionMinLength || position.Trim().Length > PositionMaxLength)
            {
                throw new BussinesRuleException($"El cargo debe tener entre {PositionMinLength} y {PositionMaxLength} caracteres.");
            }
        }

        private static void ApplyAreaRules(Guid areaId)
        {
            if (areaId == Guid.Empty)
            {
                throw new BussinesRuleException("El área organizacional es requerida.");
            }
        }
    }
}
