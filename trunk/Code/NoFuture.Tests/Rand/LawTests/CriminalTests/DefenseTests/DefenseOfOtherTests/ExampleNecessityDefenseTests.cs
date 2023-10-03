using System;
using NoFuture.Law.Criminal.US;
using NoFuture.Law.Criminal.US.Defense;
using NoFuture.Law.Criminal.US.Defense.Justification;
using NoFuture.Law.Criminal.US.Elements;
using NoFuture.Law.Criminal.US.Elements.Act;
using NoFuture.Law.Criminal.US.Elements.Intent.ComLaw;
using NoFuture.Law.Criminal.US.Terms;
using NoFuture.Law.Criminal.US.Terms.Violence;
using NoFuture.Law.US;
using NoFuture.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Law.Criminal.Tests.DefenseTests.DefenseOfOtherTests
{
    [TestFixture]
    public class ExampleNecessityDefenseTests
    {
        [Test]
        public void ExampleNecessityDefense()
        {
            var testCrime = new Felony
            {
                ActusReus = new ActusReus
                {
                    IsAction = lp => lp is TamaraEg,
                    IsVoluntary = lp => lp is TamaraEg
                },
                MensRea = new GeneralIntent
                {
                    IsIntentOnWrongdoing = lp => lp is TamaraEg,
                    IsKnowledgeOfWrongdoing = lp => lp is TamaraEg
                }
            };

            var testResult = testCrime.IsValid(new TamaraEg());
            Assert.IsTrue(testResult);

            var testSubject = new NecessityDefense<ITermCategory>
            {
                Imminence = new Imminence(ExtensionMethods.Defendant)
                {
                    GetResponseTime = lp => Imminence.NormalReactionTimeToDanger
                },
                IsMultipleInHarm = lp => lp is TamaraEg,
                Proportionality = new ChoiceThereof<ITermCategory>(ExtensionMethods.Defendant)
                {
                    GetChoice = lp => new NondeadlyForce(),
                    GetOtherPossibleChoices = lp => new ITermCategory[]
                        {new SeriousBodilyInjury(), new DeadlyForce(), new Death()}
                }
            };
            testResult = testSubject.IsValid(new TamaraEg());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);
        }
    }

    public class TamaraEg : LegalPerson, IDefendant
    {
        public TamaraEg() : base("TAMARA LOST") { }
    }
}
