using System;
using NoFuture.Rand.Core;
using NoFuture.Shared.Core;
using NoFuture.Util.Core;
using NoFuture.Util.Core.Math;

namespace NoFuture.Rand.Gov.US
{

    /// <summary>
    /// Marriage Source [https://www.census.gov/population/socdemo/hh-fam/ms2.xls] (1947-2011)
    /// Age of Birth Sources (1970-2014)
    /// [http://www.cdc.gov/nchs/data/nvsr/nvsr51/nvsr51_01.pdf] (Table 1.) 
    /// [http://www.cdc.gov/nchs/data/nvsr/nvsr64/nvsr64_12_tables.pdf] (Table I-1.)
    /// </summary>
    /// <remarks>
    /// FemaleAge2*Child have had thier intercepts up&apos;ed by 4.  The real data produces 
    /// a condition where first born's are always before marriage.
    /// Any <see cref="RLinearEquation"/> have a random Standard Dev between 0 and 1.0
    /// </remarks>
    public static class AmericanEquations
    {

        internal const int MIN_DOB_YEAR = 1914;
        internal const int MAX_DOB_YEAR = 2055;

        /// <summary>
        /// The linear regressions are for the second half of 20th century and cannot 
        /// be applied to date ranges beyond <see cref="MIN_DOB_YEAR"/> and <see cref="MAX_DOB_YEAR"/>
        /// </summary>
        /// <param name="dob"></param>
        /// <returns></returns>
        public static DateTime ProtectAgainstDistantTimes(DateTime dob)
        {
            if (dob.Year < MIN_DOB_YEAR)
                return new DateTime(MIN_DOB_YEAR, dob.Month, dob.Day);
            if (dob.Year > MAX_DOB_YEAR)
                return new DateTime(MAX_DOB_YEAR, dob.Month, dob.Day);
            return dob;
        }

        /// <summary>
        /// SolveForY using partial year value (e.g. 12/14/1979 is 1979.952791508)
        /// </summary>
        public static IEquation MaleAge2FirstMarriage = new RLinearEquation(0.1056, -181.45)
        {
            StdDev = Etx.RandomDouble(0, 1)
        };

        /// <summary>
        /// SolveForY using partial year value (e.g. 6/15/1979 is 1979.45449250094)
        /// </summary>
        public static IEquation FemaleAge2FirstMarriage
        {
            get
            {
                Func<double, double> rLinear = (x) =>
                {
                    var v = new RLinearEquation(0.1187, -209.41)
                    {
                        StdDev = Etx.RandomDouble(0, 1)
                    };
                    return v.SolveForY(x);
                };
                return new CustomEquation(rLinear);
            }
        }

        /// <summary>
        /// SolveForY using partial year value (e.g. 11/14/1989 is 1989.87065430903)
        /// </summary>
        public static IEquation FemaleAge2FirstChild = new RLinearEquation(0.1026, -176.32)
        {
            StdDev = Etx.RandomDouble(0, 1)
        };

        /// <summary>
        /// SolveForY using partial year value (e.g. 11/14/1989 is 1989.87065430903)
        /// </summary>
        public static IEquation FemaleAge2SecondChild = new RLinearEquation(0.1017, -171.88)
        {
            StdDev = Etx.RandomDouble(0, 1)
        };

        /// <summary>
        /// SolveForY using partial year value (e.g. 11/14/1989 is 1989.87065430903)
        /// </summary>
        public static IEquation FemaleAge2ThirdChild = new RLinearEquation(0.0792, -125.45)
        {
            StdDev = Etx.RandomDouble(0, 1)
        };

        /// <summary>
        /// SolveForY using partial year value (e.g. 11/14/1989 is 1989.87065430903)
        /// </summary>
        public static IEquation FemaleAge2ForthChild = new RLinearEquation(0.0545, -74.855)
        {
            StdDev = Etx.RandomDouble(0, 1)
        };

        /// <summary>
        /// Has no stat validity - just a guess
        /// </summary>
        public static IEquation Age2ProbWidowed = new ExponentialEquation
        {
            ConstantValue = Math.Pow(10, -13),
            Power = 6.547
        };

