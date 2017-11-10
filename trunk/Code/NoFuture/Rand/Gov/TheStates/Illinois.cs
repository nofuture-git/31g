using System;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Gov.TheStates
{
    public class Illinois : UsState
    {
        public Illinois() : base("IL")
        {
            var dl = new Rchar[12];
            dl[0] = new RcharUAlpha(0);
            Array.Copy(Numerics(11, 1), 0, dl, 1, 11);

            dlFormats = new[] { new DriversLicense(dl, this) };
        }
    }
}