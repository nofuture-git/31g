﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoFuture.Rand.Law.US.Criminal;
using NoFuture.Rand.Law.US.Criminal.Defense;
using NoFuture.Rand.Law.US.Criminal.Elements.Act;
using NoFuture.Rand.Law.US.Criminal.Elements.Intent.ComLaw;
using NoFuture.Rand.Law.US.Criminal.Terms;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace NoFuture.Rand.Law.Tests.CriminalTests.DefenseTests.DefenseOfOtherTests
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
                },
                OtherParties = () => new[] {new KeishaEg()}
            };

            var testResult = testCrime.IsValid(new BrianEg());
            Assert.IsTrue(testResult);

            var testSubject = new ChoiceThereof<ITermCategory>(testCrime)
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
                },
                OtherParties = () => new []{new BrianEg()}
            };

            testResult = testSubject.IsValid(new KeishaEg());
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

    public class KeishaEg : LegalPerson
    {
        public KeishaEg() : base("KEISHA TELLER") { }
    }

    public class BrianEg : LegalPerson
    {
        public BrianEg() : base("BRIAN BANKROBBER") { }
    }
}
