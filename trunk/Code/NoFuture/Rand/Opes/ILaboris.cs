using System;
using NoFuture.Rand.Core;
using NoFuture.Rand.Org;
using NoFuture.Rand.Sp;

namespace NoFuture.Rand.Opes
{
    /// <summary>
    /// Expresses a single form of personal employment in time.
    /// Is Latin for work.
    /// </summary>
    public interface ILaboris : ITempore, IDeinde, IObviate
    {
        /// <summary>
        /// A flag to designate a person as the legal owner of the employing company.
        /// </summary>
        bool IsOwner { get; set; }

        /// <summary>
        /// The name of the company providing employment
        /// </summary>
        string EmployingCompanyName { get; set; }

        /// <summary>
        /// The day on which the employing company&apos;s fiscal year ends
        /// </summary>
        int FiscalYearEndDay { get; set; }

        /// <summary>
        /// The occupation associated to this term of employment
        /// </summary>
        SocDetailedOccupation Occupation { get; set; }

        /// <summary>
        /// The current pay for this employment
        /// </summary>
        NamedReceivable[] CurrentPay { get; }

        /// <summary>
        /// The pay as it was at time <see cref="dt"/>
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        NamedReceivable[] GetPayAt(DateTime? dt);

        /// <summary>
        /// The monetary sum of of total annual employment income
        /// </summary>
        Pecuniam TotalAnnualPay { get; }

        /// <summary>
        /// The monetary difference between pay and deductions
        /// </summary>
        Pecuniam TotalAnnualNetPay { get; }

        /// <summary>
        /// The deductions associated to this employment
        /// </summary>
        ITributum Deductions { get; set; }

    }
}