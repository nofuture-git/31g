using System;
using NUnit.Framework;
using NoFuture.Rand.Law.US.Persons;
using NoFuture.Rand.Law.Tort.US.Elements;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Tort.Tests
{
    /// <summary>
    /// Li v. Yellow Cab Company of California, 532 P.2d 1226 (Cal. 1975)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, detailed explaination of comparative negligence
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class NgaLivYellowCabCompanyofCaliforniaTests
    {
        [Test]
        public void NgaLivYellowCabCompanyofCalifornia()
        {
            var test = new ComparativeNegligence<DrivingCar>(ExtensionMethods.Tortfeasor)
            {
                GetContribution = lp =>
                {
                    if (lp is NgaLi)
                        return new DrivingCar(1);
                    if (lp is YellowCabCompanyofCalifornia)
                        return new DrivingCar(2);
                    return new DrivingCar(0);
                }
            };

            var testResult = test.IsValid(new NgaLi(), new YellowCabCompanyofCalifornia());
            Assert.IsTrue(testResult);

            Console.WriteLine(test.ToString());
        }
    }

    public class NgaLi : LegalPerson, IPlaintiff
    {
        public NgaLi(): base("Nga Li") { }
    }

    public class YellowCabCompanyofCalifornia : LegalPerson, ITortfeasor
    {
        public YellowCabCompanyofCalifornia(): base("Yellow Cab Company of California") { }
    }

    public class DrivingCar : IRankable
    {
        private readonly int _rank;

        public DrivingCar(int rank)
        {
            _rank = rank;
        }

        public int GetRank()
        {
            return _rank;
        }
    }
}
