using System;
using NoFuture.Law.Criminal.US;
using NoFuture.Law.Criminal.US.Elements.Act;
using NoFuture.Law.US;
using NoFuture.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Law.Criminal.Tests.ActusReusTests
{
    /// <summary>
    /// 392 U.S. 514 (1968) POWELL v. TEXAS. No. 405. Supreme Court of United States. Argued March 7, 1968. Decided June 17, 1968.
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, there is a difference between being high or drunk and being an addict or alcoholic
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class PowellvTexasTests
    {
        [Test]
        public void PowellvTexas()
        {
            var testSubject = new ActusReus();

            //being an alcoholic is not actus rea, drinking too much and getting drunk is
            testSubject.IsVoluntary = lp => lp is Powell;
            testSubject.IsAction = lp => lp is Powell;

            var testResult = testSubject.IsValid(new Powell());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);
        }
    }

    public class Powell : LegalPerson, IDefendant
    {
        public Powell() : base("POWELL") { }
    }
}
