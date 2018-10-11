using System.Collections.Generic;
using System.Linq;
using NoFuture.Util.DotNetMeta.TokenName;
using NUnit.Framework;

namespace NoFuture.Util.DotNetMeta.Tests.TokenNameTest
{
    [TestFixture()]
    public class TestMetadataTokenNameComparer
    {

        [Test]
        public void TestEquals()
        {
            var tx = new MetadataTokenName
            {
                Name = "abc.xyz",
                Id = 4451,
                DeclTypeId = 3322,
                OwnAsmIdx = 0,
                RslvAsmIdx = 0
            };

            var ty = new MetadataTokenName
            {
                Name = "abc.xyz",
                Id = 1544,
                DeclTypeId = 2233,
                OwnAsmIdx = 1,
                RslvAsmIdx = 1
            };

            var testSubject = new MetadataTokenNameComparer();
            var testResult = testSubject.Equals(tx, ty);
            Assert.IsTrue(testResult);

            ty.Name = "xyz.abc";
            testResult = testSubject.Equals(tx, ty);
            Assert.IsFalse(testResult);
        }

        [Test]
        public void TestEquals_EmptyNames()
        {
            var tx = new MetadataTokenName
            {
                Name = "",
                Id = 4451,
                DeclTypeId = 3322,
                OwnAsmIdx = 0,
                RslvAsmIdx = 0
            };

            var ty = new MetadataTokenName
            {
                Name = "",
                Id = 1544,
                DeclTypeId = 2233,
                OwnAsmIdx = 1,
                RslvAsmIdx = 1
            };

            var testSubject = new MetadataTokenNameComparer();
            var testResult = testSubject.Equals(tx, ty);
            Assert.IsFalse(testResult);
        }

        [Test]
        public void TestEquals_SameVariable()
        {
            var tx = new MetadataTokenName
            {
                Name = "",
                Id = 4451,
                DeclTypeId = 3322,
                OwnAsmIdx = 0,
                RslvAsmIdx = 0
            };

            var ty = tx;

            var testSubject = new MetadataTokenNameComparer();
            var testResult = testSubject.Equals(tx, ty);
            Assert.IsTrue(testResult);
        }

        [Test]
        public void TestEquals_Distinct()
        {
            var testList = new List<MetadataTokenName>
            {
                new MetadataTokenName
                {
                    Name = "abc.xyz",
                    Id = 4451,
                    DeclTypeId = 3322,
                    OwnAsmIdx = 0,
                    RslvAsmIdx = 0
                },
                new MetadataTokenName
                {
                    Name = "abc.xyz",
                    Id = 1544,
                    DeclTypeId = 2233,
                    OwnAsmIdx = 1,
                    RslvAsmIdx = 1
                },
                new MetadataTokenName
                {
                    Id = 1544,
                    DeclTypeId = 2233,
                    OwnAsmIdx = 1,
                    RslvAsmIdx = 1
                },
                new MetadataTokenName
                {
                    Id = 1544,
                    DeclTypeId = 2233,
                    OwnAsmIdx = 1,
                    RslvAsmIdx = 1
                },
                new MetadataTokenName
                {
                    Id = 8852,
                    DeclTypeId = 2233,
                    OwnAsmIdx = 1,
                    RslvAsmIdx = 1
                },
                new MetadataTokenName
                {
                    Name = "xyz.abc",
                    Id = 4451,
                    DeclTypeId = 3322,
                    OwnAsmIdx = 0,
                    RslvAsmIdx = 0
                }
            };
            var testSubject = new MetadataTokenNameComparer();
            var testResult = testList.Distinct(testSubject).ToList();
            Assert.AreEqual(4, testResult.Count);

        }
    }
}
