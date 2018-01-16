using System;

namespace NoFuture.Rand.Gov.US.TheStates
{
    [Serializable]
    public class Delaware : UsState
    {
        public Delaware() : base("DE")
        {
            dlFormats = new[] {new DriversLicense(Numerics(7), this) };
        }
    }
}