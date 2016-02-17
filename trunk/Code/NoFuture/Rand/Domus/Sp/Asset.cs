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
        Identifier Id { get; set; }
    }

    [Serializable]
    public abstract class Asset : IAsset
    {
        public virtual Pecuniam Value { get; set; }
        public virtual AssetOwnership Ownership { get; set; }
        public virtual Identifier Id { get; set; }
    }

}
