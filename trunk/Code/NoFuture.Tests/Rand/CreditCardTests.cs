using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Rand;
using NoFuture.Rand.Data.Sp;
using NoFuture.Rand.Domus;

namespace NoFuture.Tests.Rand
{
    [TestClass]
    public class CreditCardTests
    {
        [TestInitialize]
        public void Init()
        {
            BinDirectories.DataRoot = @"C:\Projects\31g\trunk\bin\Data\Source";
        }
        [TestMethod]
        public void TestCreditCardNumber()
        {
            var testInput = new List<Rchar>();
            testInput.Add(new LimitedRchar(0, 'U'));
            testInput.AddRange(Etx.GetRandomRChars(true, 8, 1));
            var testSubject = new CreditCardNumber(testInput.ToArray())
            {
                CheckDigitFunc = NoFuture.Util.Etc.CalcLuhnCheckDigit
            };

            var testResult00 = testSubject.Value;
            Assert.IsNotNull(testResult00);

            var testResult02 = testSubject.Value;
            Assert.AreEqual(testResult00, testResult02);

            var testResult10 = testSubject.Validate(testResult00);
            Assert.IsTrue(testResult10);
        }

        [TestMethod]
        public void TestCreditCardCtor()
        {
            var testInput = new NorthAmerican(NoFuture.Rand.Domus.NAmerUtil.GetWorkingAdultBirthDate(), Gender.Female);
            var testSubject = new VisaCc(testInput, DateTime.Today, 0.0F);

            Assert.IsNotNull(testSubject.Number);
            Assert.IsFalse(string.IsNullOrWhiteSpace(testSubject.Number.Value));
            Assert.IsFalse(string.IsNullOrWhiteSpace(testSubject.CardHolderName));
            Assert.IsFalse(string.IsNullOrWhiteSpace(testSubject.Cvv));

            System.Diagnostics.Debug.WriteLine(testSubject.ToString());
            

        }
    }
}
