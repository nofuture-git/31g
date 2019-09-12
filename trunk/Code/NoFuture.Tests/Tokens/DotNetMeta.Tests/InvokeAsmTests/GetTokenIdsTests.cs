using System.Linq;
using NoFuture.Tokens.DotNetMeta.TokenId;
using NUnit.Framework;

namespace NoFuture.Util.DotNetMeta.Tests.InvokeAsmTests
{
    [TestFixture]
    public class GetTokenIdsTests
    {
        [Test]
        public void TestFlattenToDistinct()
        {
            var t0 = new MetadataTokenId
            {
                Id = 1,
                RslvAsmIdx = 0,
                Items = new[]
                {
                    new MetadataTokenId
                    {
                        Id = 2,
                        RslvAsmIdx = 0,
                        Items = new[]
                        {
                            new MetadataTokenId {Id = 3, RslvAsmIdx = 0},
                            new MetadataTokenId
                            {
                                Id = 4,
                                RslvAsmIdx = 0,
                                Items = new[]
                                {
                                    new MetadataTokenId {Id = 1, RslvAsmIdx = 1},
                                    new MetadataTokenId {Id = 2, RslvAsmIdx = 1}
                                }
                            },
                            new MetadataTokenId
                            {
                                Id = 5,
                                RslvAsmIdx = 0,
                                Items = new[] {new MetadataTokenId {Id = 1, RslvAsmIdx = 1}}
                            }
                            
                        },
                        
                    },
                    new MetadataTokenId {Id = 6, RslvAsmIdx = 0},
                    new MetadataTokenId {Id = 7, RslvAsmIdx = 0}
                }
            };
            var t1 = new MetadataTokenId
            {
                Id = 8,
                RslvAsmIdx = 0,
                Items = new[]
                {
                    new MetadataTokenId
                    {
                        Id = 9,
                        RslvAsmIdx = 0,
                        Items = new[] {new MetadataTokenId {Id = 1, RslvAsmIdx = 0}}
                    }
                }
            };
            var testSubject = new MetadataTokenId() {Items = new[] {t0, t1}};
            var testResult = testSubject.SelectDistinct();
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Length);
            Assert.AreEqual(11, testResult.Length);

            testResult = testSubject.SelectDistinct(true);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Length);
            Assert.AreEqual(11,testResult.Length);

            var t1x0 = testResult.FirstOrDefault(x => x.Id == 1 && x.RslvAsmIdx == 0);
            Assert.IsNotNull(t1x0);
            Assert.IsNotNull(t1x0.Items);
            Assert.AreNotEqual(0,t1x0.Items.Length);
            Assert.AreEqual(3, t1x0.Items.Length);
        }
    }
}
