using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Rand.Data.Exo.NfText;

namespace NoFuture.Tests.Rand.NfTextTests
{
    [TestClass]
    public class TestFedLrgBnk
    {
        [TestMethod]
        public void TestSplitLrgBnkListLine()
        {
            var testInput = @" PROSPERITY BK                  55      664756     EL CAMPO, TX           SNM    21,674   21,674 100   82     265   0   N    0";
            var testOutput = FedLrgBnk.SplitLrgBnkListLine(testInput);
            Assert.IsNotNull(testOutput);
            Assert.AreNotEqual(0, testOutput.Count);
            Assert.AreEqual("EL CAMPO, TX", testOutput[3]);
            foreach (var l in testOutput)
            {
                System.Diagnostics.Debug.WriteLine(l);
            }
        }
    }
}
