using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Law.Contract.US;
using NoFuture.Rand.Law.Contract.US.Defense.ToFormation;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Contract.Tests.DefenseTests
{
    /// <summary>
    /// EX PARTE ODEM Supreme Court of Alabama 537 So.2d 919 (Ala. 1988)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, capacity to contract limited when still a minor
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class ExParteOdemTests
    {
        [Test]
        public void ExParteOdem()
        {
            var testContract = new ComLawContract<Promise>
            {
                Offer = new RenderMedicalTreatment(),
                Acceptance = o => o is RenderMedicalTreatment ? new PayMedicalTreatment() : null,
                Assent = new MutualAssent
                {
                    IsApprovalExpressed = lp => true,
                    TermsOfAgreement = lp => GetTerms()
                },
            };
            testContract.Consideration = new Consideration<Promise>(testContract)
            {
                IsGivenByPromisee = (lp, p) => true,
                IsSoughtByPromisor = (lp, p) => true
            };
            var testSubject = new ByMinor<Promise>(testContract)
            {
                IsMinor = lp => lp is IrisOdem,
                IsDeclareVoid = lp => lp is IrisOdem
            };

            var testResult = testSubject.IsValid(new ChildrensHospital(), new IrisOdem());
            Assert.IsTrue(testResult);
            Console.WriteLine(testSubject.ToString());
        }
        public static ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new Term<object>("medical treatment", PayMedicalTreatment.TreatmentValue),
                new Term<object>("attorney fees", PayAttorneyFees.AttorneyFeesValue)
            };
        }
    }

    public class RenderMedicalTreatment : Promise
    {
        public override bool IsValid(params ILegalPerson[] persons)
        {
            var offeror = persons.FirstOrDefault();
            var offeree = persons.Skip(1).Take(1).FirstOrDefault();
            return offeror is ChildrensHospital && offeree is IrisOdem;
        }
    }

    public class PayMedicalTreatment : RenderMedicalTreatment
    {
        public static object TreatmentValue = new object();
    }

    public class PayAttorneyFees : PayMedicalTreatment
    {
        public static object AttorneyFeesValue = new object();
    }

    public class IrisOdem : LegalPerson
    {
        public IrisOdem(): base("Iris Odem") { }
    }

    public class ChildrensHospital : LegalPerson
    {
        public ChildrensHospital():base("Children's Hospital of Birmingham") { }
    }
}
