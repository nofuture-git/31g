using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NoFuture.Tests.Hbm
{
    [TestClass]
    public class TestConfigFile
    {
        [TestMethod]
        public void TestHibernateConfigurationNode()
        {
            var testResult = NoFuture.Hbm.XeFactory.HibernateConfigurationNode("myConnectionString", "NoFuture");
            Assert.IsNotNull(testResult);
            System.Diagnostics.Debug.WriteLine(testResult);
        }
    }
}
