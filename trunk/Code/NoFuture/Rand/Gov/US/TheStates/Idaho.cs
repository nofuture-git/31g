using System;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Gov.US.TheStates
{
    public class Idaho : UsState
    {
        public Idaho() : base("ID")
        {
            var dl = new Rchar[9];
            dl[0] = new RcharUAlpha(0);
            dl[1] = new RcharUAlpha(1);
            Array.Copy(Numerics(6, 2), 0, dl, 2, 6);
            dl[8] = new RcharUAlpha(8);

            dlFormats = new[] { new DriversLicense(dl, this), new DriversLicense(Numerics(9), this) };
        }
    }
}