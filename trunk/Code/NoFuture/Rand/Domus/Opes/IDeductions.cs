using System;
using NoFuture.Rand.Data.Sp;

namespace NoFuture.Rand.Domus.Opes
{
    /// <summary>
    /// Expresses a single form of deductions associated to an employment in time
    /// </summary>
    public interface IDeductions
    {
        /// <summary>
        /// List of the current deductions on this employment
        /// </summary>
        Pondus[] CurrentDeductions { get; }

        /// <summary>
        /// The monetary sum of all current deduction items
        /// </summary>
        Pecuniam TotalAnnualDeductions { get; }

        /// <summary>
        /// The list of deductions as it was at time <see cref="dt"/>
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        Pondus[] GetDeductionsAt(DateTime? dt);
    }
}
