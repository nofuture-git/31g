using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoFuture.Rand.Data.Sp
{

    [Serializable]
    public class Security : IAsset
    {
        #region fields
        private readonly TransactionHistory _history = new TransactionHistory();
        private DateTime _expiry = DateTime.MaxValue;
        #endregion

        #region ctor
        public Security(string cusip)
        {
            Id = new Cusip { Value = cusip };
        }

        public Security(Cusip id)
        {
            Id = id;
        }
        #endregion

        #region properties
        public Cusip Id { get; }
        public DateTime Expiry { get { return _expiry;} set { _expiry = value; } }
        public Pecuniam CurrentMarketValue => GetMarketValue(null);
        public SpStatus CurrentStatus => GetStatus(null);
        #endregion

        public void Buy(ITransaction t)
        {
            throw new NotImplementedException();
        }

        public void Sell(ITransaction t)
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
