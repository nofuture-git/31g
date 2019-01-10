﻿using System;
using NoFuture.Rand.Core;
using NoFuture.Rand.Law;
using NUnit.Framework;
using NoFuture.Rand.Law.US.Contracts.Ucc;

namespace NoFuture.Rand.Tests.LawTests.ContractTests.UccTests
{
    /// <summary>
    /// TOUSLEY-BIXLER CONSTRUCTION CO.  v.COLGATE ENTERPRISES, INC. Court of Appeals of Indiana 429 N.E.2d 979 (Ind.Ct.App. 1982)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// Doctrine, subtle concept of what is and is not goods according to UCC
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class TousleyBixlervColgateTests
    {
        [Test]
        public void TousleyBixlervColgate()
        {
            var testSubject =
                new UccContract<Goods> {SaleOf = new Order4EarthenClay(), Agreement = new PurchaseOrder50000CubicFeetClay()};
            var testResult = testSubject.IsValid(new Colgate(), new TousleyBixler());
            Assert.IsFalse(testResult);
            Console.WriteLine(testSubject.ToString());
        }
    }

    public class PurchaseOrder50000CubicFeetClay : Agreement
    {
        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            return (offeror is TousleyBixler || offeror is Colgate)
                   && (offeree is TousleyBixler || offeree is Colgate);
        }
    }

    public class Order4EarthenClay : GoodsInTerra
    {
        public int CubicFeet => 50000;
    }

    public class TousleyBixler : VocaBase, ILegalPerson
    {
        public TousleyBixler() : base("TOUSLEY-BIXLER CONSTRUCTION CO.") { }
    }

    public class Colgate : VocaBase, ILegalPerson
    {
        public Colgate() : base("COLGATE ENTERPRISES, INC.") { }
    }
}
