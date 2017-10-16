using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Com;
using NoFuture.Rand.Data.Types;

namespace NoFuture.Rand.Domus
{
    public class NorthAmericanProfession : IProfession
    {
        private readonly List<NorthAmericanEmployment> _employment = new List<NorthAmericanEmployment>();
        protected internal IComparer<DiachronIdentifier> Comparer { get; } = new DiachronIdComparer();

        protected internal List<NorthAmericanEmployment> Employment
        {
            get
            {
                _employment.Sort(Comparer);
                return _employment;
            }
        }

        public IEmployment[] GetEmployment(DateTime? dt)
        {
            return dt == null
                ? new[] {(IEmployment)_employment.LastOrDefault()}
                : Employment.Where(x => x.IsInRange(dt.Value)).Cast<IEmployment>().ToArray();
        }
    }

    public class NorthAmericanEmployment : DiachronIdentifier,  IEmployment
    {
        public IFirm Biz { get; set; }
        public bool IsOwner { get; set; }
        public StandardOccupationalClassification Occupation { get; set; }

        public override string Abbrev => "Employer";
    }
}
