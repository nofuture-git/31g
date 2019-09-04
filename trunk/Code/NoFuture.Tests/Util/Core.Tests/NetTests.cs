using System;
using NoFuture.Util.Core;
using NUnit.Framework;

namespace NoFuture.Tests.Util
{
    [TestFixture]
    public class NetTests
    {
        [Test]
        public void TestGetProxyAuthHeaderValue()
        {
            var username = "Aladdin";
            var pwd = "open sesame";

            var testResult = NfNet.GetAuthHeaderValue(username, pwd);
            Assert.AreEqual("Basic QWxhZGRpbjpvcGVuIHNlc2FtZQ==", testResult);

            testResult = NfNet.GetAuthHeaderValue(null, pwd);
            Assert.IsNotNull(testResult);

        }

        [Test]
        public void TestGetNetStatUri()
        {
            var testResult = NfNet.GetNetStatIp("23.235.44.133:443 ");
            Assert.IsNotNull(testResult);
            Assert.IsFalse(testResult.Equals(System.Net.IPAddress.Loopback));
            Assert.AreEqual("23.235.44.133", testResult.ToString());
            Console.WriteLine(testResult.ToString());


            testResult = NfNet.GetNetStatIp("[::]:0            ");
            Assert.IsNotNull(testResult);
            Assert.IsTrue(testResult.Equals(System.Net.IPAddress.IPv6Loopback));

            testResult = NfNet.GetNetStatIp("*:*               ");
            Assert.IsNotNull(testResult);
            Assert.IsTrue(testResult.Equals(System.Net.IPAddress.Loopback));

            testResult = NfNet.GetNetStatIp("0.0.0.0:0         ");
            Assert.IsNotNull(testResult);
            Assert.IsTrue(testResult.Equals(System.Net.IPAddress.Loopback));

            testResult = NfNet.GetNetStatIp("[::1]:1900        ");
            Assert.IsNotNull(testResult);
            Assert.IsTrue(testResult.Equals(System.Net.IPAddress.IPv6Loopback));

            testResult = NfNet.GetNetStatIp("0.0.0.0:57077     ");
            Assert.IsNotNull(testResult);
            Assert.IsTrue(testResult.Equals(System.Net.IPAddress.Loopback));

            testResult = NfNet.GetNetStatIp("127.0.0.1:62522   ");
            Assert.IsNotNull(testResult);
            Assert.IsTrue(testResult.Equals(System.Net.IPAddress.Loopback));

        }

        [Test]
        public void TestGetNetStatServiceByPort()
        {
            var testResult = NfNet.GetNetStatServiceByPort("tcp", "127.0.0.1:563");
            Assert.AreEqual("nntps",testResult.Item1);

            testResult = NfNet.GetNetStatServiceByPort("tcp", "127.0.0.1:563          ");
            Assert.AreEqual("nntps", testResult.Item1);

            testResult = NfNet.GetNetStatServiceByPort("tcp", "*:*");
            Assert.AreEqual(string.Empty, testResult.Item1);

            testResult = NfNet.GetNetStatServiceByPort("udp", "[::1]:1900        ");
            Assert.AreEqual("ssdp",testResult.Item1);

            testResult = NfNet.GetNetStatServiceByPort("udp", "[::]:0            ");
            Assert.AreEqual(string.Empty, testResult.Item1);

            testResult = NfNet.GetNetStatServiceByPort("udp", "[::]:3540        ");
            Assert.AreEqual("pnrp-port", testResult.Item1);
            Assert.AreEqual("PNRP User Port", testResult.Item2);
        }
    }
}
