using System;
using NoFuture.Law.Property.US.FormsOf.Intellectus;
using NoFuture.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Law.Property.Tests
{
    /// <summary>
    /// Qualitex Co. v. Jacobson Products Co., 514 U.S. 159 (1995)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, trademark is to distinguish product source
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class QualitexCovJacobsonProductsCoTests
    {
        [Test]
        public void QualitexCovJacobsonProductsCo()
        {
            var test = new GreenGoldColoredCleaningPads();
            test.IsSecondaryMeaning = true;
            Assert.IsTrue(test.IsValid(new QualitexCo(), new JacobsonProductsCo()));
            Console.WriteLine(test.ToString());
        }
    }

    public class GreenGoldColoredCleaningPads : Trademark
    {
        public GreenGoldColoredCleaningPads() : base("special shade of green-gold color on the pads") { }
        public Tuple<byte, byte, byte> RGB { get; } = new Tuple<byte, byte, byte>(151,153,72);
    }

    public class QualitexCo : LegalPerson, IPlaintiff
    {
        public QualitexCo(): base("Qualitex Co.") { }
    }

    public class JacobsonProductsCo : LegalPerson, IDefendant
    {
        public JacobsonProductsCo(): base("Jacobson Products Co.") { }
    }
}
