using System;
using NoFuture.Rand.Law;
using NoFuture.Rand.Law.US.Contracts.Terms;
using NUnit.Framework;

namespace NoFuture.Rand.Tests.LawTests.ContractTests
{
    [TestFixture]
    public class ContractTermTests
    {
        [Test]
        public void TestCompareTo()
        {
            var testSubject00 = new ContractTerm<object>("car parts", "car parts");
            var testSubject01 = new ContractTerm<object>("car parts", "car parts");

            var testResult = testSubject00.CompareTo(null);
            Assert.AreEqual(1, testResult);

            testResult = testSubject00.CompareTo(testSubject01);
            Assert.AreEqual(0, testResult);

            testSubject00.As(new WrittenTerm());
            testResult = testSubject00.CompareTo(testSubject01);
            Assert.AreEqual(-1, testResult);

            testResult = testSubject01.CompareTo(testSubject00);
            Assert.AreEqual(1, testResult);
        }

        [Test]
        public void TestCompareTo_StdPrefInter()
        {
            var testSubject00 = new ContractTerm<object>("car parts", "car parts", new ExpressTerm());
            var testSubject01 = new ContractTerm<object>("car parts", "car parts", new ExpressTerm());

            var testResult = testSubject00.CompareTo(testSubject01);
            Assert.AreEqual(0, testResult);

            testSubject00 = new ContractTerm<object>("car parts", "car parts", new ExpressTerm());
            testSubject01 = new ContractTerm<object>("car parts", "car parts", new CourseOfPerformanceTerm());

            testResult = testSubject00.CompareTo(testSubject01);
            Assert.IsTrue(testResult < 0);

            testResult = testSubject01.CompareTo(testSubject00);
            Assert.IsTrue(testResult > 0);


            testSubject00 = new ContractTerm<object>("car parts", "car parts", new CourseOfPerformanceTerm());
            testSubject01 = new ContractTerm<object>("car parts", "car parts", new CourseOfDealingTerm());

            testResult = testSubject00.CompareTo(testSubject01);
            Assert.IsTrue(testResult < 0);

            testResult = testSubject01.CompareTo(testSubject00);
            Assert.IsTrue(testResult > 0);

            testSubject00 = new ContractTerm<object>("car parts", "car parts", new CourseOfDealingTerm());
            testSubject01 = new ContractTerm<object>("car parts", "car parts", new UsageOfTradeTerm());

            testResult = testSubject00.CompareTo(testSubject01);
            Assert.IsTrue(testResult < 0);

            testResult = testSubject01.CompareTo(testSubject00);
            Assert.IsTrue(testResult > 0);
        }
    }
}
