using System;

namespace NoFuture.Rand.Domus.Sp
{
    [Serializable]
    public class AccountId : RIdentifier
    {
        public AccountId(Rchar[] format)
        {
            base.format = format;
        }

        public override string Abbrev
        {
            get { return "Acct"; }
        }
    }

    [Serializable]
    public abstract class BankAccount : Asset
    {
        public Gov.Fed.RoutingTransitNumber RoutingNumber { get; set; }

        public AccountId AccountNumber { get; set; }

        public IBalance Balance { get; set; }

        public static BankAccount GetRandomBankAccount()
        {
            var format = Etx.GetRandomRChars(true);
            var routingNumber = Gov.Fed.RoutingTransitNumber.RandomRoutingNumber();
            var accountId = new AccountId(format);

            var rand = Etx.IntNumber(1, 10);
            return rand >= 8
                ? (BankAccount) new Checking {AccountNumber = accountId, RoutingNumber = routingNumber}
                : (BankAccount) new Savings {AccountNumber = accountId, RoutingNumber = routingNumber};
        }
    }

    [Serializable]
    public class Checking : BankAccount { }

    [Serializable]
    public class Savings : BankAccount
    {
        public float InterestRate { get; set; }
    }
}
