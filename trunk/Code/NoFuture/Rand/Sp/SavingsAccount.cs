using System;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Sp
{
    [Serializable]
    public class SavingsAccount : DepositAccount
    {
        public SavingsAccount(string acctId, DateTime dateOpened) : this(new AccountId(acctId),dateOpened)
        {
        }
        public SavingsAccount(Identifier acctId, DateTime dateOpened) : base(acctId, dateOpened)
        {
        }
        public float InterestRate { get; set; }
        public override Pecuniam Value => Balance.GetCurrent(DateTime.UtcNow, InterestRate);

        public override Pecuniam GetValueAt(DateTime dt)
        {
            return Balance.GetCurrent(dt, InterestRate);
        }
    }
}