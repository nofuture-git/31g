using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Law.Property.US.Found;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Property.Tests
{
    /// <summary>
    /// Charrier v. Bell, 496 So. 2d 601 (La. App. 1 Cir. 1986)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, artifacts buired with dead are not relinquished
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class CharriervBellTests
    {
        [Test]
        public void CharriervBell()
        {
            var test = new AbandonedProperty(ExtensionMethods.Defendant)
            {
                SubjectProperty = new LegalProperty("artifacts on Trudeau Plantation"),
                Relinquishment = new Entombed()
            };

            var testResult = test.IsValid(new Charrier(), new Bell());
            Assert.IsFalse(testResult);
            Console.WriteLine(test.ToString());
        }
    }

    public class Entombed : Act
    {
        public override bool IsValid(params ILegalPerson[] persons)
        {
            AddReasonEntry("property buried with the dead is not relinquished");
            return false;
        }
    }

    public class Charrier : LegalPerson, IPlaintiff
    {
        public Charrier(): base("Charrier") { }
    }

    public class Bell : LegalPerson, IDefendant
    {
        public Bell(): base("Bell") { }
    }
}
