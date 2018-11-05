using System;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Sp
{
    [Serializable]
    public class SavingsAccount : DepositAccount
    {
        #region ctor
        public SavingsAccount(DateTime dateOpenned) : base(dateOpenned){ }

        public SavingsAccount(string acctId, DateTime dateOpenned) : this(dateOpenned)
        {
            Id = new AccountId(acctId);
        }
        public SavingsAccount(Identifier acctId, DateTime dateOpenned) : this(dateOpenned)
        {
            Id = acctId;
        }
        #endregion

        #region properties
        public float InterestRate { get; set; }
        public override Pecuniam Value => Balance.GetCurrent(DateTime.Now, InterestRate);
        #endregion

        #region methods

        public override Pecuniam GetValueAt(DateTime dt)
        {
            return Balance.GetCurrent(dt, InterestRate);
        }


        #endregion
    }
}