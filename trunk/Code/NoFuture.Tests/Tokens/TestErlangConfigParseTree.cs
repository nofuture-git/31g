using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Antlr.ErlangConfig;

namespace NoFuture.Tests.Tokens
{
    [TestClass]
    public class TestErlangConfigParseTree
    {
        public string ErlangTestConfigFile = TestAssembly.UnitTestsRoot + @"\ExampleDlls\rabbitmq.config";

        [TestMethod]
        public void TestInvokeParse()
        {
            var testResults = ErlangConfigParseTree.InvokeParse(ErlangTestConfigFile);

            Assert.IsNotNull(testResults);

            Assert.IsNotNull(testResults.Rabbit);
            Assert.IsNotNull(testResults.MgmtRabbit);
            Assert.IsNotNull(testResults.Tracing);
            Assert.IsNotNull(testResults.Ssl);

            Assert.IsNotNull(testResults.Rabbit.SslOptionsConfig);
            Assert.IsNotNull(testResults.MgmtRabbit.SslOptionsConfig);

            Assert.IsNotNull(testResults.Ssl.Versions);
            Assert.AreNotEqual(0, testResults.Ssl.Versions.Length);
            Assert.IsNotNull(testResults.Ssl.Versions.FirstOrDefault(x => x == "'tlsv1.2'"));
            Assert.IsNotNull(testResults.Ssl.Versions.FirstOrDefault(x => x == "'tlsv1.1'"));
            Assert.IsNotNull(testResults.Ssl.Versions.FirstOrDefault(x => x == "tlsv1"));

            Assert.IsNotNull(testResults.Rabbit.SslListeners);
            Assert.AreNotEqual(0, testResults.Rabbit.SslListeners.Length);

            Assert.IsNotNull(testResults.Rabbit.SslListeners.FirstOrDefault(x => x == 5671));

            Assert.IsNotNull(testResults.Rabbit.SslOptionsConfig.CaCertFile);
            Assert.AreEqual("\"C://Rabbit/RabbitMQ Server/Certificates/cacert.pem\"", testResults.Rabbit.SslOptionsConfig.CaCertFile);
            Assert.IsNotNull(testResults.Rabbit.SslOptionsConfig.CertFile);
            Assert.AreEqual("\"C://Rabbit/RabbitMQ Server/Certificates/mq-qa-cert.pem\"", testResults.Rabbit.SslOptionsConfig.CertFile);
            Assert.IsNotNull(testResults.Rabbit.SslOptionsConfig.KeyFile);
            Assert.AreEqual("\"C://Rabbit/RabbitMQ Server/Certificates/mq-qa-key.pem\"", testResults.Rabbit.SslOptionsConfig.KeyFile);

            Assert.IsNotNull(testResults.Rabbit.SslOptionsConfig.SslVersions);
            Assert.IsNotNull(testResults.Rabbit.SslOptionsConfig.SslVersions.Versions);
            Assert.AreNotEqual(0, testResults.Rabbit.SslOptionsConfig.SslVersions.Versions.Length);
            Assert.IsNotNull(testResults.Rabbit.SslOptionsConfig.SslVersions.Versions.FirstOrDefault(x => x == "'tlsv1.5 bonus'"));
            Assert.IsNotNull(testResults.Rabbit.SslOptionsConfig.SslVersions.Versions.FirstOrDefault(x => x == "'tlsv1.4++'"));

            Assert.IsNotNull(testResults.Rabbit.SslOptionsConfig.Verify);
            Assert.AreEqual("verify_peer", testResults.Rabbit.SslOptionsConfig.Verify);
            Assert.IsTrue(testResults.Rabbit.SslOptionsConfig.FailIfNoPeerCert);

            Assert.IsNotNull(testResults.Rabbit.AuthMechanisms);
            Assert.AreNotEqual(0, testResults.Rabbit.AuthMechanisms.Length);
            Assert.IsNotNull(testResults.Rabbit.AuthMechanisms.FirstOrDefault(x => x == "'EXTERNAL'"));

            Assert.IsNotNull(testResults.Rabbit.SslCertLoginFrom);
            Assert.AreEqual("common_name", testResults.Rabbit.SslCertLoginFrom);

            Assert.AreEqual(15671, testResults.MgmtRabbit.Port);
            Assert.IsNotNull(testResults.MgmtRabbit.Ip);
            Assert.AreEqual("\"0.0.0.0\"", testResults.MgmtRabbit.Ip);
            Assert.IsTrue(testResults.MgmtRabbit.EnableSsl);

            Assert.IsNotNull(testResults.MgmtRabbit.SslOptionsConfig.CaCertFile);
            Assert.AreEqual("\"C://RabbitMgmt/RabbitMQ Server/Certificates/cacert.pem\"", testResults.MgmtRabbit.SslOptionsConfig.CaCertFile);
            Assert.IsNotNull(testResults.MgmtRabbit.SslOptionsConfig.CertFile);
            Assert.AreEqual("\"C://RabbitMgmt/RabbitMQ Server/Certificates/mq-qa-cert.pem\"", testResults.MgmtRabbit.SslOptionsConfig.CertFile);
            Assert.IsNotNull(testResults.MgmtRabbit.SslOptionsConfig.KeyFile);
            Assert.AreEqual("\"C://RabbitMgmt/RabbitMQ Server/Certificates/mq-qa-key.pem\"", testResults.MgmtRabbit.SslOptionsConfig.KeyFile);

            Assert.IsNull(testResults.MgmtRabbit.SslOptionsConfig.SslVersions);

            Assert.IsNotNull(testResults.MgmtRabbit.Global);
            Assert.AreNotEqual(0,testResults.MgmtRabbit.Global.PolicyValues.Length);
            Assert.IsNotNull(testResults.MgmtRabbit.Global.PolicyValues.FirstOrDefault(x => x.Equals(new Tuple<int,int>(60,5))));
            foreach (var f in testResults.MgmtRabbit.Global.PolicyValues)
            {
                System.Diagnostics.Debug.WriteLine(f);
            }

            Assert.IsNotNull(testResults.MgmtRabbit.Basic);
            Assert.AreNotEqual(0, testResults.MgmtRabbit.Basic.PolicyValues.Length);
            foreach (var f in testResults.MgmtRabbit.Basic.PolicyValues)
            {
                System.Diagnostics.Debug.WriteLine(f);
            }
            Assert.IsNotNull(testResults.MgmtRabbit.Detailed);
            Assert.AreNotEqual(0, testResults.MgmtRabbit.Detailed.PolicyValues.Length);
            foreach (var f in testResults.MgmtRabbit.Detailed.PolicyValues)
            {
                System.Diagnostics.Debug.WriteLine(f);
            }

            Assert.IsNotNull(testResults.Tracing.Directory);
            Assert.AreEqual("\"P:/Docs/LogFiles/PROD/RabbitMqTrace\"", testResults.Tracing.Directory);


        }
    }
}
