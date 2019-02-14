using System;
using System.Collections.Generic;
using NoFuture.Rand.Law.US.Criminal;
using NoFuture.Rand.Law.US.Criminal.Defense;
using NoFuture.Rand.Law.US.Criminal.Elements.Act;
using NoFuture.Rand.Law.US.Criminal.Elements.Intent.PenalCode;
using NoFuture.Rand.Law.US.Criminal.Terms;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Tests.CriminalTests.DefenseTests.DefenseOfOtherTests
{
    /// <summary>
    /// State v. Daoud, 141 N.H. 142 (1996)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, with duress, actus reus is true since an action taken under threat is still voluntary
    /// ]]>
    /// </remarks>
    [TestFixture()]
    public class StatevDaoudTests
    {
        [Test]
        public void StatevDaoud()
        {
            var testCrime = new Misdemeanor
            {
                ActusReus = new ActusReus
                {
                    IsVoluntary = lp => lp is Daoud,
                    IsAction = lp => lp is Daoud
                },
                MensRea = StrictLiability.Value
            };

            var testResult = testCrime.IsValid(new Daoud());
            Assert.IsTrue(testResult);

            var testSubject = new ChoiceThereof<ITermCategory>(testCrime)
            {
                GetChoice = lp => lp is Daoud ? new NondeadlyForce() : null,
                GetOtherPossibleChoices = lp => new List<ITermCategory>() {new CallForTaxi(), new WalkedToNeighbor()},
                OtherParties = () => new List<ILegalPerson>(){new JohnHilane()}
            };

            testResult = testSubject.IsValid(new Daoud());
            Console.WriteLine(testSubject.ToString());
            Assert.IsFalse(testResult);

        }
    }

    public class CallForTaxi : TermCategory
    {
        protected override string CategoryName { get; } = "called a friend or a taxi";
        public override int GetCategoryRank()
        {
            return new NondeadlyForce().GetCategoryRank() - 1;
        }
    }

    public class WalkedToNeighbor : CallForTaxi
    {
        protected override string CategoryName { get; } = "walked to a neighbor's apartment";
    }

    public class Daoud : LegalPerson
    {
        public Daoud() : base("KARIN DAOUD") { }
    }

    public class JohnHilane : LegalPerson
    {
        public JohnHilane() : base("JOHN HILANE") { }
    }
}
