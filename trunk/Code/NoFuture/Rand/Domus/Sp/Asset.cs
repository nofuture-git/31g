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
        public Gov.Fed.RoutingTransitNumber RoutingNumber { get; set; }
    }

    [Serializable]
    public class Checking : BankAccount { }

    [Serializable]
    public class Savings : BankAccount { }

    [Serializable]
    public class NonMonetary : Account
    {
        public string Description { get; set; }
    }
}
