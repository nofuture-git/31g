using System;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Gov.TheStates
{
    public class California : UsState
    {
        public California() : base("CA")
        {
            var dl = new Rchar[8];
            dl[0] = new RcharUAlpha(0);
            Array.Copy(Numerics(7,1), 0, dl, 1, 7);
            dlFormats = new[] { new DriversLicense(dl, this) };
        }
    }
}