using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Geo.CA;
using NoFuture.Rand.Geo.US;
using NoFuture.Shared.Core;
using NfString = NoFuture.Util.Core.NfString;

namespace NoFuture.Rand.Geo
{
    /// <summary>
    /// A composition type to contain a postal address.
    /// </summary>
    [Serializable]
    public class PostalAddress : DiachronIdentifier, IObviate
    {
        public StreetPo Street { get; set; }
        public CityArea CityArea { get; set; }

        public override string Abbrev => "Addr";

        public override string Value
        {
            get => ToString();
            set
            {
                var val = value;
                if (string.IsNullOrWhiteSpace(val))
                {
                    Street = null;
                    CityArea = null;
                    return;
                }
                var crlf = new string(new[] {Constants.CR, Constants.LF});
                var lf = Constants.LF.ToString();

                //normalize to the same new-line char sequence then convert back to just LF
                val = NfString.ConvertToCrLf(val).Replace(crlf, lf);

                var ln1 = val.Split(Constants.LF).FirstOrDefault();
                var ln2 = val.Split(Constants.LF).LastOrDefault();
                if(!TryParse(ln1, ln2, out var rslt))
                    throw new WatDaFookIzDis("The given address could not be parsed. Try adding " +
                                             "a new line between the PO box line and the City line.");

                Street = rslt.Street;
                CityArea = rslt.CityArea;
            }
        }

        public override string ToString()
        {
            return string.Join(Environment.NewLine, Street, CityArea);
        }

        public IDictionary<string, object> ToData(KindsOfTextCase txtCase)
        {
            var itemData = Street?.ToData(txtCase) ?? new Dictionary<string, object>();
            var moreData = CityArea?.ToData(txtCase) ?? new Dictionary<string, object>();
            if(moreData.Any())
                foreach(var k in moreData.Keys)
                    itemData.Add(k,moreData[k]);
            return itemData;
        }

        /// <summary>
        /// Helper factory method to create an american address in one call.
        /// </summary>
        /// <returns></returns>
        [RandomFactory]
        public static PostalAddress RandomAmericanAddress(string zipCodePrefix = null)
        {
            var csz = CityArea.RandomAmericanCity(zipCodePrefix);
            var homeAddr = StreetPo.RandomAmericanStreet();
            return new PostalAddress { Street = homeAddr, CityArea = csz };
        }

        /// <summary>
        /// Helper factory method to create an canadian address in one call.
        /// </summary>
        /// <returns></returns>
        [RandomFactory]
        public static PostalAddress RandomCanadianAddress()
        {
            var csz = CityArea.RandomCanadianCity();
            var homeAddr = StreetPo.RandomAmericanStreet();
            return new PostalAddress { Street = homeAddr, CityArea = csz };
        }

        /// <summary>
        /// Helper method to quickly go from string literals to a <see cref="PostalAddress"/>
        /// </summary>
        /// <param name="postOfficeLine1"></param>
        /// <param name="cityLine2"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        public static bool TryParse(string postOfficeLine1, string cityLine2,
            out PostalAddress address)
        {
            address = null;
            if (string.IsNullOrWhiteSpace(postOfficeLine1) || string.IsNullOrWhiteSpace(cityLine2))
                return false;

            if (!UsStreetPo.TryParse(postOfficeLine1, out var po))
                return false;

            if (UsCityStateZip.TryParse(cityLine2, out var csz))
            {
                address = new PostalAddress { Street = po, CityArea = csz };
                return true;
            }

            if (CaCityProvidencePost.TryParse(cityLine2, out var cpp))
            {
                address = new PostalAddress { Street = po, CityArea = cpp };
                return true;
            }
            return false;
        }
    }
}
