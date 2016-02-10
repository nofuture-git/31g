using System;

namespace NoFuture.Rand.Domus.Sp
{
    [Serializable]
    public enum AssetOwnership
    {
        Individual,
        Joint
    }

    public interface IAsset
    {
        Pecuniam Value { get; set; }
        AssetOwnership Ownership { get; set; }
    }

    [Serializable]
    public abstract class Asset : IAsset
    {
        public Pecuniam Value { get; set; }
        public AssetOwnership Ownership { get; set; }
    }

    [Serializable]
    public abstract class Account : Asset
    {
        public Identifier Id { get; set; }
    }

    [Serializable]
    public abstract class BankAccount : Asset
    {
        private Gov.Fed.RoutingTransitNumber _routingNumber = Gov.Fed.RoutingTransitNumber.RandomRoutingNumber();

        private string _accountNumber = Etx.Chars(0x30, 0x39, Etx.Number(5, 12));

        public Gov.Fed.RoutingTransitNumber RoutingNumber
        {
            get { return _routingNumber; }
            set { _routingNumber = value; }
        }

        public string AccountNumber
        {
            get { return _accountNumber; }
            set { _accountNumber = value; }
        }

        public IBalance Balance { get; set; }
    }

    [Serializable]
    public class Checking : BankAccount { }

    [Serializable]
    public class Savings : BankAccount
    {
        public float InterestRate { get; set; }
    }

    [Serializable]
    public class NonMonetary : Account
    {
        public string Description { get; set; }
    }
}
