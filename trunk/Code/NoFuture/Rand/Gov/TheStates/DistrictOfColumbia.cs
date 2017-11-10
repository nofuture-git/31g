using System;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Gov.TheStates
{
    public class DistrictOfColumbia : UsState
    {
        public DistrictOfColumbia() : base("DC")
        {
            var dl = new Rchar[10];
            dl[0] = new RcharUAlpha(0);
            dl[1] = new RcharUAlpha(1);
            Array.Copy(Numerics(8, 2), 0, dl, 2, 8);
            dlFormats = new[] { new DriversLicense(dl, this) };
        }
    }
}