using System;
using System.Linq;
using NoFuture.Rand.Core;
using NoFuture.Shared.Core;

namespace NoFuture.Rand.Gov
{
    [Serializable]
    public class DriversLicense : StateIssuedId
    {
        
        public DriversLicense(Rchar[] format)
        {
            this.format = format;
        }

        public DriversLicense(Rchar[] format, UsState issuingState):this(format)
        {
            IssuingState = issuingState;
        }

        public DriversLicense IssueNewLicense(DateTime? issuedDate = null)
        {
            if(format == null || !format.Any() || IssuingState == null)
                throw new ItsDeadJim("Cannot issue a DL with having a DL Format and IssuingState");

            return new DriversLicense(format, IssuingState)
            {
                IssuedDate = issuedDate ?? Etx.Date(-5, DateTime.Today)

            };
        }

        public override string ToString()
        {
            return string.Join(" ",this.IssuingState,
                base.ToString());
        }

        public override string Abbrev => "DL";

    }

}
