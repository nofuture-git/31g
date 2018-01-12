using System.Collections.Generic;

namespace NoFuture.Rand.Gov.US
{
    public static class AmericanData
    {
        #region constants

        /// <summary>
        /// Src https://data.worldbank.org/indicator/NY.GDP.MKTP.KD 1960-2016
        /// </summary>
        public const double AVG_GDP_GROWTH_RATE = 0.031046655;

        /// <summary>
        /// Has no stat validity - just a guess
        /// </summary>
        public const double PERCENT_DIVORCED = 0.44;

        /// <summary>
        /// Has no stat validity - just a guess
        /// </summary>
        public const int AVG_LENGTH_OF_MARRIAGE = 10;

        /// <summary>
        /// Has no stat validity - just a guess
        /// </summary>
        public const double YEARS_BEFORE_NEXT_MARRIAGE = 3.857;

        /// <summary>
        /// Has no stat validity - just a guess
        /// </summary>
        public const double PERCENT_UNMARRIED_WHOLE_LIFE = 0.054;

        /// <summary>
        /// [http://www.pewsocialtrends.org/2014/11/14/four-in-ten-couples-are-saying-i-do-again/st_2014-11-14_remarriage-02/]
        /// </summary>
        public const double PERCENT_OF_MEN_MARRIED_ONCE_NEVER_AGAIN = 0.29D;

        /// <summary>
        /// [http://www.pewsocialtrends.org/2014/11/14/four-in-ten-couples-are-saying-i-do-again/st_2014-11-14_remarriage-02/]
        /// </summary>
        public const double PERCENT_OF_WOMEN_MARRIED_ONCE_NEVER_AGAIN = 0.15D;

        /// <summary>
        /// [https://en.wikipedia.org/wiki/List_of_the_verified_oldest_men]
        /// Oldest american man was 114 years and 222 days - consider this six sigma from mean
        /// </summary>
        /// <remarks>
        /// (114.61 - 76.9) / 6
        /// </remarks>
        public const double STD_DEV_MALE_LIFE_EXPECTANCY = 6.285D;

        /// <summary>
        /// [https://en.wikipedia.org/wiki/List_of_the_verified_oldest_women]
        /// Oldest american woman was 119 years and 97 days - consider this six sigma from mean
        /// </summary>
        /// <remarks>
        /// (119.27 - 81.6) / 6
        /// </remarks>
        public const double STD_DEV_FEMALE_LIFE_EXPECTANCY = 6.27834D;

        /// <summary>
        /// Mean is from WHO [http://apps.who.int/gho/data/node.main.688?lang=en]
        /// </summary>
        public const double AVG_MAX_AGE_MALE = 76.9D;

        /// <summary>
        /// Mean is from WHO [http://apps.who.int/gho/data/node.main.688?lang=en]
        /// </summary>
        public const double AVG_MAX_AGE_FEMALE = 81.6;

        /// <summary>
        /// The male average max age plus the standard deviation times 3
        /// </summary>
        public const double MAX_AGE_MALE = AVG_MAX_AGE_MALE + STD_DEV_MALE_LIFE_EXPECTANCY * 3;


        /// <summary>
        /// The female average max age plus the standard deviation times 3
        /// </summary>
        public const double MAX_AGE_FEMALE = AVG_MAX_AGE_FEMALE + STD_DEV_FEMALE_LIFE_EXPECTANCY * 3;

        /// <summary>
        /// source seems questionable - does not cite its source
        /// https://www.mckinleyirvin.com/Family-Law-Blog/2012/October/32-Shocking-Divorce-Statistics.aspx
        /// </summary>
        public const int PERCENT_DIVORCED_CHILDREN_LIVE_WITH_MOTHER = 75;

        /// <summary>
        /// src [https://www.cdc.gov/nchs/data/nvsr/nvsr65/nvsr65_04.pdf] Table B.
        /// </summary>
        public const double PERCENT_ACCIDENT_DEATH = 0.05175;

        /// <summary>
        /// https://en.wikipedia.org/wiki/List_of_countries_by_median_age
        /// </summary>
        public const double AVG_AGE_AMERICAN = 37.9;

        /// <summary>
        /// Common knowledge value
        /// </summary>
        public const double AVG_AGE_CHILD_ENTER_SCHOOL = 5.0D;

        /// <summary>
        /// https://people.hofstra.edu/geotrans/eng/ch6en/conc6en/USAownershipcars.html
        /// </summary>
        public const double PERCENT_WITH_NO_CAR = 0.09D;

        /// <summary>
        /// https://www.zanebenefits.com/blog/what-percent-of-health-insurance-is-paid-by-employers
        /// </summary>
        public const double PERCENT_EMPLY_INS_COST_PAID_BY_EMPLOYER = 0.83D;

        /// <summary>
        /// https://www.irs.gov/taxtopics/tc751
        /// </summary>
        public const double FICA_DEDUCTION_TAX_RATE = 0.062D;

        /// <summary>
        /// https://www.irs.gov/taxtopics/tc751
        /// </summary>
        public const double MEDICARE_DEDUCTION_TAX_RATE = 0.0145D;

        /// <summary>
        /// https://www.cdc.gov/nchs/fastats/health-insurance.htm
        /// </summary>
        public const double PERCENT_AMERICANS_WITH_INSURANCE = 0.876D;

        /// <summary>
        /// BLS average 1985-2017 Series Id: CUUR0000SA0L1E
        /// https://data.bls.gov/pdq/SurveyOutputServlet
        /// </summary>
        public const double AVG_INFLATION_RATE = 0.027D;

        #endregion

        #region tables

        /// <summary>
        /// src [https://www.cdc.gov/nchs/data/nvsr/nvsr65/nvsr65_04.pdf] Table B.
        /// "National Vital Statistics Reports, Vol 65 No 4, June 30, 2016"
        /// [https://www.cdc.gov/nchs/fastats/homicide.htm] number of homicides (15872/2626418)
        /// </summary>
        public static Dictionary<AmericanDeathCert.MannerOfDeath, double> MannerOfDeathAvgs =
            new Dictionary<AmericanDeathCert.MannerOfDeath, double>
            {
                {AmericanDeathCert.MannerOfDeath.Accident, 5.175 },
                {AmericanDeathCert.MannerOfDeath.Suicide, 1.63 },
                {AmericanDeathCert.MannerOfDeath.Homicide, 0.604 },
                {AmericanDeathCert.MannerOfDeath.Natural, 92.591 }
            };

        #endregion
    }
}