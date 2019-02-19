using System;
using NoFuture.Rand.Law.Criminal.US;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Criminal.Tests.ActusReusTests
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
            var testSubject = new Misdemeanor();

            //being an alchoholic is not actus rea, drinking too much and getting drunk is
            testSubject.ActusReus.IsVoluntary = lp => lp is Powell;
            testSubject.ActusReus.IsAction = lp => lp is Powell;

            var testResult = testSubject.ActusReus.IsValid(new Powell());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);
        }
    }

    public class Powell : LegalPerson
    {
        public Powell() : base("POWELL") { }
    }

    public class Texas : Government
    {

    }
}
