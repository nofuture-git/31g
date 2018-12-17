using System;
using System.Linq;
using NUnit.Framework;

namespace NoFuture.Antlr.DotNetIlTypeName.Tests
{
    [TestFixture]
    public class TestTypeNameParseTree
    {
        [Test]
        public void TestInvokeParse()
        {
            var testInput = "NeedItInIl.DomainAdapterBase`2[[AThirdDll.Whatever, AThirdDll, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null],[System.Tuple`3[[System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089],[System.Int32, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089],[System.Collections.Generic.IEnumerable`1[[MoreBinaries.DomainObject+MyInnerType, MoreBinaries, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]], mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]], mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]";
            var testResult = TypeNameParseTree.InvokeParse(testInput);

            Assert.IsNotNull(testResult);
            Assert.IsTrue(testResult.Any());

            foreach (var tr in testResult)
            {
                PrintTypeItemContent(tr, 0);
            }
        }

        [Test]
        public void TestInvokeParseSimpleGeneric()
        {
            var testInput = "System.Collections.Generic.List`1[SomeSecondDll.MyFirstMiddleClass]";
            var testResult = TypeNameParseTree.InvokeParse(testInput);

            Assert.IsNotNull(testResult);
            Assert.IsTrue(testResult.Any());

            foreach (var tr in testResult)
            {
                PrintTypeItemContent(tr, 0);
            }

        }

        [Test]
        public void TestInvokeParseAsmNameOnly()
        {
            var testInput =
                "log4net, Version=1.2.13.0, Culture=neutral, PublicKeyToken=669e0ddf0bb1aa2a, processorArchitecture=MSIL";
            var testResult = TypeNameParseTree.InvokeParse(testInput);

            Assert.IsNotNull(testResult);
            Assert.IsTrue(testResult.Any());

            foreach (var tr in testResult)
            {
                PrintTypeItemContent(tr, 0);
            }
        }

        [Test]
        public void TestInvokeParseSimpleName()
        {
            var testInput = "NoFuture.Util.TypeName";
            var testResult = TypeNameParseTree.InvokeParse(testInput);

            Assert.IsNotNull(testResult);
            Assert.IsTrue(testResult.Any());

            foreach (var tr in testResult)
            {
                PrintTypeItemContent(tr, 0);
            }
        }

        internal static void PrintTypeItemContent(NfTypeNameParseItem item, int count)
        {
            Console.WriteLine($"{new string('\t', count)}{item.FullName}");
            Console.WriteLine($"{new string('\t', count)}{item.AssemblyFullName}");
            Console.WriteLine($"{new string('\t', count)}{item.GenericCounter}");
            if (item.GenericItems == null)
                return;
            foreach (var typeNameParseItem in item.GenericItems)
            {
                PrintTypeItemContent(typeNameParseItem, count + 1);
            }

        }
    }
}
