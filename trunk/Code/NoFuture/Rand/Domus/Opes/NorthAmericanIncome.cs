using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Core;
using NoFuture.Rand.Data.Sp;

namespace NoFuture.Rand.Domus.Opes
{
    [Serializable]
    public class NorthAmericanIncome : IReditus
    {
        #region fields
        private readonly HashSet<IEmployment> _employment = new HashSet<IEmployment>();
        private readonly HashSet<Pondus> _otherIncome = new HashSet<Pondus>();
        private readonly HashSet<Pondus> _expenses = new HashSet<Pondus>();
        protected internal IComparer<ITempore> Comparer { get; } = new TemporeComparer();
        #endregion

        #region ctors

        public NorthAmericanIncome()
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
        public Pondus[] GetCurrentExpenses => GetCurrent(Expenses);

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

        protected internal virtual void AddOtherIncome(Pecuniam amt, IMereo description, DateTime? startDate,
            DateTime? endDate = null)
        {
            AddOtherIncome(new Pondus
            {
                Description = description,
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

        protected internal virtual void AddExpense(Pecuniam amt, IMereo description, DateTime? startDate,
            DateTime? endDate = null)
        {
            AddExpense(new Pondus
            {
                Description = description,
                Value = amt?.Neg,
                ToDate = endDate,
                FromDate = startDate
            });
        }

        protected internal virtual Pondus[] GetCurrent(List<Pondus> items)
        {
            if (items == null)
                return null;
            var o = items.Where(x => x.ToDate == null).ToList();
            o.Sort(Comparer);
            return o.ToArray();
        }

        protected internal virtual Pondus[] GetAt(DateTime? dt, List<Pondus> items)
        {
            if (items == null)
                return null;
            return dt == null
                ? new[] { items.LastOrDefault() }
                : items.Where(x => x.IsInRange(dt.Value)).ToArray();
        }

        #endregion
    }
}
