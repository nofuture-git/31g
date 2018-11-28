using System;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Sp
{
    [Serializable]
    public class SavingsAccount : DepositAccount
    {
        #region ctor
        public SavingsAccount(string acctId, DateTime dateOpenned) : this(new AccountId(acctId),dateOpenned)
        {
        }
        public SavingsAccount(Identifier acctId, DateTime dateOpenned) : base(acctId, dateOpenned)
        {
        }
        #endregion

        #region properties
        public float InterestRate { get; set; }
        public override Pecuniam Value => Balance.GetCurrent(DateTime.UtcNow, InterestRate);
        #endregion

        #region methods

        public override Pecuniam GetValueAt(DateTime dt)
        {
            return Balance.GetCurrent(dt, InterestRate);
        }


        #endregion
    }
}