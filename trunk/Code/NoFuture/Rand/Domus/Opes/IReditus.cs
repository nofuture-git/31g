using System;
using NoFuture.Rand.Data.Sp;

namespace NoFuture.Rand.Domus.Opes
{
    public interface IReditus
    {
        IEmployment[] CurrentEmployment { get; }
        IEmployment[] GetEmploymentAt(DateTime? dt);

        Pondus[] GetOtherIncomeAt(DateTime? dt);
        Pondus[] CurrentOtherIncome { get; }

        Pondus[] GetExpensesAt(DateTime? dt);
        Pondus[] GetCurrentExpenses { get; }
    }
}
