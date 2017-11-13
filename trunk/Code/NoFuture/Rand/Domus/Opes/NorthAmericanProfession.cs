using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Domus.Opes
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
}
