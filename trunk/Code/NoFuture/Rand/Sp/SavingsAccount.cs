using System;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Sp
{
    [Serializable]
    public class SavingsAccount : DepositAccount
    {
        public SavingsAccount(string acctId, DateTime dateOpenned) : this(new AccountId(acctId),dateOpenned)
        {
        }
        public SavingsAccount(Identifier acctId, DateTime dateOpenned) : base(acctId, dateOpenned)
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