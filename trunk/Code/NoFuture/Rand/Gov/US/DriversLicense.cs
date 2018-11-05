using System;
using System.Linq;
using NoFuture.Rand.Core;
using NoFuture.Shared.Core;

namespace NoFuture.Rand.Gov.US
{
    [Serializable]
    public class DriversLicense : StateIssuedId
    {
        public DriversLicense(string dlnumber, string state)
        {
            var usState = UsState.GetState(state);
            if(usState == null)
                throw new ArgumentException($"No state found by name '{state}'");
            if(!usState.ValidDriversLicense(dlnumber))
                throw new ArgumentException($"For the state of {usState.StateName}, " +
                                            $"the drivers license value of '{dlnumber}' is invalid.");
            format = usState.GetDriversLicenseFormats().FirstOrDefault()?.format;
            IssuingState = usState;
            _value = dlnumber;
        }

        protected internal DriversLicense(Rchar[] format)
        {
            this.format = format;
            IssuedDate = Etx.RandomDate(-5, DateTime.Today);
        }

        protected internal DriversLicense(Rchar[] format, UsState issuingState):this(format)
        {
            IssuingState = issuingState;
            IssuedDate = Etx.RandomDate(-5, DateTime.Today);
        }

        public DriversLicense IssueNewLicense(DateTime? issuedDate = null)
        {
            if(format == null || !format.Any() || IssuingState == null)
                throw new ItsDeadJim("Cannot issue a DL with having a DL Format and IssuingState");

            return new DriversLicense(format, IssuingState)
            {
                IssuedDate = issuedDate ?? Etx.RandomDate(-5, DateTime.Today)

            };
        }

        /// <summary>
        /// Gets a drivers license number at random
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        [RandomFactory]
        public static string RandomDriversLicense(string state = null)
        {
            var usState = UsState.GetState(state);
            usState = usState ?? UsState.RandomUsState();

            return usState.GetRandomDriversLicense();
        }

        public override string ToString()
        {
            return string.Join(" ",IssuingState.StateAbbrev,
                base.ToString());
        }

        public override string Abbrev => "DL";

    }

}
