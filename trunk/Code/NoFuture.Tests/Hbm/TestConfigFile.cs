using System;
using NUnit.Framework;

namespace NoFuture.Tests.Hbm
{
    [TestFixture]
    public class TestConfigFile
    {
        [Test]
        public void TestHibernateConfigurationNode()
        {
            var testResult = NoFuture.Hbm.XeFactory.HibernateConfigurationNode("myConnectionString", "NoFuture");
            Assert.IsNotNull(testResult);
            Console.WriteLine(testResult);
        }
    }
}
