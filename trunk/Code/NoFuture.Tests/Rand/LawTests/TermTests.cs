using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Law;
using NUnit.Framework;

namespace NoFuture.Law.Tests
{
    [TestFixture]
    public class TermTests
    {
        private static object something = new object();

        [Test]
        public void TestExample()
        {
            var myTerm = new Term<object>("something's name", DBNull.Value);
            myTerm.As(new ColorGreen());
            myTerm.As(new SmoothSurface());
            myTerm.As(new HardSurface());
            myTerm.As(new SmallSize());
            myTerm.As(new RoundShape());

            Assert.IsTrue(myTerm.IsCategory(new SmallSize()));
            Console.WriteLine(myTerm.ToString()); //something's name
            Console.WriteLine(myTerm.GetCategory());

            var myTerm2 = new Term<object>("another name for same thing", DBNull.Value);
            Assert.IsFalse(myTerm2.Equals(myTerm));
            Assert.IsTrue(myTerm2.EqualRefersTo(myTerm));

        }

        [Test]
        public void TestEquals()
        {
            var testSubj00 = new Term<object>("Swiss Coin Collection", new object());
            var testSubj01 = new Term<object>("Swiss Coin Collection", new object());

            Assert.IsTrue(testSubj00.Equals(testSubj01));
            Assert.IsFalse(testSubj00.EqualRefersTo(testSubj01));
            testSubj00 = new Term<object>("Swiss Coin Collection", something);
            testSubj01 = new Term<object>("Swiss Coin Collection", something);

            Assert.IsTrue(testSubj00.Equals(testSubj01));
            Assert.IsTrue(testSubj00.EqualRefersTo(testSubj01));

        }

        [Test]
        public void TestSetOps()
        {
            var set00 = new SortedSet<Term<object>>
            {
                new Term<object>("Swiss Coin Collection", new object()),
                new Term<object>("Rarity Coin Collection", new object())
            };

            var set01 = new SortedSet<Term<object>>
            {
                new Term<object>("Swiss Coin Collection", new object())
            };

            var intersect = set00.Where(oo => set01.Any(ee => oo.Equals(ee))).ToList();
            
            Assert.IsTrue(intersect.Any());
            Console.WriteLine(intersect.Count);

        }
    }

    public class ColorGreen : TermCategory
    {
        protected override string CategoryName => "green";
    }

    public class SmoothSurface : TermCategory
    {
        protected override string CategoryName => "smooth";
    }

    public class HardSurface : TermCategory
    {
        protected override string CategoryName => "hard";
    }

    public class SmallSize : TermCategory
    {
        protected override string CategoryName => "small";
    }

    public class RoundShape : TermCategory
    {
        protected override string CategoryName => "round";
    }
}
