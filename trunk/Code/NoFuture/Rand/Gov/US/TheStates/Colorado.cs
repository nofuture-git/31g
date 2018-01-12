using System;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Gov.US.TheStates
{
    public class Colorado : UsState
    {
        public Colorado() : base("CO")
        {
            var dl = new Rchar[8];
            dl[0] = new RcharUAlpha(0);
            dl[1] = new RcharUAlpha(1);
            Array.Copy(Numerics(6, 2), 0, dl, 2, 6);

            dlFormats = new[] { new DriversLicense(dl, this), new DriversLicense(Numerics(9), this) };
        }
    }
}