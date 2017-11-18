using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Data.Sp;

namespace NoFuture.Rand.Domus.Opes
{
    /// <inheritdoc cref="IRebus" />
    /// <inheritdoc cref="WealthBase" />
    /// <summary>
    /// </summary>
    public class NorthAmericanAssets : WealthBase, IRebus
    {
        private readonly HashSet<Pondus> _assets = new HashSet<Pondus>();

        public NorthAmericanAssets(NorthAmerican american, bool isRenting = false) : base(american, isRenting)
        {
        }

        public Pondus[] CurrentAssets => GetCurrent(Assets);

        #region methods

        public Pondus[] GetAssetsAt(DateTime? dt)
        {
            return GetAt(dt, Assets);
        }

        protected virtual void AddAsset(Pondus asset)
        {
            if (asset == null)
                return;

            _assets.Add(asset);
        }

        protected virtual List<Pondus> Assets
        {
            get
            {
                var e = _assets.ToList();
                e.Sort(Comparer);
                return e.ToList();
            }
        }

        #endregion

    }
}
