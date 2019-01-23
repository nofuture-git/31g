using NoFuture.Rand.Law.US.Contracts;
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

            testSubject00.Source = TermSource.Written;
            testResult = testSubject00.CompareTo(testSubject01);
            Assert.AreEqual(-1, testResult);

            testResult = testSubject01.CompareTo(testSubject00);
            Assert.AreEqual(1, testResult);
        }
    }
}
