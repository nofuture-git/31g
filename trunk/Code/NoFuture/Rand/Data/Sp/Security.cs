using System;

namespace NoFuture.Rand.Data.Sp
{
    [Serializable]
    public class Security : IAsset, INumera
    {
        #region fields
        private readonly TransactionHistory _history = new TransactionHistory();
        private DateTime _expiry = DateTime.MaxValue;
        #endregion

        #region ctor
        public Security(string cusip, Decimal amount = 1M)
        {
            Id = new Cusip { Value = cusip };
            Amount = amount;
        }

        public Security(Cusip id, Decimal amount = 1M)
        {
            Id = id;
            Amount = amount;
        }
        #endregion

        #region properties
        public Decimal Amount { get; }
        public Identifier Id { get; }
        public DateTime Expiry { get { return _expiry;} set { _expiry = value; } }
        public Pecuniam CurrentMarketValue => GetMarketValue(null);
        public SpStatus CurrentStatus => GetStatus(null);
        #endregion

        public INumera Trade(INumera exchange, DateTime? dt)
        {
            throw new NotImplementedException();
        }

        public SpStatus GetStatus(DateTime? dt)
        {
            throw new NotImplementedException();
        }

        public Pecuniam GetMarketValue(DateTime? dt)
        {
            throw new NotImplementedException();
        }
    }
}
