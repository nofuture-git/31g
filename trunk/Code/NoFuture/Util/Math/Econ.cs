using System;
using NoFuture.Shared;

namespace NoFuture.Util.Math
{
    public static class Econ
    {
        /// <summary>
        /// Calc's value after compound, per diem, interest.
        /// </summary>
        /// <param name="balance"></param>
        /// <param name="annualInterestRate"></param>
        /// <param name="numberOfDays"></param>
        /// <returns></returns>
        public static Decimal PerDiemInterest(this Decimal balance, float annualInterestRate, double numberOfDays)
        {
            var pa = Convert.ToDouble(balance);
            var freq = Constants.TropicalYear;
            var calc = System.Math.Round(pa *
                                         System.Math.Pow((1 + (annualInterestRate / freq.TotalDays)),numberOfDays), 2);
            return Convert.ToDecimal(calc);
        }
    }
}
