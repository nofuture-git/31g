using System;
using NoFuture.Rand.Law;
using NoFuture.Rand.Law.US.Criminal;
using NoFuture.Rand.Law.US.Criminal.Elements;
using NoFuture.Rand.Law.US.Criminal.Elements.Act;
using NUnit.Framework;

namespace NoFuture.Rand.Tests.LawTests.CriminalTests.ActusReusTests
{
    /// <summary>
    /// 998 S.W.2d 363 (1999) Wendell Hollis OLER, Appellant, v. The STATE of Texas, Appellee. No. 05-97-01229-CR. Court of Appeals of Texas, Dallas.
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, possession is another kind of actus reus 
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class WendellHolisvTexasTests
    {
        [Test]
        public void WendllHolisvTexas()
        {
            var testSubject = new Felony()
            {
                IsChargedWith = lp =>
                {
                    var isPossessionOfCtrlSubstance = lp is WendellHollis;
                    var isByObtainByMisrepresentation = lp is WendellHollis;
                    return isPossessionOfCtrlSubstance && isByObtainByMisrepresentation;
                }
            };
            //court contents the actus reus is by possession, therefore omission is not applicable
            var testPossession = new Possession();
            testSubject.ActusReus = testPossession;
            testPossession.IsKnowinglyProcured = lp => lp is WendellHollis;
            var testResult = testPossession.IsValid(new WendellHollis());
            Console.WriteLine(testPossession.ToString());
            Assert.IsTrue(testResult);

            //defendant contents no statute duty to report multiple prescriptions
            var testOmission = new DutyToAct {IsStatuteOrigin = lp => false};
            testResult = testOmission.IsValid(new WendellHollis());
            Assert.IsFalse(testResult);
        }
    }

    public class WendellHollis : LegalPerson
    {
        public WendellHollis() : base("WENDELL HOLLIS") { }
    }
}
