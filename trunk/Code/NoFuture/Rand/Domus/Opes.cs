using System.Collections.Generic;
using NoFuture.Rand.Data.Sp;

namespace NoFuture.Rand.Domus
{
    public abstract class Opes
    {
        public IList<Savings> SavingAccounts { get; } = new List<Savings>();
        public IList<Checking> CheckingAccounts { get; } = new List<Checking>();

        public IList<IReceivable> HomeDebt { get; } = new List<IReceivable>();
        public IList<IReceivable> VehicleDebt { get; } = new List<IReceivable>();
        public IList<IReceivable> CreditCardDebt { get; } = new List<IReceivable>();
    }
}
