using System;
using System.Linq;
using NoFuture.Rand.Law;
using NoFuture.Rand.Law.US.Criminal;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Tests.CriminalTests
{
    [TestFixture]
    public class CrimeTests
    {

        [Test]
        public void TestCompareTo()
        {
            ICrime[] testSubjects =
            {
                new Misdemeanor(), new Infraction(),
                new Misdemeanor(), new Felony(),
                new Infraction(), new Felony(),
                new Infraction(), new Misdemeanor(),
            };

            Array.Sort(testSubjects);

            foreach(var c in testSubjects)
                Console.WriteLine(c.GetType().Name);
            Assert.IsTrue(testSubjects.First() is Infraction);
            Assert.IsTrue(testSubjects.Last() is Felony);
        }

        [Test]
        public void TestGetDefendant()
        {
            var testResult = Government.GetDefendant(new TestPerson00(), null, null);
            Assert.IsNotNull(testResult);
            Console.WriteLine(testResult.Name);
        }
    }

    public class TestPerson00 : LegalPerson
    {
        public TestPerson00() : base("TEST PERSON")  { }
    }
}

namespace We.Whatever
{
    public class Classic00
    {
        public virtual object Example00(object something) { return new object(); }
        public virtual bool Example01(object something) { return false; }
        public virtual void Example02(object something) { }

        public virtual object Composition(object something)
        {
            var o = Example00(something);
            if(Example01(o))
                Example02(o);
            return o;
        }
    }

    public class ByEnclosures00
    {
        public virtual Func<object, object> Example00 { get; set; } = o => new object();
        public virtual Predicate<object> Example01 { get; set; } = o => false;
        public virtual Action<object> Example02 { get; set; } = o => { };

        public virtual Func<object, ByEnclosures00, object> Composition { get; set; } = (something, myself) =>
        {
            var o = myself.Example00(something);
            if (myself.Example01(o))
                myself.Example02(o);
            return o;
        };
    }

}

namespace We.Whatever.Tests
{
    public class TestOfClassic00 : Classic00
    {
        public override bool Example01(object something)
        {
            return true;
        }
    }

    [TestFixture]
    public class TestClassicvByEnclousure
    {
        [Test]
        public void TestExamples()
        {
            var enclosure = new ByEnclosures00
            {
                Example00 = o => new Classic00(),
                Example01 = o => true,
                Example02 = o => {}
            };


        }
    }

}


