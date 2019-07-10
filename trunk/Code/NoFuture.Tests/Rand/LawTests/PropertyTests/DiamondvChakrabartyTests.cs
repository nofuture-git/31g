using System;
using NoFuture.Rand.Law.Property.US.FormsOf.Intellectus;
using NoFuture.Rand.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Property.Tests
{
    /// <summary>
    /// Diamond v. Chakrabarty, 447 U.S. 303 (1980)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, being a alive does not preclude it from being patented
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class DiamondvChakrabartyTests
    {
        [Test]
        public void DiamondvChakrabarty()
        {
            var test = new PseudomonasBacterium()
            {
                IsSubjectToPatent = true,
                IsUseful = true,
                IsImplementable = true,
                IsAbstractIdea = false,
                IsLawOfNature = false,
                IsObviousIdea = false
            };
            var testResult = test.IsValid(new Diamond(), new Chakrabarty());
            Assert.IsTrue(testResult);
            Console.WriteLine(test.ToString());
        }
    }

    public class PseudomonasBacterium : UtilityPatent
    {

    }

    public class Diamond : LegalPerson, IPlaintiff
    {
        public Diamond(): base("Diamond") { }
    }

    public class Chakrabarty : LegalPerson, IDefendant
    {
        public Chakrabarty(): base("Chakrabarty") { }
    }
}
