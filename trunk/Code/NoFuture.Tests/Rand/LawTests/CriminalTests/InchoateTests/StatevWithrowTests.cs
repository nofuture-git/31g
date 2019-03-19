using System;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Elements.Act;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.PenalCode;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Criminal.Tests.InchoateTests
{
    /// <summary>
    /// State v. Withrow, 8 S.W.3d 75 (1999)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, possession as actus reus failed because it was not a residence the defendant had sole control of
    /// ]]>
    /// </remarks>
    [TestFixture()]
    public class StatevWithrowTests
    {
        [Test]
        public void StatevWithrow()
        {
            var testCrime = new Felony
            {
                MensRea = new Purposely
                {
                    IsKnowledgeOfWrongdoing = lp => lp is Withrow,
                },
                ActusReus = new Possession()
            };

            var testResult = testCrime.IsValid(new Withrow());
            Console.WriteLine(testCrime.ToString());
            Assert.IsFalse(testResult);
        }
    }

    public class Withrow : LegalPerson, IDefendant
    {
        public Withrow() : base("MICHAEL R. WITHROW") { }
    }
}
