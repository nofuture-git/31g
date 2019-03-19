using System;
using NoFuture.Rand.Law.Criminal.AgainstProperty.US;
using NoFuture.Rand.Law.Criminal.AgainstProperty.US.Elements;
using NoFuture.Rand.Law.Criminal.AgainstProperty.US.Elements.Theft;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.PenalCode;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Persons;
using NoFuture.Rand.Law.US.Property;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Criminal.Tests.TheftTests
{
    /// <summary>
    /// People v. Traster, 111 Cal. App. 4th 1377 (2003)
    /// </summary>
    /// <remarks>
    ///<![CDATA[
    /// doctrine issue, larceny by trick is when thief only gets possession, false pretense is when thief gets
    /// both possession and title
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class PeoplevTrasterTests
    {
        [Test]
        public void PeoplevTraster()
        {
            //sneaky bastard, a consultant, tricked companies into thinking
            // they needed MS licx, pretends to purchase them from phoney
            // supplier then fakes the any actual docx

            var testCriminalAct = new ByDeception
            {
                IsFalseImpression = lp => lp is Traster,
                IsTakenPossession = lp => lp is Traster,
                SubjectProperty = new MsLicx()
            };

            var testCrime = new Felony
            {
                ActusReus = testCriminalAct,
                MensRea = new Purposely
                {
                    IsIntentOnWrongdoing = lp => lp is Traster
                }
            };

            var reliance = new Reliance
            {
                IsReliantOnFalseRepresentation =
                    lp => lp is DemlerArmstrongAndRowlandLawFirm || lp is DemennoKerdoonCompany
            };

            testCrime.AttendantCircumstances.Add(reliance);
            var testResult = testCrime.IsValid(new Traster(), new DemlerArmstrongAndRowlandLawFirm(),
                new DemennoKerdoonCompany());
            Console.WriteLine(testCrime.ToString());
            Assert.IsTrue(testResult);

            //this is larceny by trick since the MS Licx titles where still due 
            Assert.IsTrue(testCriminalAct.IsLarcenyByTrick);
        }
    }

    public class MsLicx : PersonalProperty
    {

    }

    public class DemlerArmstrongAndRowlandLawFirm : LegalPerson, IVictim
    {
        public DemlerArmstrongAndRowlandLawFirm() : base("DEMLER, ARMSTRONG AND ROWLAND") { }
    }

    public class DemennoKerdoonCompany : LegalPerson, IVictim
    {
        public DemennoKerdoonCompany() : base("DEMENNO/KERDOON COMPANY") { }
    }

    public class Traster : LegalPerson, IDefendant
    {
        public Traster() : base("KEVIN D. TRASTER") {  }
    }
}
