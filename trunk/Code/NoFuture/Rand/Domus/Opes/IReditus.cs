﻿using System;
using NoFuture.Rand.Data.Sp;

namespace NoFuture.Rand.Domus.Opes
{
    /// <summary>
    /// Expresses a personal income in time
    /// </summary>
    public interface IReditus
    {
        /// <summary>
        /// Returns the <see cref="IEmployment"/> right Now
        /// </summary>
        /// <remarks>
        /// Its is an array since a person may have more than
        /// one job at a single time.
        /// </remarks>
        IEmployment[] CurrentEmployment { get; }

        /// <summary>
        /// Returns the <see cref="IEmployment"/> at the time of <see cref="dt"/>
        /// </summary>
        /// <param name="dt"></param>
        /// <returns>
        /// This returns an array since a person may have more than
        /// one job at a single time.
        /// </returns>
        IEmployment[] GetEmploymentAt(DateTime? dt);

        /// <summary>
        /// Returns all non-employment forms of income right Now
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
    }
}
