﻿using System;
using NUnit.Framework;
using NoFuture.Rand.Law.US.Persons;
using NoFuture.Rand.Law.Tort.US.Elements;
using NoFuture.Rand.Law.US.Property;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Tort.Tests
{
    /// <summary>
    /// Intel Corp. v. Hamidi, 71 P.3d 296 (Cal. 2003)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, its not trespass to personal property without damage or loss of possession
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class IntelCorpvHamidiTests
    {
        [Test]
        public void IntelCorpvHamidi()
        {
            var testSubject = new TrespassToChattels
            {
                Consent = new PlaintiffConsent
                {
                    IsApprovalExpressed = lp => false,
                    IsCapableThereof = lp => lp is IntelCorp
                },
                SubjectProperty = new PersonalProperty
                {
                    Name = "Intel Corp computer system",
                    EntitledTo = new IntelCorp(),
                    InPossessionOf = new IntelCorp()
                },
                PropertyDamage = new Damage(ExtensionMethods.Tortfeasor)
                {
                    IsImpair = lp => false,
                },
                IsCauseDispossession = lp => false
            };

            var testResult = testSubject.IsValid(new IntelCorp(), new Hamidi());
            Console.WriteLine(testSubject.ToString());
            Assert.IsFalse(testResult);
        }
    }

    public class IntelCorp : LegalPerson, IPlaintiff
    {

    }

    public class Hamidi : LegalPerson, ITortfeasor
    {

    }
}