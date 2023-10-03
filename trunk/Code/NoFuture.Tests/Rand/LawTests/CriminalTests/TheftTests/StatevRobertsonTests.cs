using System;
using NoFuture.Law.Criminal.US;
using NoFuture.Law.Criminal.US.Elements.AgainstProperty.Theft;
using NoFuture.Law.Criminal.US.Elements.Intent.PenalCode;
using NoFuture.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Law.Criminal.Tests.TheftTests
{
    /// <summary>
    /// State v. Robertson, 531 S. E. 2d 490 (2000)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, without violence of force, its larceny not robbery
    /// ]]>
    /// </remarks>
    [TestFixture()]
    public class StatevRobertsonTests
    {
        [Test]
        public void StatevRobertson()
        {
            var testCrime = new Felony
            {
                ActusReus = new Robbery
                {
                    SubjectProperty = new LegalProperty("purse")
                    {
                        IsEntitledTo = lp => lp is MsDover,
                        IsInPossessionOf = lp => lp is MsDover
                    },
                    IsAsportation = lp => lp is Robertson,
                    IsTakenPossession = lp => lp is Robertson,
                    //court rules this is false, snatching is larceny not robbery 
                    IsByViolence = lp => false,
                },
                MensRea = new Purposely
                {
                    IsIntentOnWrongdoing = lp => lp is Robertson
                }
            };
            var testResult = testCrime.IsValid(new Robertson(), new MsDover());
            Console.WriteLine(testCrime.ToString());
            Assert.IsFalse(testResult);
        }
    }

    public class Robertson : LegalPerson, IDefendant
    {
        public Robertson() : base("WILLIE HERBERT ROBERTSON") {  }

    }

    public class MsDover : LegalPerson, IVictim
    {
        public MsDover() : base("MS. DOVER") {  }
    }
}
