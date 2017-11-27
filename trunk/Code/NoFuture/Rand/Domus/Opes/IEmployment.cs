using System;
using NoFuture.Rand.Com;
using NoFuture.Rand.Core;
using NoFuture.Rand.Data.Endo.Grps;
using NoFuture.Rand.Data.Sp;

namespace NoFuture.Rand.Domus.Opes
{
    /// <summary>
    /// Expresses a single form of personal employment in time
    /// </summary>
    public interface IEmployment : IIdentifier<IFirm>, ITempore
    {
        /// <summary>
        /// A flag to designate a person as the legal owner of the <see cref="IFirm"/>
        /// </summary>
        bool IsOwner { get; set; }

        /// <summary>
        /// The occupation associated to this term of employment
        /// </summary>
        SocDetailedOccupation Occupation { get; set; }

        /// <summary>
        /// List of the current deductions on this employment
        /// </summary>
        Pondus[] CurrentDeductions { get; }

        /// <summary>
        /// The current pay for this employment
        /// </summary>
        Pondus[] CurrentPay { get; }

        /// <summary>
        /// The pay as it was at time <see cref="dt"/>
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        Pondus[] GetPayAt(DateTime? dt);

        /// <summary>
        /// The list of deductions as it was at time <see cref="dt"/>
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        Pondus[] GetDeductionsAt(DateTime? dt);

        /// <summary>
        /// The monetary sum of all current income items
        /// </summary>
        Pecuniam TotalPay { get; }

        /// <summary>
        /// The monetary sum of all current deduction items
        /// </summary>
        Pecuniam TotalDeductions { get; }

        /// <summary>
        /// The monetary difference between pay and deductions
        /// </summary>
        Pecuniam TotalNetPay { get; }
    }
}