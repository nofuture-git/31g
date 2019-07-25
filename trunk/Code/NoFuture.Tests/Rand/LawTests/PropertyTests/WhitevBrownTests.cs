using System;
using NoFuture.Rand.Law.Property.US.FormsOf;
using NoFuture.Rand.Law.Property.US.FormsOf.InTerra.Interests;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Property.Tests
{
    /// <summary>
    /// White v. Brown, 559 S.W.2d 938 (Tenn. 1977)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, ambiguous words looking like life estate, ruled as not
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class WhitevBrownTests
    {
        [Test]
        public void WhitevBrown()
        {
            var property = new JessieLideHome();


            var test = new FeeSimpleAbsolute(ExtensionMethods.Plaintiff)
            {
                SubjectProperty = property,
                IsExactlyThisPerson = lp => property.IsAllowedToLiveIn(lp) && !property.IsToBeSoldMeanOnlyLifeEstate(lp)
            };

            var testResult = test.IsValid(new Brown01(), new White());
            Assert.IsTrue(testResult);

            Console.WriteLine(test.ToString());
        }
    }

    public class JessieLideHome : RealProperty
    {
        public Predicate<ILegalPerson> IsAllowedToLiveIn => lp => lp is White;

        /// <summary>
        /// This is what the court turns on to devise its a fee simple abs
        /// </summary>
        public Predicate<ILegalPerson> IsToBeSoldMeanOnlyLifeEstate => lp => lp is Brown01;
    }

    public class White : LegalPerson, IPlaintiff
    {
        public White(): base("White") { }
    }

    public class Brown01 : LegalPerson, IDefendant
    {
        public Brown01(): base("Brown") { }
    }
}
