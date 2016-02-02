using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NoFuture.Tests.Host.Encryption
{

    /*
cd C:\Projects\31g\trunk\Code\NoFuture\Host.Encryption\bin\Debug

NoFuture.Host.Encryption.exe -toPlainTextPort=4696 -toCipherTextPort=4697 -hashPort=4698
     */
    [TestClass]
    public class ProgramTests
    {
        private Process _testProgram;
        private ProcessStartInfo _startInfo;
        private Socket _bkToCipherText;
        private Socket _bkToPlainText;
        private Socket _sha256Hash;

        public const int BK_PT_PORT = 4696;
        public const int BK_CT_PORT = 4697;
        public const int HP_PORT = 4698;

        public const string TEST_INPUT = "plain text";

        [TestMethod]
        public void TestsGetCmdLineArgs()
        {

            var argHash = new System.Collections.Hashtable
                              {
                                  {NoFuture.Host.Encryption.SWITCHES.TO_PLAIN_TXT_PORT, BK_PT_PORT},
                                  {NoFuture.Host.Encryption.SWITCHES.TO_CIPHER_TEXT_PORT, BK_CT_PORT},
                                  {NoFuture.Host.Encryption.SWITCHES.HASH_PORT, HP_PORT}
                              };

            var testResult = NoFuture.Host.Encryption.Program.GetHostSjclCmdArgs(argHash);
            Assert.AreEqual(BK_PT_PORT, testResult.SjclBulkKeyToPlainTextPort);
            Assert.AreEqual(BK_CT_PORT, testResult.SjclBulkKeyToCipherTextPort);
            Assert.AreEqual(HP_PORT, testResult.SjclSha256HashPort);

            Assert.IsTrue(testResult.IsValid());
        }

        [TestMethod]
        [Ignore]
        public void TestStartProgram()
        {
            StartTestProgram();
            System.Threading.Thread.Sleep(5000);
            Assert.IsNotNull(_testProgram);
            Assert.IsFalse(_testProgram.HasExited);
            //_testProgram.Close();
            //_testProgram.Dispose();
        }

        [TestMethod]
        public void TestBkToCipherTextOnSocket()
        {
            _bkToCipherText = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var endpt = new IPEndPoint(IPAddress.Loopback, BK_CT_PORT);
            _bkToCipherText.Connect(endpt);

            var bytesSent = _bkToCipherText.Send(Encoding.UTF8.GetBytes(TEST_INPUT));
            var buffer = new List<byte>();
            var data = new byte[256];

            _bkToCipherText.Receive(data, 0, data.Length, SocketFlags.None);
            buffer.AddRange(data.Where(b => b != (byte)'\0'));
            while (_bkToCipherText.Available > 0)
            {
                data = new byte[_bkToCipherText.Available];
                _bkToCipherText.Receive(data, 0, data.Length, SocketFlags.None);
                buffer.AddRange(data.Where(b => b != (byte)'\0'));
            }

            _bkToCipherText.Close();

            var cipherdata = Encoding.UTF8.GetString(buffer.ToArray());

            NoFuture.Shared.CipherText cipherText;
            var parseResult = NoFuture.Shared.CipherText.TryParse(cipherdata, out cipherText);
            Assert.IsTrue(parseResult);
            Console.WriteLine(cipherText.ToString());
        }

        [TestMethod]
        public void TestBkToPlainTextOnSocket()
        {
            var cipherText =
                "{\"adata\":\"\",\"cipher\":\"aes\",\"ct\":\"xQuA9va71VsCAat2u777e8lQ\",\"iter\":1000,\"iv\":\"YnwNxgK1rGEVq93BERFJAQ==\",\"ks\":128,\"mode\":\"ccm\",\"salt\":\"AZvEgYabTxA=\",\"ts\":64,\"v\":1}";
            _bkToPlainText = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _bkToPlainText.ReceiveTimeout = 20000;
            _bkToPlainText.SendTimeout = 20000;
            var endpt = new IPEndPoint(IPAddress.Loopback, BK_PT_PORT);
            _bkToPlainText.Connect(endpt);

            var bytesSent = _bkToPlainText.Send(Encoding.UTF8.GetBytes(cipherText.ToString()));
            var buffer = new List<byte>();
            var data = new byte[256];

            _bkToPlainText.Receive(data, 0, data.Length, SocketFlags.None);
            buffer.AddRange(data.Where(b => b != (byte)'\0'));
            while (_bkToPlainText.Available > 0)
            {
                data = new byte[_bkToPlainText.Available];
                _bkToPlainText.Receive(data, 0, data.Length, SocketFlags.None);
                buffer.AddRange(data.Where(b => b != (byte)'\0'));
            }

            _bkToPlainText.Close();

            var plaintext = Encoding.UTF8.GetString(buffer.ToArray());
            Assert.IsNotNull(plaintext);
            Assert.AreEqual(TEST_INPUT, plaintext);
            Console.WriteLine(plaintext);
        }

        [TestMethod]
        public void TestSha256HashOnSocket()
        {
            _sha256Hash = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _sha256Hash.ReceiveTimeout = 20000;
            _sha256Hash.SendTimeout = 20000;
            var endpt = new IPEndPoint(IPAddress.Loopback, HP_PORT);
            _sha256Hash.Connect(endpt);

            var bytesSent = _sha256Hash.Send(Encoding.UTF8.GetBytes(TEST_INPUT));
            var buffer = new List<byte>();
            var data = new byte[256];

            _sha256Hash.Receive(data, 0, data.Length, SocketFlags.None);
            buffer.AddRange(data.Where(b => b != (byte)'\0'));
            while (_sha256Hash.Available > 0)
            {
                data = new byte[_sha256Hash.Available];
                _sha256Hash.Receive(data, 0, data.Length, SocketFlags.None);
                buffer.AddRange(data.Where(b => b != (byte)'\0'));
            }

            _sha256Hash.Close();

            var testOutput = Encoding.UTF8.GetString(buffer.ToArray());
            var testControl = NoFuture.Encryption.Sjcl.Hash.Sign(TEST_INPUT, "BE8@+U9i+9tu");

            Console.WriteLine("Calculated as '{0}'", testControl);
            Console.WriteLine("From socket as '{0}'",testOutput);

            Assert.AreEqual(testControl,testOutput);
        }

        public void StartTestProgram()
        {
            _startInfo = new ProcessStartInfo
                             {
                                 FileName =
                                     @"C:\Projects\31g\trunk\Code\NoFuture\Host.Encryption\bin\Debug\NoFuture.Host.Encryption.exe",
                                 Arguments =
                                     string.Format(
                                                   "-" + NoFuture.Host.Encryption.SWITCHES.TO_PLAIN_TXT_PORT + "={0} -" +
                                                   NoFuture.Host.Encryption.SWITCHES.TO_CIPHER_TEXT_PORT + "={1} -" +
                                                   NoFuture.Host.Encryption.SWITCHES.HASH_PORT + "={2}",
                                                   BK_PT_PORT,
                                                   BK_CT_PORT,
                                                   HP_PORT)
                             };
            _testProgram = new Process {StartInfo = _startInfo};
            _testProgram.Start();
        }
    }
}
