using NoFuture.Rand.Core;

namespace NoFuture.Rand.Gov.TheStates
{
    public class Washington : UsState
    {
        public Washington() : base("WA")
        {
            var dl = new Rchar[12];
            dl[0] = new RcharUAlpha(0);
            dl[1] = new RcharUAlpha(1);
            dl[2] = new RcharUAlpha(2);
            dl[3] = new RcharUAlpha(3);
            dl[4] = new RcharUAlpha(4);
            dl[5] = new RcharUAlpha(5);
            dl[6] = new RcharUAlpha(6);
            dl[7] = new RcharAlphaNumeric(7);
            dl[8] = new RcharNumeric(8);
            dl[9] = new RcharLimited(9,'*');
            dl[10] = new RcharAlphaNumeric(10);
            dl[11] = new RcharAlphaNumeric(11);

            dlFormats = new[] {new DriversLicense(dl, this) };
        }
    }
}