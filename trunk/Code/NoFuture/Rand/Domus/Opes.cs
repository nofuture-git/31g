using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Data.Sp;

namespace NoFuture.Rand.Domus
{
    public abstract class Opes
    {
        public IList<DepositAccount> SavingAccounts { get; } = new List<DepositAccount>();
        public IList<DepositAccount> CheckingAccounts { get; } = new List<DepositAccount>();

        public IList<IReceivable> HomeDebt { get; } = new List<IReceivable>();
        public IList<IReceivable> VehicleDebt { get; } = new List<IReceivable>();
        public IList<IReceivable> CreditCardDebt { get; } = new List<IReceivable>();

        public FinancialData FinancialData => GetFinancialState();

        protected internal abstract FinancialData GetFinancialState(DateTime? dt = null);

        protected internal Pecuniam GetTotalCurrentCcDebt(DateTime? dt = null)
        {
            var dk = new Pecuniam(0.0M);
            foreach (var cc in CreditCardDebt.Cast<CreditCardAccount>())
            {
                dk += cc.GetCurrentBalance(dt.GetValueOrDefault(DateTime.Now)).Neg;
            }
            return dk;
        }

        protected internal Pecuniam GetTotalCurrentDebt(DateTime? dt = null)
        {
            var tlt = GetTotalCurrentCcDebt(dt).Neg;
            foreach (var hd in HomeDebt)
                tlt += hd.GetCurrentBalance(dt.GetValueOrDefault(DateTime.Now)).Neg;
            foreach (var vd in VehicleDebt)
                tlt += vd.GetCurrentBalance(dt.GetValueOrDefault(DateTime.Now)).Neg;
            return tlt;
        }

        protected internal abstract Pecuniam GetTotalCurrentWealth(DateTime? dt = null);
    }
}
