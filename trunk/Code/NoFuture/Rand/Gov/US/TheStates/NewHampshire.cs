using System;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Gov.US.TheStates
{
    public class NewHampshire : UsState
    {
        public NewHampshire() : base("NH")
        {
            var dl = new Rchar[10];
            dl[0] = new RcharNumeric(0);
            dl[1] = new RcharNumeric(1);
            dl[2] = new RcharUAlpha(2);
            dl[3] = new RcharUAlpha(3);
            dl[4] = new RcharUAlpha(4);
            Array.Copy(Numerics(4, 5), 0, dl, 5, 4);
            dl[9] = new RcharLimited(9,'1','2','3','4','5','6','7','8','9');//no zero

            dlFormats = new[] {new DriversLicense(dl, this) };
        }
    }
}