using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Law.Property.US.Acquisition.Donative;
using NoFuture.Rand.Law.Property.US.FormsOf;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Property.Tests
{
    /// <summary>
    /// Gruen v. Gruen, 68 N.Y.2d 48 (1986)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// 
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class GruensonvGruenstepmotherTests
    {
        [Test]
        public void GruensonvGruenstepmother()
        {
            var painting = new SchlossKammerAmAtterseeII()
            {
                EntitledTo = new GruenFather(),
            };
            var test = new InterVivos()
            {
                Offer = painting,
                Acceptance = p =>
                {
                    p.EntitledTo = new GruenSon();
                    return p;
                }
            };

            var testResult = test.IsValid(new GruenSon(), new GruenStepmother(), new GruenFather());
            Console.WriteLine(test.ToString());
            Assert.IsTrue(testResult);
        }
    }

    public class SchlossKammerAmAtterseeII : TangiblePersonalProperty
    {
        public SchlossKammerAmAtterseeII():base("“Schloss Kammer am Attersee II” by Gustav Klimt") { }
    }

    public class GruenSon : LegalPerson, IPlaintiff, IOfferee
    {
        public GruenSon(): base("Gruen (son)") { }
    }

    public class GruenFather : LegalPerson, IDefendant, IOfferor
    {
        public GruenFather() : base("Gruen (father) [DECEASED]") { }
    }

    public class GruenStepmother : LegalPerson, IDefendant
    {
        public GruenStepmother(): base("Gruen (step-mother)") { }
    }
}
