using System;
using NoFuture.Rand.Law.Property.US.FormsOf;
using NoFuture.Rand.Law.Property.US.FormsOf.InTerra;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Property.Tests
{
    [TestFixture]
    public class PropertyInterestFactoryTests
    {
        [Test]
        public void TestApiLookAndStyle()
        {
            var test00 =
                new PropertyInterestFactory(new RealProperty("some land"), ExtensionMethods.Defendant)
                    .IsPresentInterestPossibleInfinite(lp => true, new CurtisLandholder()) as FeeSimpleFactory;

            Assert.IsNotNull(test00 );

            var test01 = test00.IsPresentInterestDefinitelyInfinite(lp => false, new CurtisLandholder()) as DefeasibleFeeFactory;

            Assert.IsNotNull(test01);

            var test02 =
                test01.IsFutureInterestInGrantor(lp => true, new CurtisLandholder()) as
                    PropertyInterestFactoryValue<FeeSimpleSubject2ExecutoryInterest>;

            Assert.IsNotNull(test02);

            var testResult = test02.GetValue();

            Assert.IsNotNull(testResult);

            Assert.IsInstanceOf<FeeSimpleSubject2ExecutoryInterest>(testResult);

        }

        public class CurtisLandholder : LegalPerson, IDefendant
        {

        }
    }
}
