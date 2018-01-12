using System;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Gov.US.TheStates
{
    public class Indiana : UsState
    {
        public Indiana() : base("IN")
        {
            var dl = new Rchar[10];
            dl[0] = new RcharUAlpha(0);
            Array.Copy(Numerics(9, 1), 0, dl, 1, 9);

            dlFormats = new[] { new DriversLicense(dl, this), new DriversLicense(Numerics(10), this) };
        }
    }
}