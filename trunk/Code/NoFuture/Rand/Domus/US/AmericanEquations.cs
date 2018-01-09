using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Core;
using NoFuture.Rand.Data.Endo.Enums;
using NoFuture.Rand.Data.Sp;
using NoFuture.Shared.Core;
using NoFuture.Util.Core;
using NoFuture.Util.Core.Math;

namespace NoFuture.Rand.Domus
{

    /// <summary>
    /// Marriage Source [https://www.census.gov/population/socdemo/hh-fam/ms2.xls] (1947-2011)
    /// Age of Birth Sources (1970-2014)
    /// [http://www.cdc.gov/nchs/data/nvsr/nvsr51/nvsr51_01.pdf] (Table 1.) 
    /// [http://www.cdc.gov/nchs/data/nvsr/nvsr64/nvsr64_12_tables.pdf] (Table I-1.)
    /// </summary>
    /// <remarks>
    /// FemaleAge2*Child have had thier intercepts up'ed by 4.  The real data produces 
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
        internal static DateTime ProtectAgainstDistantTimes(DateTime dob)
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
        public static RLinearEquation MaleAge2FirstMarriage = new RLinearEquation
        {
            Intercept = -181.45,
            Slope = 0.1056,
            StdDev = Etx.RationalNumber(0, 1)
        };

        /// <summary>
        /// SolveForY using partial year value (e.g. 6/15/1979 is 1979.45449250094)
        /// </summary>
        public static RLinearEquation FemaleAge2FirstMarriage = new RLinearEquation
        {
            Intercept = -209.41,
            Slope = 0.1187,
            StdDev = Etx.RationalNumber(0, 1)
        };

        /// <summary>
        /// SolveForY using partial year value (e.g. 11/14/1989 is 1989.87065430903)
        /// </summary>
        public static RLinearEquation FemaleAge2FirstChild = new RLinearEquation
        {
            Intercept = -176.32,//-180.32
            Slope = 0.1026,
            StdDev = Etx.RationalNumber(0, 1)
        };

        /// <summary>
        /// SolveForY using partial year value (e.g. 11/14/1989 is 1989.87065430903)
        /// </summary>
        public static RLinearEquation FemaleAge2SecondChild = new RLinearEquation
        {
            Intercept = -171.88,//-175.88
            Slope = 0.1017,
            StdDev = Etx.RationalNumber(0, 1)
        };

        /// <summary>
        /// SolveForY using partial year value (e.g. 11/14/1989 is 1989.87065430903)
        /// </summary>
        public static RLinearEquation FemaleAge2ThirdChild = new RLinearEquation
        {
            Intercept = -125.45,//-129.45
            Slope = 0.0792,
            StdDev = Etx.RationalNumber(0, 1)
        };

        /// <summary>
        /// SolveForY using partial year value (e.g. 11/14/1989 is 1989.87065430903)
        /// </summary>
        public static RLinearEquation FemaleAge2ForthChild = new RLinearEquation
        {
            Intercept = -74.855,//-78.855
            Slope = 0.0545,
            StdDev = Etx.RationalNumber(0, 1)
        };

        /// <summary>
        /// Has no stat validity - just a guess
        /// </summary>
        public static ExponentialEquation Age2ProbWidowed = new ExponentialEquation
        {
            ConstantValue = Math.Pow(10, -13),
            Power = 6.547
        };

        /// <summary>
        /// https://en.wikipedia.org/wiki/Childfree#Statistics_and_research
        /// </summary>
        public static NaturalLogEquation FemaleYob2ProbChildless = new NaturalLogEquation
        {
            Intercept = -55.479,
            Slope = 7.336
        };

        /// <summary>
        /// Calculated as (&apos;National Health Expenditures (Amount in Billions)&apos; * 1000) / &apos;U.S. Population (Millions)) per year
        /// https://www.cms.gov/Research-Statistics-Data-and-Systems/Statistics-Trends-and-Reports/NationalHealthExpendData/Downloads/NHEGDP15.zip
        /// </summary>
        public static SecondDegreePolynomial HealthInsuranceCostPerPerson = new SecondDegreePolynomial
        {
            SecondCoefficient = 3.8555,
            Slope = -15145.0D,
            Intercept = 14873062.18D
        };

