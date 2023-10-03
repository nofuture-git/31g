using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace NoFuture.Law.Tests
{
    [TestFixture]
    public class TermCategoryTests
    {
        [Test]
        public void TestIsRank()
        {
            var negOne = new NegOneRank();
            var zero = new ZeroRank();
            var one = new OneRank();

            Assert.IsTrue(TermCategory.IsRank(TermCategoryBoolOps.Eq, zero, zero));
            Assert.IsFalse(TermCategory.IsRank(TermCategoryBoolOps.Eq, one, zero));
            Assert.IsFalse(TermCategory.IsRank(TermCategoryBoolOps.Eq, zero, one));

            Assert.IsTrue(TermCategory.IsRank(TermCategoryBoolOps.Ne, zero, one));
            Assert.IsFalse(TermCategory.IsRank(TermCategoryBoolOps.Ne, one, one));

            Assert.IsTrue(TermCategory.IsRank(TermCategoryBoolOps.Gt, one, zero));
            Assert.IsTrue(TermCategory.IsRank(TermCategoryBoolOps.Gt, zero, negOne));

            Assert.IsTrue(TermCategory.IsRank(TermCategoryBoolOps.Lt, zero, one));
            Assert.IsTrue(TermCategory.IsRank(TermCategoryBoolOps.Lt, negOne, zero));

            Assert.IsTrue(TermCategory.IsRank(TermCategoryBoolOps.Ge, one, zero));
            Assert.IsTrue(TermCategory.IsRank(TermCategoryBoolOps.Ge, zero, negOne));
            Assert.IsTrue(TermCategory.IsRank(TermCategoryBoolOps.Ge, zero, zero));
            Assert.IsTrue(TermCategory.IsRank(TermCategoryBoolOps.Ge, negOne, negOne));
            Assert.IsTrue(TermCategory.IsRank(TermCategoryBoolOps.Ge, one, one));

            Assert.IsTrue(TermCategory.IsRank(TermCategoryBoolOps.Le, zero, one));
            Assert.IsTrue(TermCategory.IsRank(TermCategoryBoolOps.Le, negOne, zero));


        }
    }

    public class ZeroRank : TermCategory
    {
        public override int GetRank()
        {
            return 0;
        }
    }

    public class OneRank : TermCategory
    {
        public override int GetRank()
        {
            return 1;
        }
    }

    public class NegOneRank : TermCategory
    {
        public override int GetRank()
        {
            return -1;
        }
    }
}
