using System;
using NoFuture.Rand.Law.Property.US.FormsOf;
using NoFuture.Rand.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Property.Tests
{
    /// <summary>
    /// Feist Publications, Inc. v. Rural Telephone Service Co., 499 U.S. 340 (1991)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, copyright requires creativity and originality, not a reward for artistic labour
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class FeistPublicationsIncvRuralTelephoneServiceCoTests
    {
        [Test]
        public void FeistPublicationsIncvRuralTelephoneServiceCo()
        {
            var test = new TelephoneDirectory()
            {
                IsMinimalCreative = false,
                IsOriginalExpression = true,
            };

            var testResult = test.IsValid(new FeistPublicationsInc(), new RuralTelephoneServiceCo());
            Assert.IsFalse(testResult);
            Console.WriteLine(test.ToString());
        }
    }

    public class TelephoneDirectory : Copyright
    {

    }

    public class FeistPublicationsInc : LegalPerson, IPlaintiff
    {
        public FeistPublicationsInc(): base("Feist Publications, Inc.") { }
    }

    public class RuralTelephoneServiceCo : LegalPerson, IDefendant
    {
        public RuralTelephoneServiceCo(): base("Rural Telephone Service Co.") { }
    }
}
