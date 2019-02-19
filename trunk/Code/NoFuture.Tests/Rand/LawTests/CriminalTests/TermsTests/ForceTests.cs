using NoFuture.Rand.Law.Criminal.US.Terms;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Tests.CriminalTests.TermsTests
{
    [TestFixture]
    public class ForceTests
    {
        [Test]
        public void TestGetCategoryRank()
        {
            var testSubject00 = new DeadlyForce();
            var testSubject01 = new NondeadlyForce();

            Assert.IsTrue(testSubject00.GetCategoryRank() > testSubject01.GetCategoryRank());
        }
    }
}
