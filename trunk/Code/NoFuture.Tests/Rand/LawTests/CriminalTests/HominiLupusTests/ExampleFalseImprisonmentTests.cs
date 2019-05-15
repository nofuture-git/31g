using System;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Elements.AgainstPersons;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.ComLaw;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Criminal.Tests.HominiLupusTests
{
    [TestFixture]
    public class ExampleFalseImprisonmentTests
    {
        [Test]
        public void ExampleIsFalseImprisonment()
        {
            var testCrime = new Felony
            {
                ActusReus = new FalseImprisonment
                {
                    IsConfineVictim = lp => lp is ThomasHitchhikerEg
                },
                MensRea = new GeneralIntent
                {
                    IsKnowledgeOfWrongdoing = lp => lp is ThomasHitchhikerEg
                }
            };

            var testResult = testCrime.IsValid(new ThomasHitchhikerEg());
            Console.WriteLine(testCrime.ToString());
            Assert.IsTrue(testResult);
        }
    }
}
