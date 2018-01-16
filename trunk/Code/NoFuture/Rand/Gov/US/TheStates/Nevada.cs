using System;

namespace NoFuture.Rand.Gov.US.TheStates
{
    [Serializable]
    public class Nevada : UsState
    {
        public Nevada() : base("NV")
        {
            dlFormats = new[] {new DriversLicense(Numerics(12), this), new DriversLicense(Numerics(10), this) };
        }
    }
}