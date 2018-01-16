using System;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Gov.US.TheStates
{
    [Serializable]
    public class Massachusetts : UsState
    {
        public Massachusetts() : base("MA")
        {
            var dl = new Rchar[9];
            dl[0] = new RcharLimited(0,
                'A',
                'B',
                'C',
                'D',
                'E',
                'F',
                'G',
                'H',
                'I',
                'J',
                'K',
                'L',
                'M',
                'N',
                'O',
                'P',
                'Q',
                'R',
                'S',
                'T',
                'U',
                'V',
                'W',
                'Y',
                'Z');// no 'X'
            Array.Copy(Numerics(8, 1), 0, dl, 1, 8);

            dlFormats = new[] { new DriversLicense(dl, this), new DriversLicense(Numerics(9), this) };
        }
    }
}