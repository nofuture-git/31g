using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Law.Property.US.Acquisition.Found;
using NoFuture.Law.US;
using NUnit.Framework;

namespace NoFuture.Law.Property.Tests
{
    [TestFixture]
    public class ExampleFoundPropertyTests
    {
        private ILegalPerson _propertyOwner = new LegalPerson("Jim Owner");

        private ILegalProperty _property = new LegalProperty("something");

        internal ILegalPerson PropertyOwner(IEnumerable<ILegalPerson> persons)
        {
            return persons.FirstOrDefault(p => p.IsSamePerson(_propertyOwner));
        }

        [Test]
        public void TestAbandonedProperty()
        {
            var test = new AbandonedProperty(PropertyOwner)
            {
                //willingly walks away from property
                Relinquishment = new Act(PropertyOwner)
                {
                    IsVoluntary = lp => lp.IsSamePerson(_propertyOwner),
                    IsAction = lp => lp.IsSamePerson(_propertyOwner)
                },
                SubjectProperty = _property
            };

            var testResult = test.IsValid(_propertyOwner);
            Assert.IsTrue(testResult);
            Console.Write(test.ToString());
        }

        [Test]
        public void TestLostProperty()
        {
            var test = new LostProperty(PropertyOwner)
            {
                Relinquishment = new Act(PropertyOwner)
                {
                    IsVoluntary = lp => true
                },
                IsPropertyLocationKnown = lp => false,
                SubjectProperty = _property
            };

            var testResult = test.IsValid(_propertyOwner);
            Assert.IsFalse(testResult);
            Console.Write(test.ToString());
        }

        [Test]
        public void TestMislaidProperty()
        {
            var test = new MislaidProperty(PropertyOwner)
            {
                Relinquishment = new Act(PropertyOwner)
                {
                    IsVoluntary = lp => lp.IsSamePerson(_propertyOwner),
                    IsAction = lp => lp.IsSamePerson(_propertyOwner)
                },
                SubjectProperty = _property,
                IsPropertyLocationKnown = lp => lp.IsSamePerson(_propertyOwner)
            };

            var testResult = test.IsValid(_propertyOwner);
            Assert.IsFalse(testResult);
            Console.WriteLine(test.ToString());
        }

        [Test]
        public void TestTreasureTrove()
        {
            var test = new TreasureTrove(PropertyOwner)
            {
                IsConcealed = p => true,
                IsGoldSilverOrCurrency = p => true,
                SubjectProperty =  _property
            };

            var testResult = test.IsValid(_propertyOwner);
            Assert.IsTrue(testResult);
            Console.Write(test.ToString());
        }
    }
}
