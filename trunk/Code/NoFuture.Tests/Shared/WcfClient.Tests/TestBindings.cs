using System;
using System.IO;
using System.Reflection;
using NUnit.Framework;

namespace NoFuture.Shared.WcfClient.Tests
{
    [TestFixture]
    public class TestBindings
    {
        public string PutTestFileOnDisk(string embeddedFileName)
        {
            //need this to be another object each time and not just another reference
            var asmName = Assembly.GetExecutingAssembly().GetName().Name;
            var liSteam = Assembly.GetExecutingAssembly().GetManifestResourceStream($"{asmName}.{embeddedFileName}");
            if (liSteam == null)
            {
                Assert.Fail($"Cannot find the embedded file {embeddedFileName}");
            }

            string fileContent = null;
            using (var txtSr = new StreamReader(liSteam))
            {
                fileContent = txtSr.ReadToEnd();
            }
            Assert.IsNotNull(fileContent);

            var nfAppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            if (String.IsNullOrWhiteSpace(nfAppData) || !Directory.Exists(nfAppData))
                throw new DirectoryNotFoundException("The Environment.GetFolderPath for " +
                                                     "SpecialFolder.ApplicationData returned a bad path.");
            nfAppData = Path.Combine(nfAppData, "NoFuture.Tests");
            if (!Directory.Exists(nfAppData))
            {
                Directory.CreateDirectory(nfAppData);
            }

            var fileOnDisk = Path.Combine(nfAppData, embeddedFileName);
            File.WriteAllText(fileOnDisk, fileContent);
            System.Threading.Thread.Sleep(50);
            return fileOnDisk;
        }

        [Test]
        public void TestNetTcpBindings()
        {
            var testInput = PutTestFileOnDisk(@"NetTcpBindingExample.xml");

            var testResult = NoFuture.Shared.WcfClient.Bindings.GetNetTcpBindings(testInput);

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);
        }

        [Test]
        public void TestCustomBindings()
        {
            var testInput = PutTestFileOnDisk(@"CustomBindingExample.config");

            var testResults = NoFuture.Shared.WcfClient.Bindings.GetCustomBindings(testInput);

            Assert.IsNotNull(testResults);
            Assert.AreNotEqual(0, testResults.Count);
        }

        [Test]
        public void TestWsHttpBinding()
        {
            var testInput = PutTestFileOnDisk(@"WsHttpBindingExample.xml");

            var testResult = NoFuture.Shared.WcfClient.Bindings.GetWsHttpBindings(testInput);

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);
        }

        [Test]
        public void TestNetMsmqBinding()
        {
            var testInput = PutTestFileOnDisk(@"MsmqBindingExample.xml");

            var testResult = NoFuture.Shared.WcfClient.Bindings.GetNetMsmqBindings(testInput);

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);
            
        }
    }
}
