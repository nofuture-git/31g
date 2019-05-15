using System;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Elements.Act;
using NoFuture.Rand.Law.Criminal.US.Terms;
using NoFuture.Rand.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Criminal.Tests.AgainstPublicTests
{
    [TestFixture]
    public class ExampleCriminalGangTests
    {
        [Test]
        public void TestIsCriminalGang()
        {
            var testGang = new CriminalGang("North Side Boys");
            for (var i = 0; i < 55; i++)
            {
                testGang.Add(new NorthSideBoyEg());
            }
            testGang.Idiosyncrasies.Add(new GangColors());
            testGang.Idiosyncrasies.Add(new GangTatoo());
            testGang.CriminalActivity.Add(new DrugDealin());

            var testResult = testGang.IsValid(new MikeWannbeEg());
            testGang.Add(new MikeWannbeEg());
            Console.WriteLine(testGang.ToString());
            Assert.IsTrue(testResult);

        }

        [Test]
        public void TestIsGangMember()
        {
            var testGang = new CriminalGang("North Side Boys");
            for (var i = 0; i < 55; i++)
            {
                testGang.Add(new NorthSideBoyEg());
            }
            testGang.Idiosyncrasies.Add(new GangColors());
            testGang.Idiosyncrasies.Add(new GangTatoo());
            testGang.CriminalActivity.Add(new DrugDealin());
            testGang.Add(new MikeWannbeEg());

            Assert.IsTrue(testGang.IsGangMember(new MikeWannbeEg()));
        }
    }

    public class GangColors : Idiosyncrasy
    {
        protected override string CategoryName => "gang colors: black and white";
    }

    public class GangTatoo : Idiosyncrasy
    {
        protected override string CategoryName => "gang tatoo";
    }

    public class NorthSideBoyEg : LegalPerson
    {
        public NorthSideBoyEg() : base("NORTH SIDE BOY") { }
    }

    public class MikeWannbeEg : LegalPerson, IDefendant
    {
        public MikeWannbeEg() : base("MIKE WANNABE") { }
    }

    public class DrugDealin : ActusReus
    {

    }
}
