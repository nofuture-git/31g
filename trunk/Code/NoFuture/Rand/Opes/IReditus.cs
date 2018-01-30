using System;
using NoFuture.Rand.Sp;

namespace NoFuture.Rand.Opes
{
    /// <summary>
    /// Expresses a personal income in time.
    /// Is Latin for revenue.
    /// </summary>
    public interface IReditus : IDeinde
    {
        /// <summary>
        /// Returns the <see cref="ILaboris"/> right Now
        /// </summary>
        /// <remarks>
        /// Its is an array since a person may have more than
        /// one job at a single time.
        /// </remarks>
        ILaboris[] CurrentEmployment { get; }

        /// <summary>
        /// Returns the <see cref="ILaboris"/> at the time of <see cref="dt"/>
        /// </summary>
        /// <param name="dt"></param>
        /// <returns>
        /// This returns an array since a person may have more than
        /// one job at a single time.
        /// </returns>
        ILaboris[] GetEmploymentAt(DateTime? dt);

        /// <summary>
        /// Returns all non-employment forms of income for the current time which have 
        /// a non-zero value
        /// </summary>
        Pondus[] CurrentExpectedOtherIncome { get; }

        /// <summary>
        /// Returns all non-employment forms of income at the time of <see cref="dt"/>
        /// </summary>
        Pondus[] GetExpectedOtherIncomeAt(DateTime? dt);

        /// <summary>
        /// A monetary sum of all income
        /// </summary>
        Pecuniam TotalAnnualExpectedIncome { get; }

        /// <summary>
        /// The monetary sum of just employment income less all employment deductions
        /// </summary>
        Pecuniam TotalAnnualExpectedNetEmploymentIncome { get; }

        /// <summary>
        /// The gross monetary sum of employment income 
        /// </summary>
        Pecuniam TotalAnnualExpectedGrossEmploymentIncome { get; }

        /// <summary>
        /// Adds an employment income to this object
        /// </summary>
        /// <param name="employment"></param>
        void AddEmployment(ILaboris employment);

    }
}
