using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NoFuture.Tests.Shared
{
    [TestClass]
    public class TestBindings
    {
        [TestMethod]
        public void TestNetTcpBindings()
        {
            var testInput = @"C:\Projects\31g\trunk\Code\NoFuture.Tests\Shared\NetTcpBindingExample.xml";

            var testResult = NoFuture.Shared.WcfClient.Bindings.GetNetTcpBindings(testInput);

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);
        }

        [TestMethod]
        public void TestWsHttpBinding()
        {
            var testInput = @"C:\Projects\31g\trunk\Code\NoFuture.Tests\Shared\WsHttpBindingExample.xml";

            var testResult = NoFuture.Shared.WcfClient.Bindings.GetWsHttpBindings(testInput);

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);
        }

        [TestMethod]
        public void TestNetMsmqBinding()
        {
            var testInput = @"C:\Projects\31g\trunk\Code\NoFuture.Tests\Shared\MsmqBindingExample.xml";

            var testResult = NoFuture.Shared.WcfClient.Bindings.GetNetMsmqBindings(testInput);

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);
            
        }
    }
}
