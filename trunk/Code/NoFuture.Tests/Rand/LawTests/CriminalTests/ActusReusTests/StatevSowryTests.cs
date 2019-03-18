using System;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Elements.Act;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.PenalCode;
using NoFuture.Rand.Law.US;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Criminal.Tests.ActusReusTests
{
    /// <summary>
    /// State v. Sowry, 155 Ohio App. 3d 742 (2004)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, one cannot act voluntarily while under the direct control of arresting officer
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class StatevSowryTests
    {
        [Test]
        public void StatevSowry()
        {
            var testSubject = new Felony
            {
                ActusReus = new ActusReus
                {
                    IsVoluntary = lp => false,
                    IsAction = lp => lp is Sowry,
                },
                MensRea = new Knowingly
                {
                    IsKnowledgeOfWrongdoing = lp => lp is Sowry,
                }
            };

            var testResult = testSubject.IsValid(new Sowry());
            Console.WriteLine(testSubject.ToString());
            Assert.IsFalse(testResult);
        }
    }

    public class Sowry : LegalPerson, IDefendant
    {

    }
}
