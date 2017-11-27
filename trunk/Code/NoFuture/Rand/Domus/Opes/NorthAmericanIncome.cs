using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Core;
using NoFuture.Rand.Data.Endo.Enums;
using NoFuture.Rand.Data.Endo.Grps;
using NoFuture.Rand.Data.Sp;
using NoFuture.Rand.Domus.Pneuma;

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

        internal NorthAmericanIncome() : base(null)
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
        public Pecuniam TotalIncome => Pondus.GetSum(_otherIncome) + CurrentEmployment.Select(e => e.TotalNetPay).GetSum();

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

        /// <summary>
        /// Gets a list of time ranges over the last three years where each block is assumed as a 
        /// span of employment
        /// </summary>
        /// <param name="personality"></param>
        /// <returns></returns>
        protected internal virtual List<Tuple<DateTime, DateTime?>> GetEmploymentRanges(IPersonality personality)
        {
            var emply = new List<Tuple<DateTime, DateTime?>>();
            var sdt = Etx.Date(-3, null, true, 60).Date;
            if (personality == null)
            {
                emply.Add(new Tuple<DateTime, DateTime?>(sdt, null));
                return emply;
            }

            var lpDt = sdt;
            while (lpDt < DateTime.Today)
            {

                var randDays = Etx.IntNumber(0, 21);
                lpDt = lpDt.AddMonths(3).AddDays(randDays);
                if (personality.GetRandomActsSpontaneous())
                {
                    emply.Add(new Tuple<DateTime, DateTime?>(sdt, lpDt < DateTime.Today ? new DateTime?(lpDt) : null));
                    sdt = lpDt;
                }
            }
            if(!emply.Any())
                emply.Add(new Tuple<DateTime, DateTime?>(Etx.Date(-3, null, true, 60).Date, null));
            return emply;
        }

        /// <summary>
        /// Get an ordered list of employment for the last three years at random
        /// </summary>
        /// <param name="personality"></param>
        /// <param name="eduLevel"></param>
        /// <returns></returns>
        protected internal virtual List<IEmployment> ResolveEmployment(IPersonality personality,
            OccidentalEdu eduLevel = OccidentalEdu.None)
        {
            var empls = new HashSet<IEmployment>();
            var emplyRanges = GetEmploymentRanges(personality);
            var occ = StandardOccupationalClassification.RandomOccupation();

            //limit result to those which match the edu level
            Predicate<SocDetailedOccupation> filter = s => true;
            if (eduLevel < (OccidentalEdu.Bachelor | OccidentalEdu.Grad))
                filter = s => !StandardOccupationalClassification.IsDegreeRequired(s);

            foreach (var range in emplyRanges)
            {
                var emply = new NorthAmericanEmployment(range.Item1, range.Item2, Person) {Occupation = occ};
                if (personality?.GetRandomActsSpontaneous() ?? false)
                {
                    occ = StandardOccupationalClassification.RandomOccupation(filter);
                }
                empls.Add(emply);
            }

            var e = empls.ToList();
            e.Sort(Comparer);
            return e;
        }

        #endregion
    }

}
