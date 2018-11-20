using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Core;
using NoFuture.Rand.Sp.Enums;

namespace NoFuture.Rand.Sp
{
    /// <inheritdoc cref="IAsset" />
    /// <summary>
    /// Represents a tradable security of high liquidity
    /// </summary>
    [Serializable]
    public class Security : Pecuniam, IAsset
    {
        private readonly SortedDictionary<DateTime, Pecuniam> _historicData = new SortedDictionary<DateTime, Pecuniam>();
        #region ctor
        public Security(string cusip, decimal qty) :base(0.0M)
        {
            Quantity = (double) qty;
            Id = new Cusip { Value = cusip };
        }

        public Security(Cusip id, decimal qty) : base(0.0M)
        {
            Quantity = (double)qty;
            Id = id;
        }
        #endregion

        #region properties
        /// <summary>
        /// Gets the <see cref="Cusip"/> id of this security.
        /// </summary>
        public override Identifier Id { get; }
        public virtual double Quantity { get; }
        public virtual Pecuniam Value => GetValueAt(DateTime.UtcNow);
        #endregion

        public virtual void SetHistoricData(IEnumerable<Tuple<DateTime, decimal>> data, CurrencyAbbrev currency)
        {
            if (data == null || !data.Any())
                return;
            _historicData.Clear();
            foreach (var tuple in data)
            {
                if (_historicData.ContainsKey(tuple.Item1))
                    continue;
                _historicData.Add(tuple.Item1, new Pecuniam(tuple.Item2, currency));
            }
        }

        public virtual Pecuniam GetValueAt(DateTime dt)
        {
            if (!_historicData.Any())
                return Zero;
            if (_historicData.ContainsKey(dt))
                return _historicData[dt] ?? Zero;
            var firstPair = _historicData.First();

            if (dt < firstPair.Key)
                return Zero;

            var value = firstPair.Value;
            
            foreach (var date in _historicData.Keys)
            {
                if (date > dt)
                    return value;
                value = _historicData[date];
            }

            return Zero;
        }
    }
}
