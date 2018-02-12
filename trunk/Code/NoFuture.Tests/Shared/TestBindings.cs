using NUnit.Framework;

namespace NoFuture.Tests.Shared
{
    [TestFixture]
    public class TestBindings
    {
        [Test]
        public void TestNetTcpBindings()
        {
            var testInput = TestAssembly.UnitTestsRoot + @"\Shared\NetTcpBindingExample.xml";

            var testResult = NoFuture.Shared.WcfClient.Bindings.GetNetTcpBindings(testInput);

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);
        }

        [Test]
        public void TestCustomBindings()
        {
            var testInput = TestAssembly.UnitTestsRoot + @"\Shared\CustomBindingExample.config";

            var testResults = NoFuture.Shared.WcfClient.Bindings.GetCustomBindings(testInput);

            Assert.IsNotNull(testResults);
            Assert.AreNotEqual(0, testResults.Count);
        }

        [Test]
        public void TestWsHttpBinding()
        {
            var testInput = TestAssembly.UnitTestsRoot + @"\Shared\WsHttpBindingExample.xml";

            var testResult = NoFuture.Shared.WcfClient.Bindings.GetWsHttpBindings(testInput);

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);
        }

        [Test]
        public void TestNetMsmqBinding()
        {
            var testInput = TestAssembly.UnitTestsRoot + @"\Shared\MsmqBindingExample.xml";

            var testResult = NoFuture.Shared.WcfClient.Bindings.GetNetMsmqBindings(testInput);

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);
            
        }
    }
}
