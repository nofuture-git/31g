using System;
using NUnit.Framework;
using NoFuture.Law.US.Persons;
using NoFuture.Law.Tort.US.Elements;
using NoFuture.Law.Tort.US.IntentionalTort;
using NoFuture.Law.US;

namespace NoFuture.Law.Tort.Tests
{
    /// <summary>
    /// Fontainebleau Hotel Corp. v. Forty-Five Twenty-Five, Inc., 114 So. 2d 357 (Fla. App. 1959)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, private nuisance may be intentional but needs to invade a protected interest of which air and sun are not
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class FortyFiveTwentyFiveIncvFontainebleauHotelCorpTests
    {
        [Test]
        public void FortyFiveTwentyFiveIncvFontainebleauHotelCorp()
        {
            var rocHotel = new LegalProperty("Roc Hotel")
            {
                IsEntitledTo = lp => lp is FortyFiveTwentyFiveInc,
                IsInPossessionOf = lp => lp is FortyFiveTwentyFiveInc
            };
            var test = new PrivateNuisance(ExtensionMethods.Tortfeasor)
            {
                IsIntentionalInvasion = lp => lp is FontainebleauHotelCorp,
                IsInvasionOfProtectableInterest = lp => false,
                SubjectProperty = rocHotel,
                Consent = new Consent(ExtensionMethods.Tortfeasor)
                {
                    IsApprovalExpressed = lp => false,
                    IsCapableThereof = lp => true
                }
            };
            var testResult = test.IsValid(new FortyFiveTwentyFiveInc(), new FontainebleauHotelCorp());
            Assert.IsFalse(testResult);
            Console.WriteLine(test.ToString());
        }
    }

    public class FortyFiveTwentyFiveInc : LegalPerson, IPlaintiff
    {
        public FortyFiveTwentyFiveInc(): base("Forty-Five Twenty-Five, Inc.") { }
    }

    public class FontainebleauHotelCorp : LegalPerson, ITortfeasor
    {
        public FontainebleauHotelCorp(): base("Fontainebleau Hotel Corp.") { }
    }
}