        /// <summary>
        /// Loosely based on https://en.wikipedia.org/wiki/Childfree#Statistics_and_research
        /// </summary>
        public static IEquation FemaleYob2ProbChildless = new SinusoidEquation
            {
                Amplitude = 0.115D,
                CenterAxis = 0.092,
                Frequency = 0.0019D,
                Phase = 1D
        };

        /// <summary>
        /// Calculated as (&apos;National Health Expenditures (Amount in Billions)&apos; * 1000) / &apos;U.S. Population (Millions)) per year
        /// https://www.cms.gov/Research-Statistics-Data-and-Systems/Statistics-Trends-and-Reports/NationalHealthExpendData/Downloads/NHEGDP15.zip
        /// </summary>
        public static IEquation HealthInsuranceCostPerPerson = new SecondDegreePolynomial(3.8555, -15145.0D, 14873062.18D);

        /// <summary>
        /// https://www.irs.com/articles/2016-federal-tax-rates-personal-exemptions-and-standard-deductions
        /// </summary>
        public static IEquation FederalIncomeTaxRate = new SecondDegreePolynomial(-0.000000000004D, 0.0000021, 0.0813D);

        /// <summary>
        /// [http://www.hhs.gov/ash/oah/adolescent-health-topics/reproductive-health/teen-pregnancy/trends.html]
        /// </summary>
        /// <param name="race"></param>
        /// <returns></returns>
        public static IEquation GetProbTeenPregnancyByRace(NorthAmericanRace race)
        {
            switch (race)
            {
                case NorthAmericanRace.Black:
                    return new RLinearEquation(-0.0034, 6.8045);
                case NorthAmericanRace.Hispanic:
                    return new RLinearEquation(-0.0025, 5.1231);
            }
            return new RLinearEquation(-0.001, 2.1241);
        }

        /// <summary>
        /// Means are <see cref="AmericanData.AVG_MAX_AGE_MALE"/> and <see cref="AmericanData.AVG_MAX_AGE_FEMALE"/>
        /// StdDev are <see cref="AmericanData.STD_DEV_MALE_LIFE_EXPECTANCY"/> and <see cref="AmericanData.STD_DEV_FEMALE_LIFE_EXPECTANCY"/>
        /// </summary>
        /// <param name="mf"></param>
        /// <returns></returns>
        public static NormalDistEquation LifeExpectancy(string mf)
        {
            return string.Equals(mf, "Male", StringComparison.OrdinalIgnoreCase)
                ? new NormalDistEquation { Mean = AmericanData.AVG_MAX_AGE_MALE, StdDev = AmericanData.STD_DEV_MALE_LIFE_EXPECTANCY }
                : new NormalDistEquation { Mean = AmericanData.AVG_MAX_AGE_FEMALE, StdDev = AmericanData.STD_DEV_FEMALE_LIFE_EXPECTANCY };
        }

        /// <summary>
        /// The average slope and intercept of all states in US_States_Data.xml
        /// </summary>
        public static IEquation NatlAverageEarnings => UsStateData.GetStateData(null).AverageEarnings;

        /// <summary>
        /// This is an attempt to have a way to calculate the federal poverty level over a range of years.
        /// The returned equation is solved-for-Y by number of members in the household.
        /// https://aspe.hhs.gov/poverty-guidelines
        /// </summary>
        /// <param name="atDate"></param>
        /// <returns></returns>
        public static IEquation GetFederalPovertyLevel(DateTime? atDate)
        {
            var dt = atDate.GetValueOrDefault(DateTime.Today);
            var estSlope =
                new SecondDegreePolynomial(0.9229, -3610.7, 3532449.338).SolveForY(dt.ToDouble());

            return new LinearEquation(7880, estSlope);
        }

        /// <summary>
        /// Gets an adjusted amount compounded for inflation from the <see cref="baseYear"/>
        /// at the rate of <see cref="AmericanData.AVG_INFLATION_RATE"/>
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="baseYear"></param>
        /// <param name="atDate"></param>
        /// <returns></returns>
        public static double GetInflationAdjustedAmount(double amount, int baseYear, DateTime? atDate = null)
        {
            var dt = atDate.GetValueOrDefault(DateTime.Today);
            if (baseYear == dt.Year)
                return amount;
            var years = (dt - new DateTime(baseYear, dt.Month, dt.Day)).TotalDays / Constants.DBL_TROPICAL_YEAR;
            var inflationRate = years < 0 ? -1 * AmericanData.AVG_INFLATION_RATE : AmericanData.AVG_INFLATION_RATE;
            return Math.Round(amount * Math.Pow(1 + inflationRate, years), 2);
        }

