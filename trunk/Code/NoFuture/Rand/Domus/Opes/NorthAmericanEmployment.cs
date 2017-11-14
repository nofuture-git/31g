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
        protected internal IComparer<Pondus> Comparer { get; } = new ITemporeComparer();
        private readonly List<Pondus> _pay = new List<Pondus>();

        public NorthAmericanEmployment() {}
        public NorthAmericanEmployment(DateTime? startDate, DateTime? endDate) :base (startDate, endDate) { }

        public override string Abbrev => "Employer";
        public IFirm Biz { get; set; }
        public bool IsOwner { get; set; }
        public StandardOccupationalClassification Occupation { get; set; }
        public Pondus GetPayAt(DateTime? dt)
        {
            return dt == null
                ? CurrentPay
                : Pay.FirstOrDefault(x => x.IsInRange(dt.Value));
        }

        public Pondus CurrentPay => Pay.LastOrDefault();

        protected internal List<Pondus> Pay
        {
            get
            {
                _pay.Sort(Comparer);
                return _pay;
            }
        }
    }
}