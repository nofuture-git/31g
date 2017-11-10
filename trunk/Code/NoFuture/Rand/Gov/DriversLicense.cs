using System;
using System.Linq;
using NoFuture.Rand.Core;
using NoFuture.Rand.Domus;
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

        public DriversLicense IssueNewLicense(NorthAmerican amer, DateTime? issuedDate = null)
        {
            if(format == null || !format.Any() || IssuingState == null)
                throw new ItsDeadJim("Cannot issue a DL with having a DL Format and IssuingState");

            if (amer == null)
                return new DriversLicense(format, IssuingState);

            var dl = new DriversLicense(format, IssuingState)
            {
                Dob = amer.BirthCert.DateOfBirth,
                FullLegalName = string.Join(" ", amer.FirstName.ToUpper(), amer.MiddleName.ToUpper(),
                    amer.LastName.ToUpper()),
                Gender = amer.MyGender,
                PrincipalResidence = amer.Address.ToString(),
                IssuedDate = issuedDate ?? Etx.Date(-5, DateTime.Today)
            };
            return dl;
        }

        public override string ToString()
        {
            return string.Join(" ",this.IssuingState,
                base.ToString());
        }

        public override string Abbrev => "DL";

    }

}
