using System;
using NoFuture.Law;
using NoFuture.Law.Procedure.Civil.US.Jurisdiction;
using NoFuture.Law.US.Courts;
using NoFuture.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Law.Procedure.Civil.Tests
{
    [TestFixture]
    public class ExampleTestFederalVenue
    {
        [Test]
        public void TestFederalVenueIsValid()
        {
            var testSubject = new FederalVenue(new FederalCourt("District 01"))
            {
                GetDomicileLocation = lp =>
                {
                    if (lp is IPlaintiff)
                        return new VocaBase("District 02");
                    if (lp is IDefendant)
                        return new VocaBase("District 08");
                    return null;
                },

                GetInjuryLocation = lp => new VocaBase("District 01"),

            };

            var testResult = testSubject.IsValid(new ExamplePlaintiff(), new ExampleDefendant());
            Assert.IsTrue(testResult);
            Console.WriteLine(testSubject.ToString());
        }
    }
}
