using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Domus.Opes
{
    [Serializable]
    public class NorthAmericanIncome : IReditus
    {
        private readonly HashSet<NorthAmericanEmployment> _employment = new HashSet<NorthAmericanEmployment>();
        protected internal IComparer<DiachronIdentifier> Comparer { get; } = new TemporeComparer();

        protected internal List<NorthAmericanEmployment> Employment
        {
            get
            {
                var e = _employment.ToList();
                e.Sort(Comparer);
                return e;
            }
        }

        public IEmployment[] GetEmployment(DateTime? dt)
        {
            return dt == null
                ? new[] {(IEmployment)Employment.LastOrDefault()}
                : Employment.Where(x => x.IsInRange(dt.Value)).Cast<IEmployment>().ToArray();
        }

        protected internal void AddEmployment()
        {
            
        }
    }
}
