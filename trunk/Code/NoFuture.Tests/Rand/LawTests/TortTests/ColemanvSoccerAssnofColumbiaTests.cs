using System;
using NUnit.Framework;
using NoFuture.Rand.Law.US.Persons;
using NoFuture.Rand.Law.Tort.US.Elements;
using NoFuture.Rand.Law.Tort.US.UnintentionalTort;
using NoFuture.Rand.Law.US.Property;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Tort.Tests
{
    /// <summary>
    /// Coleman v. Soccer Ass’n. of Columbia, 432 Md. 679 (2013).
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// 
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class ColemanvSoccerAssnofColumbiaTests
    {
        [Test]
        public void ColemanvSoccerAssnofColumbia()
        {
            var test = new ContributoryNegligence<SoccerFieldActivity>(ExtensionMethods.Tortfeasor)
            {
                GetContribution = lp =>
                {
                    if (lp is Coleman)
                        return new SoccerFieldActivity(1);
                    if (lp is SoccerAssnofColumbia)
                        return new SoccerFieldActivity(10);
                    return new SoccerFieldActivity(0);
                }
            };

            var testResult = test.IsValid(new Coleman(), new SoccerAssnofColumbia());
            Assert.IsFalse(testResult);
            Console.WriteLine(test.ToString());

        }
    }

    public class Coleman : LegalPerson, IPlaintiff
    {
        public Coleman(): base("Coleman") { }
    }

    public class SoccerAssnofColumbia : LegalPerson, ITortfeasor
    {
        public SoccerAssnofColumbia(): base("Soccer Ass’n. of Columbia") { }
    }

    public class SoccerFieldActivity : IRankable
    {
        private readonly int _rank;

        public SoccerFieldActivity(int rank)
        {
            _rank = rank;
        }

        public int GetRank()
        {
            return _rank;
        }
    }
}