        /// <summary>
        /// This is an assumed graph for the age of an american.
        /// Its asymetric where age 44 is 0.0, age 21 is 0.14 but 
        /// age 67 (being likewise 23 years difference from 44) is 0.36 - reaches 1.0 at 80
        /// </summary>
        public static IEquation ClassicHook = new ThirdDegreePolynomial(0.0000081, -0.0005986, 0.005986, 0.205625);

        /// <summary>
        /// Based on averages from four different states (viz. FL, NY, WA, KS)
        /// http://www.kscourts.org/Rules-procedures-forms/Child-support-guidelines/2012_new/CSG%20AO%20261%20Clean%20Version%20032612.pdf
        /// https://www.flsenate.gov/Laws/Statutes/2013/61.30
        /// https://www.courts.wa.gov/forms/documents/WSCSS_Schedule2015.pdf
        /// https://www.childsupport.ny.gov/dcse/pdfs/CSSA.pdf
        /// </summary>
        /// <param name="numberOfChildren"></param>
        /// <returns>
        /// Returns the equation to solve for monthly child support payments using monthly income.
        /// </returns>
        /// <remarks>
        /// Dollar amounts are ~2015 dollars.
        /// </remarks>
        /// <example>
        /// <![CDATA[
        /// var yearlyIncome = 65000D;
        /// var numberOfChildren = 2;
        /// var rslt = AmericanEquations.GetChildSupportMonthlyCostEquation(numberOfChildren).SolveForY(yearlyIncome/12)
        /// ]]>
        /// </example>
        public static IEquation GetChildSupportMonthlyCostEquation(int numberOfChildren)
        {
            switch (numberOfChildren)
            {
                case 1:
                    Func<double, double> v1 = (x) =>
                    {
                        var parabolic = new SecondDegreePolynomial(-0.0000036, 0.1804442, 49.7609758);
                        var linear = new LinearEquation(49.7609758, 0.090222203778836843);
                        //once the parabolic's derivative slope goes to 0, switch over to a linear equation
                        return x >= 25061.68 ? linear.SolveForY(x) : parabolic.SolveForY(x);
                    };
                    return new CustomEquation(v1);
                case 2:
                    Func<double, double> v2 = (x) =>
                    {
                        var parabolic = new SecondDegreePolynomial(-0.0000051, 0.2193660, 30.5587561);
                        var linear = new LinearEquation(30.5587561, 0.10968300301723154);
                        //once the parabolic's derivative slope goes to 0, switch over to a linear equation
                        return x >= 21506.47 ? linear.SolveForY(x) : parabolic.SolveForY(x);
                    };
                    return new CustomEquation(v2);
                case 3:
                    Func<double, double> v3 = (x) =>
                    {
                        var para = new SecondDegreePolynomial(-0.0000063, 0.2483648, 14.8337182);
                        var linear = new LinearEquation(14.8337182, 0.12418241298856655);
                        //once the parabolic's derivative slope goes to 0, switch over to a linear equation
                        return x >= 19711.49 ? linear.SolveForY(x) : para.SolveForY(x);
                    };
                    return new CustomEquation(v3);
                case 4:
                    Func<double, double> v4 = (x) =>
                    {
                        var para = new SecondDegreePolynomial(-0.0000069, 0.2637436, 1.7419081);
                        var linear = new LinearEquation(1.7419081, 0.13187176611276977);
                        //once the parabolic's derivative slope goes to 0, switch over to a linear equation
                        return x >= 19111.86 ? linear.SolveForY(x) : para.SolveForY(x);
                    };
                    return new CustomEquation(v4);
                default:
                    Func<double, double> v5 = (x) =>
                    {
                        var para = new SecondDegreePolynomial(-0.0000077, 0.2848716, -13.7394084);
                        var linear = new LinearEquation(-13.7394084, 0.14243576811963998);
                        //once the parabolic's derivative slope goes to 0, switch over to a linear equation
                        return x >= 18498.16 ? linear.SolveForY(x) : para.SolveForY(x);
                    };
                    return new CustomEquation(v5);
            }
        }
    }
}