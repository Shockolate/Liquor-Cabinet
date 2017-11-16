using LiquorCabinet.Settings;
using NUnit.Framework;

namespace LiquorCabinet.UnitTests
{
    [TestFixture]
    public class DtosTests
    {
        [Test]
        public void SettingsTouch()
        {
            Assert.DoesNotThrow(() =>
            {
                var settings = new Settings.Settings
                {
                    Database = new DatabaseSettings {Server = "DataSource", Database = "InitialCatalog", Password = "Password", UserId = "UserId"}
                };
                Assert.That(settings, Is.Not.Null);
            });
        }
    }
}