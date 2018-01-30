using System;
using NoFuture.Rand.Sp;

namespace NoFuture.Rand.Domus.Opes
{
    /// <summary>
    /// Expresses a personal expense in time
    /// </summary>
    public interface IExpense : IDeinde
    {

        /// <summary>
        /// Returns all current expenses right Now
        /// </summary>
        Pondus[] CurrentExpectedExpenses { get; }

        /// <summary>
        /// Returns all current expenses at the time of <see cref="dt"/>
        /// </summary>
        Pondus[] GetExpectedExpensesAt(DateTime? dt);

        /// <summary>
        /// A monetary sum of all current expenses
        /// </summary>
        Pecuniam TotalAnnualExpectedExpenses { get; }

    }
}
