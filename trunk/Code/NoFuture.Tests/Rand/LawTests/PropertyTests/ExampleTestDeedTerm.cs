using NoFuture.Rand.Law.Property.US.FormsOf.InTerra;
using NoFuture.Rand.Law.Property.US.Terms;
using NoFuture.Rand.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Property.Tests
{
    [TestFixture]
    public class ExampleTestDeedTerm
    {
        [Test]
        public void TestDeedTermIsValid()
        {
            var property = new ExampleParcelOfLand
            {
                IsEntitledTo = lp => lp is ExampleOfferor,
                IsInPossessionOf = lp => lp is ExampleOfferor,
                Name = "Black Acre"
            };

            var testSubject = new DeedTerm("Deed to Black Acre", property)
            {
                IsSigned = lp => lp is ExampleOfferor || lp is ExampleOfferee,
                IsWritten = lp => true
            };

            var testResult = testSubject.IsValid(new ExampleOfferor(), new ExampleOfferee());
            Assert.IsTrue(testResult);

        }
    }

    public class ExampleParcelOfLand : RealProperty { }

    public class ExampleOfferor : LegalPerson, IOfferor
    {
        public ExampleOfferor() :base("Offeror") { }
    }

    public class ExampleOfferee : LegalPerson, IOfferee
    {
        public ExampleOfferee() : base("Offeree") { }
    }
}
