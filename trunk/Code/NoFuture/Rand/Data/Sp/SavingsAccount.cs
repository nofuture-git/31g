using System;
using NoFuture.Rand.Core;
using NoFuture.Rand.Domus;

namespace NoFuture.Rand.Data.Sp
{
    [Serializable]
    public class SavingsAccount : DepositAccount
    {
        #region ctor
        public SavingsAccount(RIdentifier acctId, DateTime dateOpenned) : base(dateOpenned)
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

        public static SavingsAccount GetRandomSavingAcct(IPerson p, DateTime? dt = null)
        {
            var dtd = dt.GetValueOrDefault(DateTime.Now);
            var accountId = new AccountId(Etx.GetRandomRChars(true));
            var bank = Com.Bank.GetRandomBank(p?.Address?.HomeCityArea);
            return new SavingsAccount(accountId,dtd) {Bank = bank};
        }
        #endregion
    }
}