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

NoFuture.Host.Encryption.exe -sjclPkPtPort=4694 -sjclPkCtPort=4695 -sjclBkPtPort=4696 -sjclBkCtPort=4697 -sjclHpPort=4698
     */
    [TestClass]
    public class ProgramTests
    {
        private Process _testProgram;
        private ProcessStartInfo _startInfo;
        private Socket _pkToCipherText;
        private Socket _ctToPlainText;
        private Socket _bkToCipherText;
        private Socket _bkToPlainText;
        private Socket _sha256Hash;

        public const int PK_PT_PORT = 4694;
        public const int PK_CT_PORT = 4695;
        public const int BK_PT_PORT = 4696;
        public const int BK_CT_PORT = 4697;
        public const int HP_PORT = 4698;

        public const string TEST_INPUT = "plain text";

        [TestMethod]
        public void TestsGetCmdLineArgs()
        {

            var argHash = new System.Collections.Hashtable
                              {
                                  {NoFuture.Host.Encryption.SWITCHES.SJCL_PK_PT_PORT, PK_PT_PORT},
                                  {NoFuture.Host.Encryption.SWITCHES.SJCL_PK_CT_PORT, PK_CT_PORT},
                                  {NoFuture.Host.Encryption.SWITCHES.SJCL_BK_PT_PORT, BK_PT_PORT},
                                  {NoFuture.Host.Encryption.SWITCHES.SJCL_BK_CT_PORT, BK_CT_PORT},
                                  {NoFuture.Host.Encryption.SWITCHES.SJCL_HP_PORT, HP_PORT}
                              };

            var testResult = NoFuture.Host.Encryption.Program.GetCmdLineArgs(argHash);
            Assert.AreEqual(PK_PT_PORT, testResult.SjclPublicKeyToPlainTextPort);
            Assert.AreEqual(PK_CT_PORT, testResult.SjclPublicKeyToCipherTextPort);
            Assert.AreEqual(BK_PT_PORT, testResult.SjclBulkKeyToPlainTextPort);
            Assert.AreEqual(BK_CT_PORT, testResult.SjclBulkKeyToCipherTextPort);
            Assert.AreEqual(HP_PORT, testResult.SjclSha256HashPort);

            Assert.IsTrue(testResult.Valid);
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
        public void TestPkToCipherTextOnSocket()
        {
            
            _pkToCipherText = new Socket(AddressFamily.InterNetwork,SocketType.Stream, ProtocolType.Tcp);
            var endpt = new IPEndPoint(IPAddress.Loopback, PK_CT_PORT);
            _pkToCipherText.Connect(endpt);

            var bytesSent = _pkToCipherText.Send(Encoding.UTF8.GetBytes(TEST_INPUT));
            var buffer = new List<byte>();
            var data = new byte[256];

            _pkToCipherText.Receive(data, 0, data.Length, SocketFlags.None);
            buffer.AddRange(data.Where(b => b != (byte)'\0'));
            while(_pkToCipherText.Available > 0)
            {
                data = new byte[_pkToCipherText.Available];
                _pkToCipherText.Receive(data, 0, data.Length, SocketFlags.None);
                buffer.AddRange(data.Where(b => b != (byte)'\0'));
            }

            _pkToCipherText.Close();

            var cipherdata = Encoding.UTF8.GetString(buffer.ToArray());

            NoFuture.Shared.CipherText cipherText;
            var parseResult = NoFuture.Shared.CipherText.TryParse(cipherdata, out cipherText);
            Assert.IsTrue(parseResult);
            Console.WriteLine(cipherText.ToString());

        }

        [TestMethod]
        public void TestPkToPlainTextOnSocketFromString()
        {
            var cipherText =
                "{\"adata\":\"\",\"cipher\":\"aes\",\"ct\":\"Nip8p5zmDOWm2GbmWlU5pKeM\",\"iter\":1000,\"iv\":\"zxOCDe\\/dzsgZqfaRt8YcjA==\",\"ks\":128,\"mode\":\"ccm\",\"salt\":\"YPMKMySa0ZQ=\",\"ts\":64,\"v\":1}";
            NoFuture.Shared.CipherText test_input;
            NoFuture.Shared.CipherText.TryParse(cipherText, out test_input);

            _ctToPlainText = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _ctToPlainText.ReceiveTimeout = 20000;
            _ctToPlainText.SendTimeout = 20000;
            var endpt = new IPEndPoint(IPAddress.Loopback, PK_PT_PORT);
            _ctToPlainText.Connect(endpt);

            var bytesSent = _ctToPlainText.Send(Encoding.UTF8.GetBytes(test_input.ToString()));
            var buffer = new List<byte>();
            var data = new byte[256];

            _ctToPlainText.Receive(data, 0, data.Length, SocketFlags.None);
            buffer.AddRange(data.Where(b => b != (byte)'\0'));
            while (_ctToPlainText.Available > 0)
            {
                data = new byte[_ctToPlainText.Available];
                _ctToPlainText.Receive(data, 0, data.Length, SocketFlags.None);
                buffer.AddRange(data.Where(b => b != (byte)'\0'));
            }

            _ctToPlainText.Close();

            var plaintext = Encoding.UTF8.GetString(buffer.ToArray());
            Assert.IsNotNull(plaintext);
            Assert.AreEqual(TEST_INPUT, plaintext);
            Console.WriteLine(plaintext);

        }

        [TestMethod]
        public void TestPkToPlainTextOnSocketFromCipherTextObject()
        {
            var test_input = new NoFuture.Shared.CipherText
            {
                adata = string.Empty,
                cipher = "aes",
                ct = "praRbuuVATkwyxDMjZhiACQaLupW3Jsde78=",
                iter = 1000,
                iv = "DF4D2nAa6WZT6qYMojq2YQ==",
                ks = 128,
                mode = "ccm",
                salt = @"M9egkIg2Y\/Q=",
                ts = 64,
                v = 1
            };
            _ctToPlainText = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _ctToPlainText.ReceiveTimeout = 20000;
            _ctToPlainText.SendTimeout = 20000;
            var endpt = new IPEndPoint(IPAddress.Loopback, PK_PT_PORT);
            _ctToPlainText.Connect(endpt);

            var bytesSent = _ctToPlainText.Send(Encoding.UTF8.GetBytes(test_input.ToString()));
            var buffer = new List<byte>();
            var data = new byte[256];

            _ctToPlainText.Receive(data, 0, data.Length, SocketFlags.None);
            buffer.AddRange(data.Where(b => b != (byte)'\0'));
            while (_ctToPlainText.Available > 0)
            {
                data = new byte[_ctToPlainText.Available];
                _ctToPlainText.Receive(data, 0, data.Length, SocketFlags.None);
                buffer.AddRange(data.Where(b => b != (byte)'\0'));
            }

            _ctToPlainText.Close();

            var plaintext = Encoding.UTF8.GetString(buffer.ToArray());
            Assert.IsNotNull(plaintext);
            Assert.AreEqual("This is plain text", plaintext);
            Console.WriteLine(plaintext);
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
                                                   "-" + NoFuture.Host.Encryption.SWITCHES.SJCL_PK_PT_PORT + "={0} -" +
                                                   NoFuture.Host.Encryption.SWITCHES.SJCL_PK_CT_PORT + "={1} -" +
                                                   NoFuture.Host.Encryption.SWITCHES.SJCL_BK_PT_PORT + "={2} -" +
                                                   NoFuture.Host.Encryption.SWITCHES.SJCL_BK_CT_PORT + "={3} -" +
                                                   NoFuture.Host.Encryption.SWITCHES.SJCL_HP_PORT + "={4}",
                                                   PK_PT_PORT,
                                                   PK_CT_PORT,
                                                   BK_PT_PORT,
                                                   BK_CT_PORT,
                                                   HP_PORT)
                             };
            _testProgram = new Process {StartInfo = _startInfo};
            _testProgram.Start();
        }
    }
}
