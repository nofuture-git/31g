using System;
using System.Text;

namespace NoFuture.Rand.Gov.Bls
{
    public class ConsumerPriceIndex : ISeries
    {
        public Uri ApiLink { get { return new Uri("http://www.bls.gov/help/hlpforma.htm#CU"); } }
        public string Prefix { get { return "CU"; }}

        public char? SeasonalAdjustment { get; set; }
        public char? Periodicity { get; set; }
        public char? BaseYear { get; set; }

        public Bls.Codes.CuArea Area { get; set; }
        public Bls.Codes.CuItem Item { get; set; }

        public override string ToString()
        {
            var str = new StringBuilder();
            str.Append(Prefix);
            str.Append(SeasonalAdjustment ?? Globals.Unadjusted);
            str.Append(Periodicity ?? Globals.Monthly);
            str.Append(Area == null ? Globals.Defaults.CuArea : Area.AreaCode);
            str.Append(BaseYear ?? Globals.CurrentBaseYear);
            str.Append(Item == null ? Globals.Defaults.CuItem : Item.ItemCode);

            return str.ToString();

        }

    }

    public class EmploymentCostIndex : ISeries
    {
        public Uri ApiLink { get { return new Uri("http://www.bls.gov/help/hlpforma.htm#EC"); } }
        public string Prefix { get { return "EC"; } }

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

    public class IndustryProductivity : ISeries
    {
        public Uri ApiLink { get { return new Uri("http://www.bls.gov/help/hlpforma.htm#IP"); } }
        public string Prefix { get { return "IP"; } }

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

    public class ProducerPriceIndex : ISeries
    {
        public Uri ApiLink { get { return new Uri("http://www.bls.gov/help/hlpforma.htm#WP"); } }
        public string Prefix { get { return "WP"; } }

        public char? SeasonalAdjustment { get; set; }

        public Bls.Codes.WpGroup Group { get; set; }
        public Bls.Codes.WpItem Item { get; set; }

        public override string ToString()
        {
            var str = new StringBuilder();
            str.Append(Prefix);
            str.Append(SeasonalAdjustment ?? Globals.Unadjusted);
            str.Append(Group == null ? Globals.Defaults.WpGroup : Group.GroupCode);
            str.Append(Item == null ? Globals.Defaults.WpItem : Item.ItemCode);

            return Globals.PostUrl + str;
        }
    }

    public class NatlEmployment : ISeries
    {
        public Uri ApiLink { get { return new Uri("http://www.bls.gov/help/hlpforma.htm#CE"); } }
        public string Prefix { get { return "CE"; } }
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
