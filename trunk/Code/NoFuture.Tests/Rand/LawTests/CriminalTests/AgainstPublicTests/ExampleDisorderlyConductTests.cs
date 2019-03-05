using System;
using NoFuture.Rand.Law.Criminal.AgainstPublic.US.Elements;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.PenalCode;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Criminal.Tests.AgainstPublicTests
{
    [TestFixture]
    public class ExampleDisorderlyConductTests
    {
        [Test]
        public void ExampleDirorderlyConductAct()
        {
            var testAct = new DisorderlyConduct
            {
                IsUnreasonablyLoud = lp => lp is DavidInebriatedEg || lp is DanielDrunkbuddyEg,
            };

            var testResult = testAct.IsValid(new DavidInebriatedEg(), new DanielDrunkbuddyEg());
            Console.WriteLine(testAct.ToString());
            Assert.IsTrue(testResult);
        }

        [Test]
        public void TestWithAttendantCircumstances()
        {
            var publicPlace = new PublicPlace("sidewalk")
                {IsWithin = lp => lp is DavidInebriatedEg || lp is DanielDrunkbuddyEg};
            var testCrime = new Misdemeanor
            {
                ActusReus = new DisorderlyConduct
                {
                    IsUnreasonablyLoud = lp => lp is DavidInebriatedEg || lp is DanielDrunkbuddyEg
                },
                MensRea = new Recklessly
                {
                    IsDisregardOfRisk = lp => lp is DavidInebriatedEg || lp is DanielDrunkbuddyEg,
                    IsUnjustifiableRisk = lp => lp is DavidInebriatedEg || lp is DanielDrunkbuddyEg
                },
                AttendantCircumstances = { publicPlace }
            };
            var testResult = testCrime.IsValid(new DavidInebriatedEg(), new DanielDrunkbuddyEg());
            Console.WriteLine(testCrime.ToString());
            Assert.IsTrue(testResult);

            publicPlace.IsAccessibleToPublic = false;
            publicPlace.Name = "private residence in the country";
            testResult = testCrime.IsValid(new DavidInebriatedEg(), new DanielDrunkbuddyEg());
            Console.WriteLine(testCrime.ToString());
            Assert.IsFalse(testResult);
        }
    }

    public class DavidInebriatedEg : LegalPerson
    {
        public DavidInebriatedEg() : base("DAVID INEBRIATED") { }
    }

    public class DanielDrunkbuddyEg : LegalPerson
    {
        public DanielDrunkbuddyEg() : base("DANIEL DRUNKBUDDY") { }
    }

}
