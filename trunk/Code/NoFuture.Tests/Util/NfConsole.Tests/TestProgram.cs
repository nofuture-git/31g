using System;
using System.Reflection;
using NUnit.Framework;

namespace NoFuture.Util.NfConsole.Tests
{
    [TestFixture]
    public class TestProgram
    {
        [Test]
        public void TestGetBindingFlags()
        {
            var testResult =
                NoFuture.Util.NfConsole.Program.GetBindingFlags("DeclaredOnly,Instance,NonPublic,Public,Static");
            Assert.IsNotNull(testResult);
            Assert.AreEqual(BindingFlags.DeclaredOnly | BindingFlags.Instance |
                            BindingFlags.NonPublic |
                            BindingFlags.Public | BindingFlags.Static, testResult.Value);

        }

        [Test]
        public void TestResolveBool()
        {
            var testSubject = new TestProgramImpl(null, false);
            var testResult = testSubject.ResolveBool("false");
            Assert.IsNotNull(testResult);
            Assert.AreEqual(false, testResult.Value);

            testResult = testSubject.ResolveBool("true");
            Assert.IsNotNull(testResult);
            Assert.AreEqual(true, testResult.Value);
        }

        [Test]
        public void TestResolveInt()
        {
            var testSubject = new TestProgramImpl(null, false);
            var testResult = testSubject.ResolveInt("138");
            Assert.IsNotNull(testResult);
            Assert.AreEqual(138, testResult.Value);
        }
    }

    public class TestProgramImpl : NoFuture.Util.NfConsole.Program
    {
        public TestProgramImpl(string[] args, bool isVisable) : base(args, isVisable)
        {
        }

        protected override string MyName { get; }
        protected override string GetHelpText()
        {
            throw new NotImplementedException();
        }

        protected override void ParseProgramArgs()
        {
            throw new NotImplementedException();
        }
    }
}
