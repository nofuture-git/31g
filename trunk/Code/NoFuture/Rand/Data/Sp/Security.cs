using System;

namespace NoFuture.Rand.Data.Sp
{
    [Serializable]
    public abstract class Security : Pecuniam, IAccount<Identifier>
    {
        #region fields
        protected readonly TransactionHistory _history = new TransactionHistory();
        #endregion

        #region ctor
        protected Security(string cusip, DateTime? inceptionDate = null, decimal amount = 0M):base(amount)
        {
            Id = new Cusip { Value = cusip };
            Inception = inceptionDate.GetValueOrDefault(DateTime.Now);
        }

        protected Security(Cusip id, DateTime? inceptionDate = null, decimal amount = 0M) : base(amount)
        {
            Id = id;
            Inception = inceptionDate.GetValueOrDefault(DateTime.Now);
        }
        #endregion

        #region properties
        public override Identifier Id { get; }
        public virtual DateTime Inception { get; }
        public virtual DateTime? Terminus { get; set; }
        public virtual Pecuniam CurrentMarketValue => GetMarketValue(null);
        public virtual SpStatus CurrentStatus => GetStatus(null);
        #endregion

        #region methods
        public abstract SpStatus GetStatus(DateTime? dt);

        public abstract Pecuniam GetMarketValue(DateTime? dt);
        #endregion
    }
}
