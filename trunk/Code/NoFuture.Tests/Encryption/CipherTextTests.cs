using System;
using System.Text;
using NoFuture.Shared;
using NUnit.Framework;

namespace NoFuture.Encryption.Tests
{
    [TestFixture]
    public class CipherTextTests
    {
        [Test]
        public void TestToString()
        {
            var testSubject = new CipherText
                                  {
                                      adata = "atada",
                                      cipher = "rehpic",
                                      ct = "tc",
                                      iter = 1000,
                                      iv = "vi",
                                      ks = 1000,
                                      mode = "edom",
                                      v = 1,
                                      salt = "tlas",
                                      ts =1
                                  };
            var testResult = testSubject.ToString();
            Assert.IsNotNull(testResult);
            Assert.IsFalse(String.IsNullOrWhiteSpace(testResult));

            Assert.IsTrue(testResult.Contains(string.Format("\"adata\":\"{0}\"", testSubject.adata)));
            Assert.IsTrue(testResult.Contains(string.Format("\"cipher\":\"{0}\"", testSubject.cipher)));
            Assert.IsTrue(testResult.Contains(string.Format("\"ct\":\"{0}\"", testSubject.ct)));
            Assert.IsTrue(testResult.Contains(string.Format("\"iter\":{0}", testSubject.iter)));
            Assert.IsTrue(testResult.Contains(string.Format("\"iv\":\"{0}\"", testSubject.iv)));
            Assert.IsTrue(testResult.Contains(string.Format("\"ks\":{0}", testSubject.ks)));
            Assert.IsTrue(testResult.Contains(string.Format("\"mode\":\"{0}\"", testSubject.mode)));
            Assert.IsTrue(testResult.Contains(string.Format("\"v\":{0}", testSubject.v)));
            Assert.IsTrue(testResult.Contains(string.Format("\"salt\":\"{0}\"", testSubject.salt)));
            Assert.IsTrue(testResult.Contains(string.Format("\"ts\":{0}", testSubject.ts)));
        }

        [Test]
        public void TestTryParse()
        {
            var testValues = new CipherText
            {
                adata = "atada",
                cipher = "rehpic",
                ct = "tc",
                iter = 1000,
                iv = "vi",
                ks = 1000,
                mode = "edom",
                v = 1,
                salt = "tlas",
                ts = 1
            };
            var toStringBuilder = new StringBuilder();
            toStringBuilder.Append("{");
            toStringBuilder.Append(string.Format("\"adata\":\"{0}\",", testValues.adata));
            toStringBuilder.Append(string.Format("\"cipher\":\"{0}\",", testValues.cipher));
            toStringBuilder.Append(string.Format("\"iter\":{0},", testValues.iter));
            toStringBuilder.Append(string.Format("\"iv\":\"{0}\",", testValues.iv));
            toStringBuilder.Append(string.Format("\"ks\":{0},", testValues.ks));
            toStringBuilder.Append(string.Format("\"mode\":\"{0}\",", testValues.mode));
            toStringBuilder.Append(string.Format("\"v\":{0},", testValues.v));
            toStringBuilder.Append(string.Format("\"salt\":\"{0}\",", testValues.salt));
            toStringBuilder.Append(string.Format("\"ts\":{0},", testValues.ts));
            toStringBuilder.Append(string.Format("\"ct\":\"{0}\"", testValues.ct));
            toStringBuilder.Append("}");

           CipherText testSubject;
            var testResult =CipherText.TryParse(toStringBuilder.ToString(), out testSubject);
            Assert.IsTrue(testResult);
        }

        [Test]
        public void ToggleStringJson()
        {
            var testValues = new CipherText
            {
                adata = "atada",
                cipher = "rehpic",
                ct = "tc",
                iter = 1000,
                iv = "vi",
                ks = 1000,
                mode = "edom",
                v = 1,
                salt = "tlas",
                ts = 1
            };
            CipherText testSubject;
            var testResultTryParse = CipherText.TryParse(testValues.ToString(), out testSubject);
            Assert.IsTrue(testResultTryParse);

        }
    }
}
