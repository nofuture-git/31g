using System;
using System.Text;

namespace NoFuture.Rand.Data.Exo.UsGov.Bls
{
    public class EmploymentCostIndex : ISeries
    {
        public Uri ApiLink => new Uri("https://www.bls.gov/help/hlpforma.htm#EC");
        public string Prefix => "EC";

        public char? SeasonalAdjustment { get; set; }

        public Bls.Codes.EcCompensation Compensation { get; set; }
        public Bls.Codes.EcGroup Group { get; set; }
        public Bls.Codes.EcOwnership Ownership { get; set; }
        public Bls.Codes.EcPeriod Period { get; set; }

        public override string ToString()
        {
            var str = new StringBuilder();
            str.Append(Prefix);
            str.Append(SeasonalAdjustment ?? Globals.Unadjusted);
            str.Append(Compensation == null ? Globals.Defaults.EcCompensation : Compensation.CompCode);
            str.Append(Group == null ? Globals.Defaults.EcGroup : Group.GroupCode);
            str.Append(Ownership == null ? Globals.Defaults.EcOwnership : Ownership.OwnershipCode);
            str.Append(Period == null ? Globals.Defaults.EcPeriod : Period.Period);

            return Globals.PostUrl + str;
        }
    }
}