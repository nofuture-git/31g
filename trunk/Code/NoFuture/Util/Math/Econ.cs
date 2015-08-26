using System;

namespace NoFuture.Util.Math
{
    public class Econ
    {
        public static Decimal CompoundInterest(Decimal principalAmt, float annualInterestRate, int termInYears, TimeSpan? frequency)
        {
            var pa = System.Convert.ToDouble(principalAmt);
            var freq = frequency == null ? new TimeSpan(365, 0, 0, 0) : frequency.Value;
            var calc = System.Math.Round(pa *
                       System.Math.Pow((1 + (annualInterestRate / freq.TotalDays)), (freq.TotalDays * termInYears)), 2);

            return System.Convert.ToDecimal(calc);
        }
    }
}
