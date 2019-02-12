﻿using System;
using NoFuture.Rand.Law;
using NoFuture.Rand.Law.US.Criminal;
using NoFuture.Rand.Law.US.Criminal.Elements.Intent.ComLaw;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Tests.CriminalTests.MensReaTests
{
    /// <summary>
    /// HAROLD E. STAPLES, III, PETITIONER v. UNITED STATES No. 92-1441
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, not requiring mens rea is mostly for regulations with fines, not felonies
    /// ]]>
    /// </remarks>
    [TestFixture()]
    public class HaroldStaplesvUsTests
    {
        [Test]
        public void HaroldStaplesvUs()
        {
            var testSubject = new Felony();

            testSubject.ActusReus.IsAction = lp => lp is HaroldStaples;
            testSubject.ActusReus.IsVoluntary = lp => lp is HaroldStaples;

            //mens rea is not needed for Fed Statute intended to regulate
            //for example ownership of hand gernades was a crime without mens rea
            testSubject.MensRea = null;
            var testResult = testSubject.IsValid(new HaroldStaples());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);

            //for this case, gun ownership is not the same thing
            testSubject.MensRea = new GeneralIntent();
            testResult = testSubject.IsValid(new HaroldStaples());
            Console.WriteLine(testSubject.ToString());
            Assert.IsFalse(testResult);
        }
    }

    public class HaroldStaples : LegalPerson
    {
        public HaroldStaples(): base("HAROLD E. STAPLES") { }
        public bool IsPossessionOfAr15Rifle => true;
        public bool IsAr15RifleFullyAutoFire => true;
    }
}
