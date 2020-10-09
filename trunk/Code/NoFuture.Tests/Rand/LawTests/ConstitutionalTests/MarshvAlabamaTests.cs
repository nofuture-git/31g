using System;
using System.Collections.Generic;
using NoFuture.Rand.Law.Constitutional.US;
using NoFuture.Rand.Law.Property.US.FormsOf.InTerra;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Constitutional.Tests
{
    /// <summary>
    /// Marsh v. Alabama 326 U.S. 501 (1946)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// Doctrine issue, property which acts as a public community is protected regardless of
    /// who has title
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class MarshvAlabamaTests
    {
        [Test]
        public void MarshvAlabama()
        {
            var testSubject = new StateAction()
            {
                Consent = Consent.NotGiven(),
                GetActByPerson = p =>
                {
                    if (p is Marsh)
                        return new DistributeReligiousLiterature()
                        {
                            IsAction = lp => true,
                            IsVoluntary = lp => true
                        };

                    return null;
                },
                SubjectProperty = new TownOfChickasaw(),
                IsProtectedRight = a => a is DistributeReligiousLiterature,
                IsPublicCommunity = t => t is TownOfChickasaw
            };

            var testResult = testSubject.IsValid(new Alabama(), new Marsh());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);
        }
    }

    public class Marsh : LegalPerson, IDefendant
    {
        public Marsh(): base("Marsh") { }
    }

    public class Alabama : LegalPerson, IPlaintiff
    {
        public Alabama(): base("Alabama") { }
    }

    public class TownOfChickasaw : RealProperty
    {
        
    }

    public class DistributeReligiousLiterature : Act
    {
        
    }
}
