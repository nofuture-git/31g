using System;
using NUnit.Framework;
using NoFuture.Rand.Law.US.Persons;
using NoFuture.Rand.Law.Tort.US.Elements;
using NoFuture.Rand.Law.US.Property;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Tort.Tests
{
    /// <summary>
    /// Gortarez v. Smitty’s Super Valu, 680 P.2d 807 (Ariz. 1984)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// 
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class GortarezvSmittysSuperValuTests
    {
        [Test]
        public void GortarezvSmittysSuperValu()
        {
            
        }
    }

    public class AirFreshener : PersonalProperty
    {
        public override decimal? PropertyValue { get; set; } = 0.56m;
        public override ILegalPerson EntitledTo { get; set; } = new SmittysSuperValu();
        public override ILegalPerson InPossessionOf { get; set; } = new SmittysSuperValu();
    }

    public class Gortarez : LegalPerson, IPlaintiff
    {
        public Gortarez(): base("") { }
    }

    public class SmittysSuperValu : LegalPerson, IMerchant, ITortfeasor
    {
        public SmittysSuperValu(): base("") { }
    }
}
