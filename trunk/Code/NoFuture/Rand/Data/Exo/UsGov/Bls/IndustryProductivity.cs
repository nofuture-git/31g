using System;
using System.Text;

namespace NoFuture.Rand.Data.Exo.UsGov.Bls
{
    public class IndustryProductivity : ISeries
    {
        public Uri ApiLink => new Uri("https://www.bls.gov/help/hlpforma.htm#IP");
        public string Prefix => "IP";

        public char? SeasonalAdjustment { get; set; }

        public Bls.Codes.IpSector Sector { get; set; }
        public Bls.Codes.IpIndustry Industry { get; set; }
        public Bls.Codes.IpMeasure Measure { get; set; }
        public Bls.Codes.IpDuration Duration { get; set; }

        public override string ToString()
        {
            var str = new StringBuilder();
            str.Append(Prefix);
            str.Append(SeasonalAdjustment ?? Globals.Unadjusted);
            str.Append(Sector == null ? Globals.Defaults.IpSector : Sector.SectorCode);
            str.Append(Industry == null ? Globals.Defaults.IpIndustry : Industry.IndustryCode);
            str.Append(Measure == null ? Globals.Defaults.IpMeasure : Measure.MeasureCode);
            str.Append(Duration == null ? Globals.Defaults.IpDuration : Duration.DurationCode);

            return Globals.PostUrl + str;
        }
    }
}