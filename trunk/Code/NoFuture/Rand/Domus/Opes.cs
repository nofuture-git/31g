using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Data.Sp;

namespace NoFuture.Rand.Domus
{
    public abstract class Opes
    {
        public IList<SavingsAccount> SavingAccounts { get; } = new List<SavingsAccount>();
        public IList<CheckingAccount> CheckingAccounts { get; } = new List<CheckingAccount>();

        public IList<IReceivable> HomeDebt { get; } = new List<IReceivable>();
        public IList<IReceivable> VehicleDebt { get; } = new List<IReceivable>();
        public IList<IReceivable> CreditCardDebt { get; } = new List<IReceivable>();

        public Pecuniam GetTotalCurrentCcDebt()
        {
            var dk = new Pecuniam(0.0M);
            foreach (var cc in CreditCardDebt.Cast<CreditCardAccount>())
            {
                dk += cc.GetCurrentBalance(DateTime.Now);
            }
            return dk;
        }
    }
}
