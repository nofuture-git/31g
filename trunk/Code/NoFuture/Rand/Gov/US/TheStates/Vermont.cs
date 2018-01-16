using System;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Gov.US.TheStates
{
    [Serializable]
    public class Vermont : UsState
    {
        public Vermont() : base("VT")
        {
            var dl = new Rchar[8];
            Array.Copy(Numerics(7),0,dl,0,7);
            dl[7] = new RcharLimited(7,'A');

            dlFormats = new[] {new DriversLicense(dl, this), new DriversLicense(Numerics(8), this) };
        }
    }
}