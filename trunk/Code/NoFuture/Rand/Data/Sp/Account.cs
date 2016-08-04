using System;
using System.Linq;
using NoFuture.Rand.Com;
using NoFuture.Rand.Data.Types;

namespace NoFuture.Rand.Data.Sp
{
    [Serializable]
    public enum AccountStatus
    {
        Closed,
        Current,
        Late,
        NoHistory
    }
    [Serializable]
    public class AccountId : RIdentifier
    {
        public AccountId(Rchar[] format)
        {
            this.format = format;
        }

        public override string Abbrev => "Acct";
    }

    [Serializable]
    public abstract class BankAccount : IAsset
    {
        public AccountId AccountNumber { get; set; }

        public IBalance Balance { get; set; }

        public FinancialFirm Bank { get; set; }

        public override string ToString()
        {
            return string.Join(" ", GetType().Name, Bank, AccountNumber);
        }

        public abstract Pecuniam Value { get; }
    }

    [Serializable]
    public class Checking : BankAccount
    {
        public override Pecuniam Value => Balance.GetCurrent(DateTime.Now, 0F);

        public static Checking GetRandomCheckingAcct(CityArea ca)
        {
            var accountId = new AccountId(Etx.GetRandomRChars(true));
            var bank = Com.Bank.GetRandomBank(ca);
            return new Checking {AccountNumber = accountId, Bank = bank};
        }
    }

    [Serializable]
    public class Savings : BankAccount
    {
        public float InterestRate { get; set; }
        public override Pecuniam Value => Balance.GetCurrent(DateTime.Now, InterestRate);

        public static Savings GetRandomSavingAcct(CityArea ca)
        {
            var accountId = new AccountId(Etx.GetRandomRChars(true));
            var bank = Com.Bank.GetRandomBank(ca);
            return new Savings {AccountNumber = accountId, Bank = bank};
        }
    }
}
