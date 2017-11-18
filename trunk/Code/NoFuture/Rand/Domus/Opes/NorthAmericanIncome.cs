using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Data.Sp;

namespace NoFuture.Rand.Domus.Opes
{
    /// <inheritdoc cref="IReditus" />
    /// <inheritdoc cref="WealthBase" />
    /// <summary>
    /// </summary>
    [Serializable]
    public class NorthAmericanIncome : WealthBase, IReditus
    {
        #region fields
        private readonly HashSet<IEmployment> _employment = new HashSet<IEmployment>();
        private readonly HashSet<Pondus> _otherIncome = new HashSet<Pondus>();
        private readonly HashSet<Pondus> _expenses = new HashSet<Pondus>();
        #endregion

        #region ctors

        public NorthAmericanIncome(NorthAmerican american, bool isRenting = false): base(american, isRenting)
        {
            
        }
        #endregion

        #region properties
        public virtual IEmployment[] CurrentEmployment
        {
            get
            {
                var e = Employment.Where(x => x.ToDate == null).ToList();
                e.Sort(Comparer);
                return e.ToArray();
            }
        }
        public virtual Pondus[] CurrentOtherIncome => GetCurrent(OtherIncome);
        public Pondus[] CurrentExpenses => GetCurrent(Expenses);
        public Pecuniam TotalExpenses => Pondus.GetSum(CurrentExpenses);
        public Pecuniam TotalIncome => Pondus.GetSum(_otherIncome) + CurrentEmployment.Select(e => e.CurrentNetPay).GetSum();

        protected internal virtual List<IEmployment> Employment
        {
            get
            {
                var e = _employment.ToList();
                e.Sort(Comparer);
                return e.ToList();
            }
        }

        protected internal virtual List<Pondus> OtherIncome
        {
            get
            {
                var o = _otherIncome.ToList();
                o.Sort(Comparer);
                return o.ToList();
            }
        }

        protected internal virtual List<Pondus> Expenses
        {
            get
            {
                var e = _expenses.ToList();
                e.Sort(Comparer);
                return e.ToList();
            }
        }

        #endregion

        #region methods
        public virtual IEmployment[] GetEmploymentAt(DateTime? dt)
        {
            return dt == null
                ? new[] {Employment.LastOrDefault()}
                : Employment.Where(x => x.IsInRange(dt.Value)).ToArray();
        }

        public virtual Pondus[] GetOtherIncomeAt(DateTime? dt)
        {
            return GetAt(dt, OtherIncome);
        }

        public Pondus[] GetExpensesAt(DateTime? dt)
        {
            return GetAt(dt, Expenses);
        }

        protected internal virtual void AddEmployment(IEmployment employment)
        {
            if (employment != null)
                _employment.Add(employment);
        }

        protected internal virtual void AddOtherIncome(Pondus otherIncome)
        {
            if (otherIncome != null)
                _otherIncome.Add(otherIncome);
        }

        protected internal virtual void AddOtherIncome(Pecuniam amt, string name, DateTime? startDate,
            DateTime? endDate = null)
        {
            AddOtherIncome(new Pondus(name)
            {
                Value = amt?.Neg,
                ToDate = endDate,
                FromDate = startDate
            });
        }

        protected internal virtual void AddExpense(Pondus expense)
        {
            if (expense == null)
                return;
            _expenses.Add(expense);
        }

        protected internal virtual void AddExpense(Pecuniam amt, string name, DateTime? startDate,
            DateTime? endDate = null)
        {
            AddExpense(new Pondus(name)
            {
                Value = amt?.Neg,
                ToDate = endDate,
                FromDate = startDate
            });
        }
        #endregion
    }

}
