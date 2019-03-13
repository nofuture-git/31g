using System;
using System.Collections.Generic;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Defense.Justification;
using NoFuture.Rand.Law.Criminal.US.Elements;
using NoFuture.Rand.Law.Criminal.US.Elements.Act;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.ComLaw;
using NoFuture.Rand.Law.Criminal.US.Terms;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Criminal.Tests.DefenseTests.DefenseOfOtherTests
{
    /// <summary>
    /// People v. Lovercamp, 43 Cal. App. 3d 823 (1974)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, necessity defense of prisoners is strict and limiting
    /// ]]>
    /// </remarks>
    [TestFixture()]
    public class PeoplevLovercampTests
    {
        [Test]
        public void PeoplevLovercamp()
        {
            var testCrime = new Felony
            {
                ActusReus = new ActusReus
                {
                    IsAction = lp => lp is Lovercamp || lp is MsWynashe,
                    IsVoluntary = lp => lp is Lovercamp || lp is MsWynashe
                },
                MensRea = new GeneralIntent
                {
                    IsKnowledgeOfWrongdoing = lp => lp is Lovercamp || lp is MsWynashe,
                    IsIntentOnWrongdoing = lp => lp is Lovercamp || lp is MsWynashe,
                },
            };

            var testResult = testCrime.IsValid(new Lovercamp());
            Assert.IsTrue(testResult);

            var testSubject = new NecessityDefense(testCrime)
            {
                Imminence = new Imminence(testCrime.GetDefendant)
                {
                    GetResponseTime = lp => Imminence.NormalReactionTimeToDanger
                },
                IsMultipleInHarm = lp => lp is Lovercamp || lp is MsWynashe,
                Proportionality = new ChoiceThereof<ITermCategory>(testCrime.GetDefendant)
                {
                    GetChoice = lp =>
                    {
                        if (lp is Lovercamp || lp is MsWynashe)
                            return new EscCamp();
                        if (lp is LesbianInmates)
                            return new SeriousBodilyInjury();
                        return null;
                    },
                    GetOtherPossibleChoices = lp => new List<ITermCategory> {new SeriousBodilyInjury(), new DeadlyForce()}
                },
            };

            testResult = testSubject.IsValid(new Lovercamp(), new LesbianInmates());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);

        }
    }

    public class EscCamp : TermCategory
    {
        protected override string CategoryName { get; } = " escape from the California Rehabilitation Center";
        public override int GetCategoryRank()
        {
            return new NondeadlyForce().GetCategoryRank() - 1;
        }
    }

    public class LesbianInmates : LegalPerson, IDefendant
    {
        public LesbianInmates() : base("LESBIAN INMATES") { }
    }

    public class Lovercamp : LegalPerson, IDefendant
    {
        public Lovercamp() : base("MARSHA LOVERCAMP") { }
    }

    public class MsWynashe : LegalPerson, IDefendant
    {
        public MsWynashe() : base("MS. WYNASHE") { }
    }
}
