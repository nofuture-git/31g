using System;
using System.Text;

namespace NoFuture.Rand.Data.Exo.UsGov.Bls
{
    public class NatlEmployment : ISeries
    {
        public Uri ApiLink => new Uri("https://www.bls.gov/help/hlpforma.htm#CE");
        public string Prefix => "CE";
        public char? SeasonalAdjustment { get; set; }

        public Bls.Codes.CeSupersector Supersector { get; set; }
        public Bls.Codes.CeIndustry Industry { get; set; }
        public Bls.Codes.CeDatatype Datatype { get; set; }

        public override string ToString()
        {
            var str = new StringBuilder();
            str.Append(Prefix);
            str.Append(SeasonalAdjustment ?? Globals.Unadjusted);
            if (Supersector != null)
            {
                str.Append(Supersector.SupersectorCode);
                str.Append("000000");
            }
            else if (Industry != null)
            {
                str.Append(Industry.IndustryCode);
            }
            else
            {
                str.Append(Globals.Defaults.CeIndustry);
            }
            
            str.Append(Datatype == null ? Globals.Defaults.CeDatatype : Datatype.DataTypeCode);
            return Globals.PostUrl + str;
        }
    }
}