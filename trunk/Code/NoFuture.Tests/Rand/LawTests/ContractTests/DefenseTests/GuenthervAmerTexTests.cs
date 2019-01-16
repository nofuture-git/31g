﻿using System;
using System.Collections.Generic;
using NoFuture.Rand.Law;
using NoFuture.Rand.Law.US.Contracts;
using NoFuture.Rand.Law.US.Contracts.Defense;
using NUnit.Framework;

namespace NoFuture.Rand.Tests.LawTests.ContractTests.DefenseTests
{
    /// <summary>
    /// GUENTHER v. AMER-TEX CONSTRUCTION CO. Court of Civil Appeals of Texas, Third District, Austin 534 S.W.2d 396 (Tex.App.—Austin 1976)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, the writing was not sufficient, despite there being everything else being obvious
    /// ]]>
    /// </remarks>
    [TestFixture()]
    public class GuenthervAmerTexTests
    {
        [Test]
        public void GuenthervAmerTex()
        {
            var testContract = new BilateralContract
            {
                Offer = new LandNextToPottersCreekTx(),
                Acceptance = o => o is LandNextToPottersCreekTx ? new AcceptLandNextToPottersCreekTx() : null,
                MutualAssent = new MutualAssent
                {
                    IsApprovalExpressed = lp => true,
                    TermsOfAgreement = lp => GetTerms()
                }
            };

            testContract.Consideration = new Consideration<Promise>(testContract)
            {
                IsGivenByPromisee = (lp, p) => true,
                IsSoughtByPromisor = (lp, p) => true
            };

            var testSubject = new StatuteOfFrauds<Promise>(testContract)
            {
                IsSufficientWriting = c =>
                {
                    var o = c.Offer as LandNextToPottersCreekTx;
                    return o?.IsCourtAgreeCouldMapLandOnGround ?? false;
                },
                IsSigned = c => true
            };

            testSubject.Scope.IsExecutorsPersonalResources = c => true;

            var testResult = testSubject.IsValid(new Guenther(), new AmerTex());
            //the court concludes the map is too vague to be used to plot the ground
            Assert.IsTrue(testResult);
            Console.WriteLine(testSubject.ToString());
        }

        public static ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new Term<object>("land", DBNull.Value)
            };
        }
    }

    public class LandNextToPottersCreekTx : Promise
    {
        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            return (offeror is AmerTex || offeror is RosalieMcClure)
                   && (offeree is AmerTex || offeree is RosalieMcClure)
                ;
        }

        public bool IsCourtAgreeCouldMapLandOnGround => false;

        public string Name => "EXHIBIT 'A'";
    }
    
    public class AcceptLandNextToPottersCreekTx : LandNextToPottersCreekTx { }

    public class RosalieMcClure : LegalPerson
    {
        public RosalieMcClure(): this("Rosalie M. McClure") {}
        public RosalieMcClure(string name) : base(name) { }
        public virtual bool HasDied => true;
    }


    public class Guenther : RosalieMcClure
    {
        public Guenther() : base("Jack Guenther") { }
        public override bool HasDied => false;
        public virtual bool IsExecutor => true;
    }

    public class AmerTex : LegalPerson
    {
        public AmerTex() : base("Amer-Tex Construction Company") { }
    }
}
