using System;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Defense;
using NoFuture.Rand.Law.Criminal.US.Elements;
using NoFuture.Rand.Law.Criminal.US.Elements.Act;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.ComLaw;
using NoFuture.Rand.Law.Criminal.US.Terms;
using NoFuture.Rand.Law.US;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Criminal.Tests.DefenseTests.DefenseOfOtherTests
{
    [TestFixture()]
    public class ExampleDuressDefenseTests
    {
        [Test]
        public void ExampleDuressDefense()
        {
            var testCrime = new Felony
            {
                ActusReus = new ActusReus
                {
                    IsVoluntary = lp => lp is BrianEg,
                    IsAction = lp => lp is BrianEg
                },
                MensRea = new GeneralIntent
                {
                    IsKnowledgeOfWrongdoing = lp => lp is BrianEg,
                    IsIntentOnWrongdoing = lp => lp is BrianEg
                }
            };

            var testResult = testCrime.IsValid(new BrianEg());
            Assert.IsTrue(testResult);

            var testSubject = new ChoiceThereof<ITermCategory>(ExtensionMethods.Defendant)
            {
                GetChoice = lp =>
                {
                    if (lp is KeishaEg)
                        return new Embezzled();
                    if (lp is BrianEg)
                        return new SeriousBodilyInjury();
                    return null;
                },
                GetOtherPossibleChoices = lp =>
                {
                    if (lp is KeishaEg)
                        return new[] {new SeriousBodilyInjury(),};
                    return null;
                }
            };

            testResult = testSubject.IsValid(new KeishaEg(), new BrianEg());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);
        }
    }

    public class Embezzled : TermCategory
    {
        protected override string CategoryName { get; } = "embezzled";
        public override int GetCategoryRank()
        {
            return new SeriousBodilyInjury().GetCategoryRank() - 1;
        }
    }

    public class KeishaEg : LegalPerson, IDefendant
    {
        public KeishaEg() : base("KEISHA TELLER") { }
    }

    public class BrianEg : LegalPerson, IDefendant
    {
        public BrianEg() : base("BRIAN BANKROBBER") { }
    }
}
