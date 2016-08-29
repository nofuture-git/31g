using System;
using System.Collections.Generic;
using System.Linq;

namespace NoFuture.Rand.Data.Sp
{
    [Serializable]
    public class Security : Pecuniam, ITransactionable, IAsset
    {
        #region fields
        protected readonly Balance Balance = new Balance();
        private readonly Dictionary<Guid, Decimal> _trans2Qty = new Dictionary<Guid, decimal>();
        #endregion

        #region ctor
        public Security(string cusip):base(Math.Abs(1M))
        {
            Id = new Cusip { Value = cusip };
        }

        public Security(Cusip id) : base(Math.Abs(1M))
        {
            Id = id;
        }
        #endregion

        #region properties
        public override Identifier Id { get; }
        #endregion

        #region methods
        public virtual void Push(DateTime dt, Pecuniam buyPrice, Pecuniam fee = null, string note = null)
        {
            fee = fee ?? Pecuniam.Zero;
            var id = Balance.AddTransaction(dt, buyPrice.Neg, fee.Abs, note);
            _trans2Qty.Add(id, Amount);
        }

        public virtual bool Pop(DateTime dt, Pecuniam sellPrice, Pecuniam fee = null, string note = null)
        {
            fee = fee ?? Pecuniam.Zero;
            var id = Balance.AddTransaction(dt, sellPrice.Abs, fee.Abs, note);
            _trans2Qty.Add(id, -1*Amount);
            return true;
        }
        #endregion

        public Pecuniam CurrentValue => Balance.GetCurrent(DateTime.Now, 0.0F);

        public Pecuniam GetValueAt(DateTime dt)
        {
            if (Balance.IsEmpty)
                return Pecuniam.Zero;
            var fstDt = Balance.First;
            if (fstDt.AtTime > dt)
                return Pecuniam.Zero;
            var ts = Balance.GetTransactionsBetween(fstDt.AtTime, dt, true);

            var pl = ts.Sum(x => x.Cash.Amount);
            var fees = ts.Sum(x => x.Fee.Amount);

            return (pl - fees).ToPecuniam();
        }

        public SpStatus GetStatus(DateTime? dt)
        {
            dt = dt ?? DateTime.Now;
            if(Balance.IsEmpty)
                return SpStatus.NoHistory;
            var fstDt = Balance.First;
            if(fstDt.AtTime > dt)
                return SpStatus.NoHistory;

            var ts = Balance.GetTransactionsBetween(fstDt.AtTime, dt.Value, true);
            var tSum = 0M;
            foreach (var t in ts)
            {
                if (_trans2Qty.ContainsKey(t.UniqueId))
                    tSum += _trans2Qty[t.UniqueId];
            }

            if(tSum < 0)
                return SpStatus.Short;
            if(tSum > 0)
                return SpStatus.Current;
            return SpStatus.Closed;
        }

        
    }
}
