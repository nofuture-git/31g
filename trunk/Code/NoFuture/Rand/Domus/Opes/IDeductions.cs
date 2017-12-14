using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoFuture.Rand.Data.Sp;

namespace NoFuture.Rand.Domus.Opes
{
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
