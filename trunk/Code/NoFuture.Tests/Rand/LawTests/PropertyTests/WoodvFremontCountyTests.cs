using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Law.Property.US.FormsOf;
using NoFuture.Rand.Law.Property.US.FormsOf.InTerra;
using NoFuture.Rand.Law.Property.US.FormsOf.InTerra.Sequential;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Property.Tests
{
    /// <summary>
    /// Wood v. Board of County Commissioners of Fremont County, 759 P.2d 1250 (Wyo. 1988)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, illustrates fee simple subject to conditional subsequent and fee simple determinable
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class WoodvFremontCountyTests
    {
        [Test]
        public void WoodvFremontCounty()
        {
            var test00 = new FeeSimpleDeterminable(ExtensionMethods.Defendant)
            {
                IsSpecialLimitationPresent = lp => SomeLandInWyoming.WillExpireAutoIfNotUsedAsStated,
            };
            var testResult = test00.IsValid(new Wood(), new FremontCounty());
            Assert.IsFalse(testResult);
            Console.WriteLine(test00.ToString());

            var test01 = new FeeSimpleSubject2ConditionSubsequent(ExtensionMethods.Defendant)
            {
                IsRightOfEntry = lp => SomeLandInWyoming.IsStrictlyConditionalSubsequent
            };

            testResult = test01.IsValid(new Wood(), new FremontCounty());
            Assert.IsFalse(testResult);
            Console.WriteLine(test00.ToString());

        }
    }

    public class SomeLandInWyoming : RealProperty
    {
        public static bool WillExpireAutoIfNotUsedAsStated => false;
        public static bool IsStrictlyConditionalSubsequent => false;
    }

    public class Wood : LegalPerson, IPlaintiff
    {
        public Wood(): base("Cecil and Edna Wood") { }
    }

    public class FremontCounty : LegalPerson, IDefendant
    {
        public FremontCounty(): base("Board of County Commissioners of Fremont County") { }
    }
}
