using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Com;
using NoFuture.Rand.Core;
using NoFuture.Rand.Data.Endo.Grps;

namespace NoFuture.Rand.Domus.Opes
{
    [Serializable]
    public class NorthAmericanEmployment : DiachronIdentifier,  IEmployment
    {
        protected internal IComparer<Pondus> Comparer { get; } = new TemporeComparer();
        private readonly HashSet<Pondus> _pay = new HashSet<Pondus>();
        private readonly HashSet<Pondus> _deductions = new HashSet<Pondus>();

        public NorthAmericanEmployment() {}
        public NorthAmericanEmployment(DateTime? startDate, DateTime? endDate) :base (startDate, endDate) { }

        public override string Abbrev => "Employer";
        public IFirm Biz { get; set; }
        public bool IsOwner { get; set; }
        public StandardOccupationalClassification Occupation { get; set; }
        public Pondus[] CurrentDeductions { get; }
        public Pondus CurrentPay => Pay.LastOrDefault();

        public Pondus GetPayAt(DateTime? dt)
        {
            return dt == null
                ? CurrentPay
                : Pay.FirstOrDefault(x => x.IsInRange(dt.Value));
        }

        public Pondus[] GetDeductionsAt(DateTime? dt)
        {
            throw new NotImplementedException();
        }

        protected internal List<Pondus> Deductions
        {
            get
            {
                var d = _deductions.ToList();
                d.Sort(Comparer);
                return d;
            }
        }

        protected internal List<Pondus> Pay
        {
            get
            {
                var p = _pay.ToList();
                p.Sort(Comparer);
                return p;
            }
        }
    }
}