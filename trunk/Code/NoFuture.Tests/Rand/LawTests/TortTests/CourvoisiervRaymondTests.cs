using System;
using NoFuture.Law.Criminal.US.Defense.Justification;
using NUnit.Framework;
using NoFuture.Law.US.Persons;
using NoFuture.Law.US;

namespace NoFuture.Law.Tort.Tests
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
            var test = new DefenseOfSelf(ExtensionMethods.Tortfeasor)
            {
                IsReasonableFearOfInjuryOrDeath = lp => lp is Courvoisier,
                Proportionality = new Proportionality<ITermCategory>(ExtensionMethods.Tortfeasor)
                {
                    GetChoice = lp => new PullRevolverShoot(),
                    IsProportional = (t1, t2) => t1.Equals(t2),
                },
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
