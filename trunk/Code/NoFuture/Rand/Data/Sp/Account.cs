﻿using System;
using NoFuture.Rand.Com;

namespace NoFuture.Rand.Data.Sp
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
    public abstract class BankAccount
    {
        public AccountId AccountNumber { get; set; }

        public IBalance Balance { get; set; }

        public FinancialFirm Bank { get; set; }

        public static BankAccount GetRandomBankAccount()
        {
            var format = Etx.GetRandomRChars(true);
            var accountId = new AccountId(format);

            FinancialFirm bank = null;

            var banks = Data.TreeData.CommercialBankData;
            if (banks != null && banks.Length > 0)
            {
                var pickOne = Etx.IntNumber(0, banks.Length - 1);
                bank = banks[pickOne];
                if (bank.RoutingNumber == null)
                    bank.RoutingNumber = Gov.Fed.RoutingTransitNumber.RandomRoutingNumber();;
            }

            var rand = Etx.IntNumber(1, 10);
            return rand >= 8
                ? (BankAccount) new Checking {AccountNumber = accountId, Bank = bank}
                : (BankAccount)new Savings { AccountNumber = accountId,  Bank = bank };
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