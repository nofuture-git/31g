using System;
using System.Collections.Generic;

namespace NoFuture.Rand.Data.Sp
{
    [Serializable]
    public abstract class Security : Pecuniam, ITransactionable
    {
        #region fields
        protected readonly Balance Balance = new Balance();
        private readonly Dictionary<Guid, Decimal> _trans2Qty;
        #endregion

        #region ctor
        protected Security(string cusip, decimal numOf):base(Math.Abs(numOf))
        {
            Id = new Cusip { Value = cusip };
        }

        protected Security(Cusip id, decimal numOf) : base(Math.Abs(numOf))
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
            var id = Balance.AddTransaction(dt, buyPrice.Neg, fee, note);
            _trans2Qty.Add(id, Amount);
        }

        public virtual bool Pop(DateTime dt, Pecuniam sellPrice, Pecuniam fee = null, string note = null)
        {
            var id = Balance.AddTransaction(dt, sellPrice.Abs, fee, note);
            _trans2Qty.Add(id, -1*Amount);
            return true;
        }
        #endregion
    }
}
