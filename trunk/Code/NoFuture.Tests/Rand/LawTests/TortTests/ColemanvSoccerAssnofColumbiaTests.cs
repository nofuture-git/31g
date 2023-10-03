using System;
using NUnit.Framework;
using NoFuture.Law.US.Persons;
using NoFuture.Law.Tort.US.UnintentionalTort;
using NoFuture.Law.US;

namespace NoFuture.Law.Tort.Tests
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
