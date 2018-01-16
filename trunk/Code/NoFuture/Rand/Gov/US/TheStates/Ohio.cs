using System;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Gov.US.TheStates
{
    [Serializable]
    public class Ohio : UsState
    {
        public Ohio() : base("OH")
        {
            var dl = new Rchar[9];
            dl[0] = new RcharUAlpha(0);
            dl[1] = new RcharUAlpha(1);
            Array.Copy(Numerics(7, 2), 0, dl, 2, 7);
            dlFormats = new[] {new DriversLicense(dl, this) };
        }
    }
}