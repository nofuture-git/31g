using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Domus.Opes
{
    [Serializable]
    public class NorthAmericanIncome : IReditus
    {
        private readonly HashSet<IEmployment> _employment = new HashSet<IEmployment>();
        protected internal IComparer<IEmployment> Comparer { get; } = new TemporeComparer();

        protected internal List<IEmployment> Employment
        {
            get
            {
                var e = _employment.ToList();
                e.Sort(Comparer);
                return e.ToList();
            }
        }

        public IEmployment[] GetEmployment(DateTime? dt)
        {
            return dt == null
                ? new[] {Employment.LastOrDefault()}
                : Employment.Where(x => x.IsInRange(dt.Value)).ToArray();
        }

        protected internal void AddEmployment(IEmployment employment)
        {
            if (employment != null)
                _employment.Add(employment);
        }
    }
}
