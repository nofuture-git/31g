using System;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Gov.US.TheStates
{
    public class Arizona : UsState
    {
        public Arizona() : base("AZ")
        {
            var dl = new Rchar[9];
            dl[0] = new RcharLimited(0, 'A', 'B', 'D', 'Y');
            Array.Copy(Numerics(8,1), 0, dl, 1, 8);

            dlFormats = new[] { new DriversLicense(dl, this), new DriversLicense(Numerics(9), this) };
        }
    }
}