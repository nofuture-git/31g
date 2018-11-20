using NoFuture.Rand.Core;
using NoFuture.Rand.Org;
using NoFuture.Rand.Sp;

namespace NoFuture.Rand.Opes
{
    /// <inheritdoc cref="IDeinde"/>
    /// <summary>
    /// Expresses a single form of personal employment in time.
    /// Is Latin for work.
    /// </summary>
    public interface ILaboris : ITempore, IDeinde
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
        /// The monetary difference between pay and deductions
        /// </summary>
        Pecuniam TotalAnnualNetPay { get; }

        /// <summary>
        /// The deductions associated to this employment
        /// </summary>
        IDeinde Deductions { get; set; }

    }
}