using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using NoFuture.Rand.Core;
using NoFuture.Rand.Domus.US;
using NoFuture.Rand.Gov;
using NoFuture.Rand.Sp;
using NoFuture.Rand.Sp.Cc;
using NoFuture.Util.Core;
using NUnit.Framework;

namespace NoFuture.Rand.Tests.SpTests
{
    [TestFixture]
    public class CreditCardTests
    {
        [Test]
        public void TestCreditCardNumber()
        {
            var testInput = new List<Rchar>();
            testInput.Add(new RcharLimited(0, 'U'));
            testInput.AddRange(Etx.RandomRChars(true, 8, 1));
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

        [Test]
        public void TestCreditCardCtor()
        {
            var testInput = American.RandomAmerican(Etx.RandomAdultBirthDate(), Gender.Female);
            var testSubject = new VisaCc(testInput, DateTime.Today, null);

            Assert.IsNotNull(testSubject.Number);
            Assert.IsFalse(string.IsNullOrWhiteSpace(testSubject.Number.Value));
            Assert.IsFalse(string.IsNullOrWhiteSpace(testSubject.CardHolderName));
            Assert.IsFalse(string.IsNullOrWhiteSpace(testSubject.Cvv));

            System.Diagnostics.Debug.WriteLine(testSubject.ToString());
        }

        [Test]
        public void TestMakePayment()
        {
            var testInput = American.RandomAmerican(Etx.RandomAdultBirthDate(), Gender.Female);
            var testSubject = new CreditCardAccount(new VisaCc(testInput, DateTime.Today.AddDays(-15), null), CreditCardAccount.DF_MIN_PMT_RATE, new Pecuniam(1800.0M));
            Assert.IsTrue(testSubject.Max == new Pecuniam(1800.0M));

        }

        [Test]
        public void TestGetMinPayment()
        {
            var testInput = American.RandomAmerican(Etx.RandomAdultBirthDate(), Gender.Female);
            var testSubject = new CreditCardAccount(new VisaCc(testInput, new DateTime(2014,1,11), null), CreditCardAccount.DF_MIN_PMT_RATE, new Pecuniam(1800.0M));

            testSubject.AddPositiveValue(new DateTime(2014, 1, 11), new Pecuniam(63.32M));
            testSubject.AddPositiveValue(new DateTime(2014, 1, 11), new Pecuniam(7.54M));
            testSubject.AddPositiveValue(new DateTime(2014, 1, 12), new Pecuniam(139.47M));
            testSubject.AddPositiveValue(new DateTime(2014, 1, 12), new Pecuniam(2.38M));
            testSubject.AddPositiveValue(new DateTime(2014, 1, 14), new Pecuniam(57.89M));
            testSubject.AddPositiveValue(new DateTime(2014, 1, 14), new Pecuniam(10.09M));
            testSubject.AddPositiveValue(new DateTime(2014, 1, 15), new Pecuniam(7.78M));
            testSubject.AddPositiveValue(new DateTime(2014, 1, 15), new Pecuniam(52.13M));
            testSubject.AddPositiveValue(new DateTime(2014, 1, 15), new Pecuniam(22.95M));
            testSubject.AddPositiveValue(new DateTime(2014, 1, 16), new Pecuniam(47.59M));
            testSubject.AddPositiveValue(new DateTime(2014, 1, 17), new Pecuniam(703.65M));
            testSubject.AddPositiveValue(new DateTime(2014, 1, 17), new Pecuniam(32.11M));
            testSubject.AddPositiveValue(new DateTime(2014, 1, 17), new Pecuniam(12.83M));
            testSubject.AddPositiveValue(new DateTime(2014, 1, 18), new Pecuniam(60.83M));
            testSubject.AddPositiveValue(new DateTime(2014, 1, 20), new Pecuniam(57.64M));
            testSubject.AddPositiveValue(new DateTime(2014, 1, 20), new Pecuniam(49.07M));
            testSubject.AddPositiveValue(new DateTime(2014, 1, 21), new Pecuniam(3.55M));
            testSubject.AddPositiveValue(new DateTime(2014, 1, 24), new Pecuniam(6.94M));
            testSubject.AddPositiveValue(new DateTime(2014, 1, 25), new Pecuniam(6.94M));
            testSubject.AddPositiveValue(new DateTime(2014, 1, 27), new Pecuniam(10.61M));
            testSubject.AddPositiveValue(new DateTime(2014, 1, 27), new Pecuniam(50.73M));
            testSubject.AddPositiveValue(new DateTime(2014, 1, 28), new Pecuniam(8.32M));

            var testResult = testSubject.GetMinPayment(new DateTime(2014, 1, 28));

            Assert.AreEqual(-17.58M, testResult.Amount);
            System.Diagnostics.Debug.WriteLine(testResult);
            System.Diagnostics.Debug.WriteLine(testSubject.GetValueAt(new DateTime(2014, 1, 28)));

            testSubject.AddPositiveValue(new DateTime(2014, 1, 30), new Pecuniam(61.28M));
            testSubject.AddPositiveValue(new DateTime(2014, 2, 1), new Pecuniam(23.11M));
            testSubject.AddPositiveValue(new DateTime(2014, 2, 2), new Pecuniam(9.83M));
            testSubject.AddPositiveValue(new DateTime(2014, 2, 3), new Pecuniam(8.53M));
            testSubject.AddPositiveValue(new DateTime(2014, 2, 3), new Pecuniam(2.09M));
            testSubject.AddPositiveValue(new DateTime(2014, 2, 4), new Pecuniam(7.79M));
            testSubject.AddPositiveValue(new DateTime(2014, 2, 6), new Pecuniam(47.24M));
            testSubject.AddPositiveValue(new DateTime(2014, 2, 7), new Pecuniam(55.95M));
            testSubject.AddPositiveValue(new DateTime(2014, 2, 7), new Pecuniam(30.1M));
            testSubject.AddPositiveValue(new DateTime(2014, 2, 8), new Pecuniam(37.39M));
            testSubject.AddPositiveValue(new DateTime(2014, 2, 10), new Pecuniam(3.91M));



        }

        [Test]
        public void TestRandomMasterCardNumber()
        {
            var testResult = MasterCardCc.RandomMasterCardNumber();
            Assert.IsNotNull(testResult);
            Assert.IsNotNull(testResult.Value);
            Assert.AreEqual(16, testResult.Value.Length);
            Assert.IsTrue(Regex.IsMatch(testResult.Value, "^5[1-5]"));
            Console.WriteLine(testResult.ToString());
        }

        [Test]
        public void TestRandomVisaNumber()
        {
            var testResult = VisaCc.RandomVisaNumber();
            Assert.IsNotNull(testResult);
            Assert.IsNotNull(testResult.Value);
            Assert.AreEqual(16, testResult.Value.Length);
            Assert.IsTrue(Regex.IsMatch(testResult.Value, "^4"));
            Console.WriteLine(testResult.ToString());
        }

        [Test]
        public void TestRandomDiscoverNumber()
        {
            var testResult = DiscoverCc.RandomDiscoverNumber();
            Assert.IsNotNull(testResult);
            Assert.IsNotNull(testResult.Value);
            Assert.AreEqual(16, testResult.Value.Length);
            Assert.IsTrue(Regex.IsMatch(testResult.Value, "^6011"));
            Console.WriteLine(testResult.ToString());
        }

        [Test]
        public void TestRandomAmexNumber()
        {
            var testResult = AmexCc.RandomAmexNumber();
            Assert.IsNotNull(testResult);
            Assert.IsNotNull(testResult.Value);
            Assert.AreEqual(15, testResult.Value.Length);
            Assert.IsTrue(Regex.IsMatch(testResult.Value, "^3(4|7)"));
            Console.WriteLine(testResult.ToString());
        }

        [Test]
        public void TestAmexCcCtor()
        {
            var ccNum = "3754151538861370";
            try
            {
                var badLength = new AmexCc(ccNum, new VocaBase("Bee Cardholder"),null,null);
                Assert.Fail($"The value {ccNum} is too long and should have failed.");
            }
            catch (ArgumentException)
            {
                Assert.Pass();
            }

            ccNum = "375415153886137";
            var isValid = new AmexCc(ccNum, new VocaBase("Bee Cardholder"), null, null);
            Assert.IsNotNull(isValid);

            ccNum = "385415153886130";
            try
            {
                var badChkDigit = new AmexCc(ccNum, new VocaBase("Bee Cardholder"), null, null);
                Assert.Fail($"The value {ccNum} has the wrong check digit.");
            }
            catch (ArgumentException)
            {
                Assert.Pass();
            }

            ccNum = "4263800185720486";
            try
            {
                var badChkDigit = new AmexCc(ccNum, new VocaBase("Bee Cardholder"), null, null);
                Assert.Fail($"The value {ccNum} is not AMEX pattern");
            }
            catch (ArgumentException)
            {
                Assert.Pass();
            }
        }

        [Test]
        public void TestMasterCardCcCtor()
        {
            var ccNum = "54331142445527120";
            try
            {
                var badLength = new MasterCardCc(ccNum, new VocaBase("Bee Cardholder"), null, null);
                Assert.Fail($"The value {ccNum} is too long and should have failed.");
            }
            catch (ArgumentException)
            {
                Assert.Pass();
            }

            ccNum = "5433114244552712";
            var isValid = new MasterCardCc(ccNum, new VocaBase("Bee Cardholder"), null, null);
            Assert.IsNotNull(isValid);

            ccNum = "5433114244552710";
            try
            {
                var badChkDigit = new MasterCardCc(ccNum, new VocaBase("Bee Cardholder"), null, null);
                Assert.Fail($"The value {ccNum} has the wrong check digit.");
            }
            catch (ArgumentException)
            {
                Assert.Pass();
            }

            ccNum = "6011723257285586";
            try
            {
                var badChkDigit = new MasterCardCc(ccNum, new VocaBase("Bee Cardholder"), null, null);
                Assert.Fail($"The value {ccNum} is not MC pattern");
            }
            catch (ArgumentException)
            {
                Assert.Pass();
            }
        }

        [Test]
        public void TestVisaCcCtor()
        {
            var ccNum = "42638001857204860";
            try
            {
                var badLength = new VisaCc(ccNum, new VocaBase("Bee Cardholder"), null, null);
                Assert.Fail($"The value {ccNum} is too long and should have failed.");
            }
            catch (ArgumentException)
            {
                Assert.Pass();
            }

            ccNum = "4263800185720486";
            var isValid = new VisaCc(ccNum, new VocaBase("Bee Cardholder"), null, null);
            Assert.IsNotNull(isValid);

            ccNum = "4263800185720481";
            try
            {
                var badChkDigit = new VisaCc(ccNum, new VocaBase("Bee Cardholder"), null, null);
                Assert.Fail($"The value {ccNum} has the wrong check digit.");
            }
            catch (ArgumentException)
            {
                Assert.Pass();
            }

            ccNum = "5433114244552712";
            try
            {
                var badChkDigit = new VisaCc(ccNum, new VocaBase("Bee Cardholder"), null, null);
                Assert.Fail($"The value {ccNum} is not VISA pattern");
            }
            catch (ArgumentException)
            {
                Assert.Pass();
            }
        }

        [Test]
        public void TestDiscoverCcCtor()
        {
            var ccNum = "60117232572855860";
            try
            {
                var badLength = new DiscoverCc(ccNum, new VocaBase("Bee Cardholder"), null, null);
                Assert.Fail($"The value {ccNum} is too long and should have failed.");
            }
            catch (ArgumentException)
            {
                Assert.Pass();
            }

            ccNum = "6011723257285586";
            var isValid = new DiscoverCc(ccNum, new VocaBase("Bee Cardholder"), null, null);
            Assert.IsNotNull(isValid);

            ccNum = "6011723257285582";
            try
            {
                var badChkDigit = new DiscoverCc(ccNum, new VocaBase("Bee Cardholder"), null, null);
                Assert.Fail($"The value {ccNum} has the wrong check digit.");
            }
            catch (ArgumentException)
            {
                Assert.Pass();
            }

            ccNum = "4263800185720486";
            try
            {
                var badChkDigit = new DiscoverCc(ccNum, new VocaBase("Bee Cardholder"), null, null);
                Assert.Fail($"The value {ccNum} is not DISCOVER pattern");
            }
            catch (ArgumentException)
            {
                Assert.Pass();
            }
        }

        [Test]
        public void TestRandomCc_AsParser()
        {
            var testResult = CreditCard.RandomCreditCard("6011723257285586", "Bee Cardholder");
            Assert.IsNotNull(testResult);
            Assert.IsInstanceOf<DiscoverCc>(testResult);

            testResult = CreditCard.RandomCreditCard("375415153886137", "Bee Cardholder");
            Assert.IsNotNull(testResult);
            Assert.IsInstanceOf<AmexCc>(testResult);

            testResult = CreditCard.RandomCreditCard("4263800185720486", "Bee Cardholder");
            Assert.IsNotNull(testResult);
            Assert.IsInstanceOf<VisaCc>(testResult);

            testResult = CreditCard.RandomCreditCard("5433114244552712", "Bee Cardholder");
            Assert.IsNotNull(testResult);
            Assert.IsInstanceOf<MasterCardCc>(testResult);
        }
    }
}
