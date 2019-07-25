using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Law.Property.US.FormsOf;
using NoFuture.Rand.Law.Property.US.FormsOf.InTerra;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Property.Tests
{
    /// <summary>
    /// Wood v. Board of County Commissioners of Fremont County, 759 P.2d 1250 (Wyo. 1988)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// 
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class WoodvFremontCountyTests
    {
        [Test]
        public void WoodvFremontCounty()
        {
            var test = new PropertyInterestFactory(new SomeLandInWyoming(), ExtensionMethods.Defendant);

            

        }
    }

    public class SomeLandInWyoming : RealProperty
    {

    }

    public class Wood : LegalPerson, IPlaintiff
    {
        public Wood(): base("Cecil and Edna Wood") { }
    }

    public class FremontCounty : LegalPerson, IDefendant
    {
        public FremontCounty(): base("Board of County Commissioners of Fremont County") { }
    }
}
