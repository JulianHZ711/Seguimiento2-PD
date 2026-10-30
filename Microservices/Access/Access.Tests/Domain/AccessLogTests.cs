using Access.Domain.Entities.AccessLogs;
using Access.Domain.Entities.AccessLogs.ValueObjects;
using Access.Domain.Entities.Credentials;
using Access.Domain.Exceptions;

namespace Access.Tests.Domain
{
    [TestClass]
    public sealed class AccessLogTests
    {
        [TestMethod]
        public void Authorized_WithActiveCredential_RegistersEmployeeAndCredential()
        {
            Credential credential = new("EMP-0001", Guid.NewGuid());
            Guid accessPointId = Guid.NewGuid();

            AccessLog accessLog = AccessLog.Authorized(credential, accessPointId, MovementType.Entry);

            Assert.AreEqual(AccessResult.Authorized, accessLog.Result);
            Assert.AreEqual(credential.Id, accessLog.CredentialId);
            Assert.AreEqual(credential.EmployeeId, accessLog.EmployeeId);
            Assert.AreEqual(accessPointId, accessLog.AccessPointId);
            Assert.AreEqual(MovementType.Entry, accessLog.MovementType);
            Assert.IsNull(accessLog.RejectionReason);
        }

        [TestMethod]
        public void Authorized_WithBlockedCredential_Throws()
        {
            Credential credential = new("EMP-0001", Guid.NewGuid());
            credential.Block();

            Assert.ThrowsExactly<BussinesRuleException>(() => AccessLog.Authorized(credential, Guid.NewGuid(), MovementType.Entry));
        }

        [TestMethod]
        public void Rejected_WithUnknownCode_KeepsCodeAndReason()
        {
            AccessLog accessLog = AccessLog.Rejected("no-existe-999", Guid.NewGuid(), MovementType.Exit, "La credencial no está registrada.");

            Assert.AreEqual(AccessResult.Rejected, accessLog.Result);
            Assert.AreEqual("NO-EXISTE-999", accessLog.CredentialCode);
            Assert.AreEqual("La credencial no está registrada.", accessLog.RejectionReason);
            Assert.IsNull(accessLog.CredentialId);
            Assert.IsNull(accessLog.EmployeeId);
        }

        [TestMethod]
        [DataRow("")]
        [DataRow("   ")]
        public void Rejected_WithoutReason_Throws(string reason)
        {
            Assert.ThrowsExactly<BussinesRuleException>(() => AccessLog.Rejected("EMP-0001", Guid.NewGuid(), MovementType.Entry, reason));
        }

        [TestMethod]
        public void Rejected_WithoutAccessPoint_Throws()
        {
            Assert.ThrowsExactly<BussinesRuleException>(() => AccessLog.Rejected("EMP-0001", Guid.Empty, MovementType.Entry, "Motivo"));
        }

        [TestMethod]
        public void Rejected_WithInvalidMovementType_Throws()
        {
            Assert.ThrowsExactly<BussinesRuleException>(() => AccessLog.Rejected("EMP-0001", Guid.NewGuid(), (MovementType)9, "Motivo"));
        }
    }
}
