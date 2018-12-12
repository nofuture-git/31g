using System;

namespace NoFuture.Util.Core.Math
{
    public static class Econ
    {

        public const double DBL_TROPICAL_YEAR = 365.24255;

        public static TimeSpan TropicalYear = new TimeSpan(365, 5, 49, 16, 320);
        /// <summary>
        /// Calc's value after compound, per diem, interest.
        /// </summary>
        /// <param name="balance"></param>
        /// <param name="annualInterestRate"></param>
        /// <param name="numberOfDays"></param>
        /// <param name="daysPerYear"></param>
        /// <returns></returns>
        public static decimal PerDiemInterest(this decimal balance, double annualInterestRate, double numberOfDays, double daysPerYear = DBL_TROPICAL_YEAR)
        {
            var pa = Convert.ToDouble(balance);
            return Convert.ToDecimal(PerDiemInterest(pa, annualInterestRate, numberOfDays, daysPerYear));
        }

        /// <summary>
        /// Calc's value after compound, per diem, interest.
        /// </summary>
        /// <param name="balance"></param>
        /// <param name="annualInterestRate"></param>
        /// <param name="numberOfDays"></param>
        /// <param name="daysPerYear"></param>
        /// <returns></returns>
        public static double PerDiemInterest(this double balance, double annualInterestRate, double numberOfDays, double daysPerYear = DBL_TROPICAL_YEAR)
        {
            var pa = balance;
            var calc = System.Math.Round(pa *
                                         System.Math.Pow(1 + annualInterestRate / daysPerYear, numberOfDays), 2);
            return calc;
        }
    }
}

