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

        public static BankAccount GetRandomBankAccount(CityArea ca = null)
        {
            var format = Etx.GetRandomRChars(true);
            var accountId = new AccountId(format);

            FinancialFirm bank = null;

            var banks = TreeData.CommercialBankData;
            if (ca?.AddressData != null)
            {
                var stateCode = ca.AddressData.StateAbbrv;
                var cityName = ca.AddressData.City;

                var banksByState =
                    banks.Where(x => x.BusinessAddress?.Item2?.AddressData?.StateAbbrv == stateCode).ToArray();
                var banksByCityState =
                    banksByState.Where(x => x.BusinessAddress?.Item2?.AddressData?.City == cityName).ToArray();

                if (banksByCityState.Any())
                    banks = banksByCityState;
                else if (banksByState.Any())
                    banks = banksByState;
            }

            if (banks.Any())
            {
                var pickOne = Etx.IntNumber(0, banks.Length - 1);
                bank = banks[pickOne];
                bank.LoadXrefXmlData();
                if (bank.RoutingNumber == null)
                    bank.RoutingNumber = Gov.Fed.RoutingTransitNumber.RandomRoutingNumber();
            }

            return Etx.TryAboveOrAt(8, Etx.Dice.Ten)
                ? new Checking {AccountNumber = accountId, Bank = bank}
                : (BankAccount)new Savings { AccountNumber = accountId,  Bank = bank };
        }

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
    }

    [Serializable]
    public class Savings : BankAccount
    {
        public float InterestRate { get; set; }
        public override Pecuniam Value => Balance.GetCurrent(DateTime.Now, InterestRate);
    }
}
