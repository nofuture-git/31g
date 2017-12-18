using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Data.Endo;
using NoFuture.Rand.Data.Endo.Enums;
using NoFuture.Rand.Data.Sp;
using NoFuture.Rand.Data.Sp.Cc;
using NoFuture.Rand.Domus;
using NoFuture.Util.Core;

namespace NoFuture.Rand.Tests
{
    [TestClass]
    public class CreditCardTests
    {
        [TestMethod]
        public void TestCreditCardNumber()
        {
            var testInput = new List<Rchar>();
            testInput.Add(new RcharLimited(0, 'U'));
            testInput.AddRange(Etx.GetRandomRChars(true, 8, 1));
            var testSubject = new CreditCardNumber(testInput.ToArray())
            {
                CheckDigitFunc = Etc.CalcLuhnCheckDigit
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
            var testInput = new NorthAmerican(AmericanUtil.GetWorkingAdultBirthDate(), Gender.Female);
            var testSubject = new VisaCc(testInput, DateTime.Today, null);

            Assert.IsNotNull(testSubject.Number);
            Assert.IsFalse(string.IsNullOrWhiteSpace(testSubject.Number.Value));
            Assert.IsFalse(string.IsNullOrWhiteSpace(testSubject.CardHolderName));
            Assert.IsFalse(string.IsNullOrWhiteSpace(testSubject.Cvv));

            System.Diagnostics.Debug.WriteLine(testSubject.ToString());
        }

        [TestMethod]
        public void TestMakePayment()
        {
            var testInput = new NorthAmerican(AmericanUtil.GetWorkingAdultBirthDate(), Gender.Female);
            var testSubject = new CreditCardAccount(new VisaCc(testInput, DateTime.Today.AddDays(-15), null), CreditCardAccount.DF_MIN_PMT_RATE, new Pecuniam(1800.0M));
            Assert.IsTrue(testSubject.Max == new Pecuniam(1800.0M));

        }

        [TestMethod]
        public void TestGetMinPayment()
        {
            var testInput = new NorthAmerican(AmericanUtil.GetWorkingAdultBirthDate(), Gender.Female);
            var testSubject = new CreditCardAccount(new VisaCc(testInput, new DateTime(2014,1,11), null), CreditCardAccount.DF_MIN_PMT_RATE, new Pecuniam(1800.0M));

            testSubject.Pop(new DateTime(2014, 1, 11), new Pecuniam(63.32M));
            testSubject.Pop(new DateTime(2014, 1, 11), new Pecuniam(7.54M));
            testSubject.Pop(new DateTime(2014, 1, 12), new Pecuniam(139.47M));
            testSubject.Pop(new DateTime(2014, 1, 12), new Pecuniam(2.38M));
            testSubject.Pop(new DateTime(2014, 1, 14), new Pecuniam(57.89M));
            testSubject.Pop(new DateTime(2014, 1, 14), new Pecuniam(10.09M));
            testSubject.Pop(new DateTime(2014, 1, 15), new Pecuniam(7.78M));
            testSubject.Pop(new DateTime(2014, 1, 15), new Pecuniam(52.13M));
            testSubject.Pop(new DateTime(2014, 1, 15), new Pecuniam(22.95M));
            testSubject.Pop(new DateTime(2014, 1, 16), new Pecuniam(47.59M));
            testSubject.Pop(new DateTime(2014, 1, 17), new Pecuniam(703.65M));
            testSubject.Pop(new DateTime(2014, 1, 17), new Pecuniam(32.11M));
            testSubject.Pop(new DateTime(2014, 1, 17), new Pecuniam(12.83M));
            testSubject.Pop(new DateTime(2014, 1, 18), new Pecuniam(60.83M));
            testSubject.Pop(new DateTime(2014, 1, 20), new Pecuniam(57.64M));
            testSubject.Pop(new DateTime(2014, 1, 20), new Pecuniam(49.07M));
            testSubject.Pop(new DateTime(2014, 1, 21), new Pecuniam(3.55M));
            testSubject.Pop(new DateTime(2014, 1, 24), new Pecuniam(6.94M));
            testSubject.Pop(new DateTime(2014, 1, 25), new Pecuniam(6.94M));
            testSubject.Pop(new DateTime(2014, 1, 27), new Pecuniam(10.61M));
            testSubject.Pop(new DateTime(2014, 1, 27), new Pecuniam(50.73M));
            testSubject.Pop(new DateTime(2014, 1, 28), new Pecuniam(8.32M));

            var testResult = testSubject.GetMinPayment(new DateTime(2014, 1, 28));

            Assert.AreEqual(-17.58M, testResult.Amount);
            System.Diagnostics.Debug.WriteLine(testResult);
            System.Diagnostics.Debug.WriteLine(testSubject.GetValueAt(new DateTime(2014, 1, 28)));

            testSubject.Pop(new DateTime(2014, 1, 30), new Pecuniam(61.28M));
            testSubject.Pop(new DateTime(2014, 2, 1), new Pecuniam(23.11M));
            testSubject.Pop(new DateTime(2014, 2, 2), new Pecuniam(9.83M));
            testSubject.Pop(new DateTime(2014, 2, 3), new Pecuniam(8.53M));
            testSubject.Pop(new DateTime(2014, 2, 3), new Pecuniam(2.09M));
            testSubject.Pop(new DateTime(2014, 2, 4), new Pecuniam(7.79M));
            testSubject.Pop(new DateTime(2014, 2, 6), new Pecuniam(47.24M));
            testSubject.Pop(new DateTime(2014, 2, 7), new Pecuniam(55.95M));
            testSubject.Pop(new DateTime(2014, 2, 7), new Pecuniam(30.1M));
            testSubject.Pop(new DateTime(2014, 2, 8), new Pecuniam(37.39M));
            testSubject.Pop(new DateTime(2014, 2, 10), new Pecuniam(3.91M));



        }
    }
}
