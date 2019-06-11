using System;
using NUnit.Framework;
using NoFuture.Rand.Law.US.Persons;
using NoFuture.Rand.Law.Tort.US.Elements;
using NoFuture.Rand.Law.Tort.US.UnintentionalTort;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Tort.Tests
{
    /// <summary>
    /// Tedla v. Ellman, 19 N.E.2d 287 (N.Y. 1939)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, disobedience to a statue when doing so actually acheives its goal while obeying it does just the opposite
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class TedlavEllmanTests
    {
        [Test]
        public void TedlavEllman()
        {
            var test = new NegligenceByStatute(ExtensionMethods.Tortfeasor)
            {
                IsObeyStatute = lp => !(lp is Ellman),
                IsObedienceAddToDanger = lp => lp is Ellman
            };

            var testResult = test.IsValid(new Tedla(), new Ellman());
            Assert.IsFalse(testResult);

            Console.WriteLine(test.ToString());
        }
    }

    public class Tedla : LegalPerson, IPlaintiff
    {
        public Tedla(): base("Tedla") { }
    }

    public class Ellman : LegalPerson, ITortfeasor
    {
        public Ellman(): base("Ellman") { }
    }
}