        /// <summary>
        /// https://www.irs.com/articles/2016-federal-tax-rates-personal-exemptions-and-standard-deductions
        /// </summary>
        public static SecondDegreePolynomial FederalIncomeTaxRate = new SecondDegreePolynomial
        {
            SecondCoefficient = -0.000000000004D,
            Slope = 0.0000021,
            Intercept = 0.0813D
        };

        /// <summary>
        /// [http://www.hhs.gov/ash/oah/adolescent-health-topics/reproductive-health/teen-pregnancy/trends.html]
        /// </summary>
        /// <param name="race"></param>
        /// <returns></returns>
        public static RLinearEquation GetProbTeenPregnancyByRace(NorthAmericanRace race)
        {
            switch (race)
            {
                case NorthAmericanRace.Black:
                    return new RLinearEquation
                    {
                        Intercept = 6.8045,
                        Slope = -0.0034,
                    };
                case NorthAmericanRace.Hispanic:
                    return new RLinearEquation
                    {
                        Intercept = 5.1231,
                        Slope = -0.0025,
                    };
            }
            return new RLinearEquation
            {
                Intercept = 2.1241,
                Slope = -0.001,
            };
        }

        /// <summary>
        /// Means are <see cref="AmericanData.AVG_MAX_AGE_MALE"/> and <see cref="AmericanData.AVG_MAX_AGE_FEMALE"/>
        /// StdDev are <see cref="AmericanData.STD_DEV_MALE_LIFE_EXPECTANCY"/> and <see cref="AmericanData.STD_DEV_FEMALE_LIFE_EXPECTANCY"/>
        /// </summary>
        /// <param name="mf"></param>
        /// <returns></returns>
        public static NormalDistEquation LifeExpectancy(Gender mf)
        {
            return mf == Gender.Male
                ? new NormalDistEquation { Mean = AmericanData.AVG_MAX_AGE_MALE, StdDev = AmericanData.STD_DEV_MALE_LIFE_EXPECTANCY }
                : new NormalDistEquation { Mean = AmericanData.AVG_MAX_AGE_FEMALE, StdDev = AmericanData.STD_DEV_FEMALE_LIFE_EXPECTANCY };
        }



        /// <summary>
        /// TODO, this is the value for OH, need to get an averages for all data
        /// </summary>
        public static LinearEquation NatlAverageEarnings = new LinearEquation(-2046735.65519574, 1042.04539007091);

        /// <summary>
        /// This is an attempt to have a way to calculate the federal poverty level over a range of years.
        /// The returned equation is solved-for-Y by number of members in the household.
        /// https://aspe.hhs.gov/poverty-guidelines
        /// </summary>
        /// <param name="atDate"></param>
        /// <returns></returns>
        public static LinearEquation GetFederalPovertyLevel(DateTime? atDate)
        {
            var dt = atDate.GetValueOrDefault(DateTime.Today);
            var estSlope =
                new SecondDegreePolynomial { Intercept = 3532449.338, Slope = -3610.7, SecondCoefficient = 0.9229 }
                .SolveForY(dt.ToDouble());

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
        public static ThirdDegreePolynomial ClassicHook = new ThirdDegreePolynomial
        {
            Intercept = 0.205625,
            Slope = 0.005986,
            SecondCoefficient = -0.0005986,
            ThirdCoefficient = 0.0000081
        };

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
        /// Dollar amounts are ~2015 dollars
        /// </remarks>
        public static SecondDegreePolynomial GetChildSupportMonthlyCostEquation(int numberOfChildren)
        {
            switch (numberOfChildren)
            {
                case 1:
                    return new SecondDegreePolynomial
                    {
                        SecondCoefficient = -0.0000036,
                        Slope = 0.1804442,
                        Intercept = 49.7609758
                    };
                case 2:
                    return new SecondDegreePolynomial
                    {
                        SecondCoefficient = -0.0000051,
                        Slope = 0.2193660,
                        Intercept = 30.5587561
                    };
                case 3:
                    return new SecondDegreePolynomial
                    {
                        SecondCoefficient = -0.0000063,
                        Slope = 0.2483648,
                        Intercept = 14.8337182,
                    };
                case 4:
                    return new SecondDegreePolynomial
                    {
                        SecondCoefficient = -0.0000069,
                        Slope = 0.2637436,
                        Intercept = 1.7419081
                    };
                default:
                    return new SecondDegreePolynomial
                    {
                        SecondCoefficient = -0.0000077,
                        Slope = 0.2848716,
                        Intercept = -13.7394084
                    };
            }
        }
    }
}