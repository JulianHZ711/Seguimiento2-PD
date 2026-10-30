using Access.Domain.Entities.Credentials;
using Access.Domain.Entities.Credentials.ValueObjects;
using Access.Domain.Exceptions;

namespace Access.Tests.Domain
{
    [TestClass]
    public sealed class CredentialTests
    {
        [TestMethod]
        public void Constructor_NormalizesCodeAndStartsActive()
        {
            Credential credential = new("  emp-0001 ", Guid.NewGuid());

            Assert.AreEqual("EMP-0001", credential.Code);
            Assert.AreEqual(CredentialStatus.Active, credential.Status);
            Assert.IsTrue(credential.CanBeUsed);
        }

        [TestMethod]
        [DataRow("")]
        [DataRow("AB1")]
        [DataRow("EMP 0001")]
        [DataRow("EMP_0001")]
        public void Constructor_WithInvalidCode_Throws(string code)
        {
            Assert.ThrowsExactly<BussinesRuleException>(() => new Credential(code, Guid.NewGuid()));
        }

        [TestMethod]
        public void Constructor_WithoutEmployee_Throws()
        {
            Assert.ThrowsExactly<BussinesRuleException>(() => new Credential("EMP-0001", Guid.Empty));
        }

        [TestMethod]
        public void Block_ActiveCredential_CannotBeUsed()
        {
            Credential credential = new("EMP-0001", Guid.NewGuid());

            credential.Block();

            Assert.AreEqual(CredentialStatus.Blocked, credential.Status);
            Assert.IsFalse(credential.CanBeUsed);
            Assert.IsNotNull(credential.BlockedAt);
        }

        [TestMethod]
        public void Block_WhenNotActive_Throws()
        {
            Credential credential = new("EMP-0001", Guid.NewGuid());
            credential.Block();

            Assert.ThrowsExactly<BussinesRuleException>(() => credential.Block());
        }

        [TestMethod]
        public void Revoke_BlockedCredential_IsRevoked()
        {
            Credential credential = new("EMP-0001", Guid.NewGuid());
            credential.Block();

            credential.Revoke();

            Assert.AreEqual(CredentialStatus.Revoked, credential.Status);
            Assert.IsFalse(credential.CanBeUsed);
            Assert.IsNotNull(credential.RevokedAt);
        }

        [TestMethod]
        public void Revoke_WhenAlreadyRevoked_Throws()
        {
            Credential credential = new("EMP-0001", Guid.NewGuid());
            credential.Revoke();

            Assert.ThrowsExactly<BussinesRuleException>(() => credential.Revoke());
        }
    }
}
