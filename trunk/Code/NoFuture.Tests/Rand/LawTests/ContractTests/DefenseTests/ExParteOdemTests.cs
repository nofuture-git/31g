﻿using System;
using System.Collections.Generic;
using NoFuture.Rand.Law;
using NoFuture.Rand.Law.US.Contracts;
using NoFuture.Rand.Law.US.Contracts.Defense;
using NUnit.Framework;

namespace NoFuture.Rand.Tests.LawTests.ContractTests.DefenseTests
{
    [TestFixture]
    public class ExParteOdemTests
    {
        [Test]
        public void ExParteOdem()
        {
            var testContract = new BilateralContract
            {
                Offer = new RenderMedicalTreatment(),
                Acceptance = o => o is RenderMedicalTreatment ? new PayMedicalTreatment() : null,
                MutualAssent = new MutualAssent
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
            var testSubject = new Voidable<Promise>(testContract)
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
        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
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
