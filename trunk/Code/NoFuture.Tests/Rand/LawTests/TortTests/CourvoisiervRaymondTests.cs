using System;
using NoFuture.Rand.Law.Criminal.US.Defense.Justification;
using NUnit.Framework;
using NoFuture.Rand.Law.US.Persons;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Tort.Tests
{
    /// <summary>
    /// Courvoisier v. Raymond, 47 P. 284 (Colo. 1896)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctine issue: reasonable error in self-defense
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class CourvoisiervRaymondTests
    {
        [Test]
        public void CourvoisiervRaymond()
        {
            var test = new DefenseOfSelf
            {
                Imminence = new Imminence(ExtensionMethods.Tortfeasor),
                IsReasonableFearOfInjuryOrDeath = lp => lp is Courvoisier,
                Proportionality = new Proportionality<ITermCategory>(ExtensionMethods.Tortfeasor)
                {
                    GetChoice = lp => new PullRevolverShoot(),
                    IsProportional = (t1, t2) => t1.Equals(t2),
                },
                Provocation = new Provocation(ExtensionMethods.Tortfeasor)
                {
                    IsInitiatorOfAttack = lp => false,
                    IsInitiatorRespondingToExcessiveForce = lp => false,
                    IsInitiatorWithdraws = lp => false
                }
            };

            var testResult = test.IsValid(new Courvoisier(), new Raymond());
            Console.WriteLine(test.ToString());
            Assert.IsTrue(testResult);

        }
    }

    public class PullRevolverShoot : TermCategory
    {
        protected override string CategoryName => "blasted varmit";

        public override bool Equals(object obj)
        {
            return obj is PullRevolverShoot;
        }
    }

    public class Courvoisier : LegalPerson, ITortfeasor
    {
        public Courvoisier(): base("") { }
    }

    public class Raymond : LegalPerson, IPlaintiff
    {
        public Raymond(): base("") { }
    }
}
