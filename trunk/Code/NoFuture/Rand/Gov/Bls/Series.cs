using System;
using System.Text;
using NoFuture.Shared;
using NoFuture.Shared.Core;

namespace NoFuture.Rand.Gov.Bls
{
    public abstract class BlsSeriesBase : ISeries
    {
        public abstract Uri ApiLink { get; }
        public abstract string Prefix { get; }

        /// <summary>
        /// Creates the body for a Multiseries POST to the 
        /// BLS API. The <see cref="sYear"/> must be equal to or less
        /// than the <see cref="eYear"/>
        /// </summary>
        /// <param name="seriesCodes">Must not exceed the BLS limit of 50.</param>
        /// <param name="sYear">Start year, has a minimum year of 1950 and max of the current year.</param>
        /// <param name="eYear">End year, has a maximum of the current year and a minimum of 1950</param>
        /// <returns>
        /// JSON string for POST body
        /// </returns>
        /// <remarks>
        /// The <see cref="NfConfig.SecurityKeys.BlsApiRegistrationKey"/> must be assigned before invocation.
        /// See [https://www.bls.gov/developers/api_faqs.htm#register1] concerning the arg-limits.
        /// </remarks>
        public static string GetMultiSeriesPostBody(string[] seriesCodes, int sYear, int eYear)
        {
            if(seriesCodes == null || seriesCodes.Length <=0)
                throw new ArgumentNullException(nameof(seriesCodes));

            if(sYear < 1950 || sYear > DateTime.Today.Year )
                throw new ArgumentException($"The start year {sYear} is not valid - " +
                                            "the minimum is 1950 and the maximum " +
                                            "is the current year.");

            if (eYear < 1950 || eYear > DateTime.Today.Year)
                throw new ArgumentException($"The end year {eYear} is not valid - " +
                                            "the minimum is 1950 and the maximum " +
                                            "is the current year.");

            if (sYear > eYear)
                throw new ArgumentException($"The start year, {sYear}, must be " +
                                            $"greater-than or equal-to the end year, {eYear}.");

            if(eYear - sYear > 20)
                throw new ArgumentException("BLS Api has a max spread of 20 years per " +
                                            $"request.  The {sYear} to {eYear} is a differnce" +
                                            $"of {eYear - sYear} years.");
            if(seriesCodes.Length > 50)
                throw new ArgumentException("BLS Api has a max series count " +
                                            "of 50 - the passed in arg has a count " +
                                            $"of {seriesCodes.Length}.");

            if (string.IsNullOrWhiteSpace(NfConfig.SecurityKeys.BlsApiRegistrationKey))
                throw new RahRowRagee("The 'NoFuture.Globals.SecurityKeys.BlsApiRegistrationKey' " +
                                                 "must be set before calling this property.");
            dynamic payload =
                new
                {
                    seriesid = seriesCodes,
                    startyear = $"{sYear}",
                    endyear = $"{eYear}",
                    registrationKey = NfConfig.SecurityKeys.BlsApiRegistrationKey
                };
            return Newtonsoft.Json.JsonConvert.SerializeObject(payload);
        }
    }
    public class ConsumerPriceIndex : ISeries
    {
        public Uri ApiLink { get { return new Uri("https://www.bls.gov/help/hlpforma.htm#CU"); } }
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
        public Uri ApiLink { get { return new Uri("https://www.bls.gov/help/hlpforma.htm#EC"); } }
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
        public Uri ApiLink { get { return new Uri("https://www.bls.gov/help/hlpforma.htm#IP"); } }
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
        public Uri ApiLink { get { return new Uri("https://www.bls.gov/help/hlpforma.htm#WP"); } }
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
        public Uri ApiLink { get { return new Uri("https://www.bls.gov/help/hlpforma.htm#CE"); } }
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
