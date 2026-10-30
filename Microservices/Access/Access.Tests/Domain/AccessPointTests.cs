using Access.Domain.Entities.AccessPoints;
using Access.Domain.Entities.AccessPoints.ValueObjects;
using Access.Domain.Exceptions;

namespace Access.Tests.Domain
{
    [TestClass]
    public sealed class AccessPointTests
    {
        [TestMethod]
        public void Constructor_IsEnabledByDefault()
        {
            AccessPoint accessPoint = new("  Recepción principal ", AccessPointType.Reception);

            Assert.AreEqual("Recepción principal", accessPoint.Name);
            Assert.IsTrue(accessPoint.IsEnabled);
        }

        [TestMethod]
        public void Constructor_CanBeCreatedDisabled()
        {
            AccessPoint accessPoint = new("Bodega de alto valor", AccessPointType.RestrictedArea, isEnabled: false);

            Assert.IsFalse(accessPoint.IsEnabled);
        }

        [TestMethod]
        [DataRow("")]
        [DataRow("AB")]
        public void Constructor_WithInvalidName_Throws(string name)
        {
            Assert.ThrowsExactly<BussinesRuleException>(() => new AccessPoint(name, AccessPointType.Parking));
        }

        [TestMethod]
        public void Constructor_WithInvalidType_Throws()
        {
            Assert.ThrowsExactly<BussinesRuleException>(() => new AccessPoint("Parqueadero", (AccessPointType)99));
        }
    }
}
