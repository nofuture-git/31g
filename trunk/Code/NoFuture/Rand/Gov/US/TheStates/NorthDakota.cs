using System;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Gov.US.TheStates
{
    [Serializable]
    public class NorthDakota : UsState
    {
        public NorthDakota() : base("ND")
        {
            var dl = new Rchar[9];
            dl[0] = new RcharUAlpha(0);
            dl[1] = new RcharUAlpha(1);
            dl[2] = new RcharUAlpha(2);
            Array.Copy(Numerics(6,3),0,dl,3,6);

            dlFormats = new[] { new DriversLicense(dl, this), new DriversLicense(Numerics(9), this) };
        }
    }
}