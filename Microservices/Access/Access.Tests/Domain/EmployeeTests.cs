using Access.Domain.Entities.Areas;
using Access.Domain.Entities.Employees;
using Access.Domain.Entities.Employees.ValueObjects;
using Access.Domain.Exceptions;

namespace Access.Tests.Domain
{
    [TestClass]
    public sealed class EmployeeTests
    {
        private static Employee CreateEmployee()
        {
            return new Employee(new DocumentNumber("1036123456"),
                                "Ana María",
                                "Gómez Ruiz",
                                new Email("ana.gomez@empresa.com"),
                                "Auxiliar de bodega",
                                Guid.NewGuid());
        }

        [TestMethod]
        public void Constructor_CreatesActiveEmployee()
        {
            Employee employee = CreateEmployee();

            Assert.AreNotEqual(Guid.Empty, employee.Id);
            Assert.AreEqual(EmploymentStatus.Active, employee.Status);
            Assert.IsTrue(employee.IsActive);
            Assert.IsNull(employee.DeactivatedAt);
        }

        [TestMethod]
        public void Constructor_WithoutArea_Throws()
        {
            Assert.ThrowsExactly<BussinesRuleException>(() =>
                new Employee(new DocumentNumber("1036123456"), "Ana", "Gómez", new Email("ana@empresa.com"), "Auxiliar", Guid.Empty));
        }

        [TestMethod]
        [DataRow("")]
        [DataRow("   ")]
        [DataRow("A")]
        public void Constructor_WithInvalidFirstName_Throws(string firstName)
        {
            Assert.ThrowsExactly<BussinesRuleException>(() =>
                new Employee(new DocumentNumber("1036123456"), firstName, "Gómez", new Email("ana@empresa.com"), "Auxiliar", Guid.NewGuid()));
        }

        [TestMethod]
        public void Update_ChangesDataButKeepsDocumentNumber()
        {
            Employee employee = CreateEmployee();
            Guid newAreaId = Guid.NewGuid();

            employee.Update("Ana", "Gómez", new Email("ana.nueva@empresa.com"), "Coordinadora", newAreaId);

            Assert.AreEqual("1036123456", employee.DocumentNumber.Value);
            Assert.AreEqual("ana.nueva@empresa.com", employee.Email.Value);
            Assert.AreEqual("Coordinadora", employee.Position);
            Assert.AreEqual(newAreaId, employee.AreaId);
        }

        [TestMethod]
        public void Deactivate_ChangesStatusAndRegistersDate()
        {
            Employee employee = CreateEmployee();

            employee.Deactivate();

            Assert.AreEqual(EmploymentStatus.Inactive, employee.Status);
            Assert.IsFalse(employee.IsActive);
            Assert.IsNotNull(employee.DeactivatedAt);
        }

        [TestMethod]
        public void Deactivate_WhenAlreadyInactive_Throws()
        {
            Employee employee = CreateEmployee();
            employee.Deactivate();

            Assert.ThrowsExactly<BussinesRuleException>(() => employee.Deactivate());
        }

        [TestMethod]
        public void Update_WhenInactive_Throws()
        {
            Employee employee = CreateEmployee();
            employee.Deactivate();

            Assert.ThrowsExactly<BussinesRuleException>(() =>
                employee.Update("Ana", "Gómez", new Email("ana@empresa.com"), "Auxiliar", Guid.NewGuid()));
        }

        [TestMethod]
        public void Email_IsNormalizedToLowerCase()
        {
            Email email = new("  Ana.Gomez@Empresa.COM ");

            Assert.AreEqual("ana.gomez@empresa.com", email.Value);
        }

        [TestMethod]
        [DataRow("")]
        [DataRow("correo-sin-arroba")]
        [DataRow("ana@empresa")]
        public void Email_WithInvalidFormat_Throws(string value)
        {
            Assert.ThrowsExactly<BussinesRuleException>(() => new Email(value));
        }

        [TestMethod]
        public void DocumentNumber_IsNormalizedToUpperCase()
        {
            DocumentNumber document = new(" ab12345 ");

            Assert.AreEqual("AB12345", document.Value);
        }

        [TestMethod]
        [DataRow("")]
        [DataRow("1234")]
        [DataRow("1.036.123.456")]
        public void DocumentNumber_WithInvalidValue_Throws(string value)
        {
            Assert.ThrowsExactly<BussinesRuleException>(() => new DocumentNumber(value));
        }

        [TestMethod]
        public void Area_WithoutName_Throws()
        {
            Assert.ThrowsExactly<BussinesRuleException>(() => new Area("  "));
        }
    }
}
