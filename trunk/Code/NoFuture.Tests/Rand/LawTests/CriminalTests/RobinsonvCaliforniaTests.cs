using System;
using NoFuture.Rand.Law;
using NoFuture.Rand.Law.US.Criminal;
using NUnit.Framework;

namespace NoFuture.Rand.Tests.LawTests.CriminalTests
{
    /// <summary>
    /// 370 U.S. 660 (1962) ROBINSON  v. CALIFORNIA. No. 554. Supreme Court of United States.  Argued April 17, 1962. Decided June 25, 1962.
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, the status of a person does not work as actus rea
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class RobinsonvCaliforniaTests
    {
        [Test]
        public void RobinsonvCalifornia()
        {
            var testSubject = new Felony();
            //court found that simply having a status is not actus rea
            testSubject.ActusReus.IsVoluntary = lp => false;

            var testResult = testSubject.IsValid(new Robinson());
            Console.WriteLine(testSubject.ToString());
            Assert.IsFalse(testResult);
        }
    }

    public class Robinson : LegalPerson
    {
        public Robinson():base("ROBINSON") { }
    }

    public class California : Government
    {

    }
}
