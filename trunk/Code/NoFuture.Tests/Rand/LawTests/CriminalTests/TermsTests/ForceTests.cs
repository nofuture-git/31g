﻿using NoFuture.Rand.Law.Criminal.US.Terms;
using NoFuture.Rand.Law.Criminal.US.Terms.Violence;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Criminal.Tests.TermsTests
{
    [TestFixture]
    public class ForceTests
    {
        [Test]
        public void TestGetCategoryRank()
        {
            var testSubject00 = new DeadlyForce();
            var testSubject01 = new NondeadlyForce();

            Assert.IsTrue(testSubject00.GetRank() > testSubject01.GetRank());
        }
    }
}
