using System;
using NUnit.Framework;
using NoFuture.Rand.Law.US.Persons;
using NoFuture.Rand.Law.Tort.US.Elements;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Tort.Tests
{
    /// <summary>
    /// Gorris v. Scott, [1874] L.R. 9 Ex. 125
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, demo of non-negligence when attempting to use a statute's disobedience completely out of context
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class GorrisvScottTests
    {
        [Test]
        public void GorrisvScott()
        {
            var test = new NegligenceByStatute(ExtensionMethods.Tortfeasor)
            {
                IsObeyStatute = lp => !(lp is Scott),
                IsGroupMemberOfStatuesProtection = lp => !(lp is Scott) && !(lp is Gorris)
            };

            var testResult = test.IsValid(new Gorris(), new Scott());
            Assert.IsFalse(testResult);

            Console.WriteLine(test.ToString());
        }
    }

    public class Gorris : LegalPerson, IPlaintiff
    {
        public Gorris(): base("Gorris") { }
    }

    public class Scott : LegalPerson, ITortfeasor
    {
        public Scott(): base("Scott") { }
    }
}
