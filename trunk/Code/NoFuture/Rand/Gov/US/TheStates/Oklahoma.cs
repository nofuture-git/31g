using System;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Gov.US.TheStates
{
    public class Oklahoma : UsState
    {
        public Oklahoma() : base("OK")
        {
            var dl = new Rchar[10];
            dl[0] = new RcharLimited(0,
                'A',
                'B',
                'C',
                'D',
                'E',
                'F',
                'G',
                'H',
                'J',
                'K',
                'L',
                'M',
                'N',
                'P',
                'R',
                'S',
                'T',
                'U',
                'V',
                'W',
                'Y',
                'Z');//no 'I','O', 'X' nor 'Q'
            Array.Copy(Numerics(9,1),0,dl,1,9);
            dlFormats = new[] {new DriversLicense(dl, this), new DriversLicense(Numerics(9), this) };
        }
    }
}