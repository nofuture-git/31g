using System;
using System.Text;
using NoFuture.Rand.Core;
using NoFuture.Util.Core;
using NfString = NoFuture.Util.Core.NfString;

namespace NoFuture.Rand.Gov.US.TheStates
{
    [Serializable]
    public class Wisconsin : UsState
    {
        public Wisconsin() : base("WI")
        {
            var dl = new Rchar[13];
            dl[0] = new RcharUAlpha(0);
            Array.Copy(Numerics(12,1),0,dl,1,12);
            dlFormats = new[] {new DriversLicense(dl, this) };
        }

        public override string GetRandomDriversLicense()
        {
            var dlVal = base.GetRandomDriversLicense();
            var chkDigit = NfString.CalcLuhnCheckDigit(dlVal);
            var dlOut = new StringBuilder();
            dlOut.Append(dlVal);
            dlOut.Append(chkDigit.ToString());
            return dlOut.ToString();
        }

        public override bool ValidDriversLicense(string dlnumber)
        {
            if (string.IsNullOrWhiteSpace(dlnumber))
                return false;
            //all but last digit
            var dlVal = dlnumber.Substring(0, dlnumber.Length - 1);

            var dlLastChar = dlnumber.Substring(dlnumber.Length - 1, 1);
            var dlChkDigit = 0;
            if (!int.TryParse(dlLastChar, out dlChkDigit))
                return false;

            var calcChkDigit = NfString.CalcLuhnCheckDigit(dlVal);

            return base.ValidDriversLicense(dlVal) && dlChkDigit == calcChkDigit;
        }
    }
}