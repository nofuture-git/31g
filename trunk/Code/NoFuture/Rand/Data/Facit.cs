using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NoFuture.Rand.Core;
using NoFuture.Rand.Data.Endo.Enums;
using NoFuture.Rand.Gov.Nhtsa;
using NoFuture.Util.Core;

namespace NoFuture.Rand.Data
{
    /// <summary>
    /// A factory for various values whose randomness is dependent of 
    /// Data contained within Rand.Data.Source
    /// </summary>
    public static class Facit
    {
        /// <summary>
        /// Gets a random Uri host.
        /// </summary>
        /// <returns></returns>
        public static string RandomUriHost(bool withSubDomain = true, bool usCommonOnly = false)
        {
            var webDomains = usCommonOnly ? ListData.UsWebmailDomains : ListData.WebmailDomains;
            var host = new StringBuilder();

            if (withSubDomain)
            {
                var subdomain = Etx.DiscreteRange(ListData.Subdomains);
                host.Append(subdomain + ".");
            }

            if (webDomains != null)
            {
                host.Append(webDomains[Etx.IntNumber(0, webDomains.Length - 1)]);
            }
            else
            {
                host.Append(Word());
                host.Append(Etx.DiscreteRange(new[] { ".com", ".net", ".edu", ".org" }));
            }
            return host.ToString();
        }

        /// <summary>
        /// Create a random http scheme uri with optional query string.
        /// </summary>
        /// <param name="useHttps"></param>
        /// <param name="addQry"></param>
        /// <returns></returns>
        public static Uri RandomHttpUri(bool useHttps = false, bool addQry = false)
        {

            var pathSeg = new List<string>();
            var pathSegLen = Etx.IntNumber(0, 5);
            for (var i = 0; i < pathSegLen; i++)
            {
                Etx.DiscreteRange(new Dictionary<string, double>()
                {
                    {Word(), 72},
                    {Etx.Consonant(false).ToString(), 11},
                    {Etx.IntNumber(1, 9999).ToString(), 17}
                });
                pathSeg.Add(Word());
            }

            if (Etx.CoinToss)
            {
                pathSeg.Add(Word() + Etx.DiscreteRange(new[] {".php", ".aspx", ".html", ".txt", ".asp"}));
            }

            var uri = new UriBuilder
            {
                Scheme = useHttps ? "https" : "http",
                Host = RandomUriHost(),
                Path = String.Join("/", pathSeg.ToArray())
            };

            if (!addQry)
                return uri.Uri;

            var qry = new List<string>();
            var qryParms = Etx.IntNumber(1, 5);
            for (var i = 0; i < qryParms; i++)
            {
                var len = Etx.IntNumber(1, 4);
                var qryParam = new List<string>();
                for (var j = 0; j < len; j++)
                {
                    if (Etx.CoinToss)
                    {
                        qryParam.Add(Word());
                        continue;
                    }
                    if (Etx.CoinToss)
                    {
                        qryParam.Add(Etx.IntNumber(0, 99999).ToString());
                        continue;
                    }
                    qryParam.Add(Etx.Consonant(Etx.CoinToss).ToString());

                }
                qry.Add(String.Join("_", qryParam) + "=" + Etx.SupriseMe());
            }

            uri.Query = String.Join("&", qry);
            return uri.Uri;
        }




        /// <summary>
        /// Attempts to return a common english
        /// </summary>
        /// <returns></returns>
        public static string Word()
        {
            var enWords = TreeData.EnglishWords;
            if (enWords == null || enWords.Count <= 0)
                return Etx.Word(8);
            var pick = Etx.IntNumber(0, enWords.Count - 1);
            var enWord = enWords[pick]?.Item1;
            return !String.IsNullOrWhiteSpace(enWord)
                ? enWord
                : Etx.Word(8);
        }



        /*
         TODO static methods from Domus
         */



        /*
         TODO static methods from Gov
         */

        public static Tuple<WorldManufacturerId, string> GetRandomManufacturerId()
        {
            var df = new Tuple<WorldManufacturerId, string>(WorldManufacturerId.CreateRandomManufacturerId(), String.Empty);
            if (TreeData.VinWmi == null)
            {
                return df;
            }

            //pick the kind of vehicle
            var xml = TreeData.VinWmi;
            var wmiOut = new WorldManufacturerId();
            var xpath = "//vehicle-type";
            var pick = Etx.IntNumber(1, 3);
            switch (pick)
            {
                case 1:
                    xpath += "[@name='car']/wmi";
                    break;
                case 2:
                    xpath += "[@name='truck']/wmi";
                    break;
                case 3:
                    xpath += "[@name='suv']/wmi";
                    break;
            }

            // pick a manufacturer for vehicle type
            var mfNodes = xml.SelectNodes(xpath);
            if (mfNodes == null || mfNodes.Count <= 0)
                return df;
            pick = Etx.IntNumber(0, mfNodes.Count - 1);
            var mfNode = mfNodes[pick];
            if (mfNode == null)
                return df;
            var mfName = mfNode.Attributes?["id"]?.Value;
            if (String.IsNullOrWhiteSpace(mfName))
                return df;
            xpath += $"[@id='{mfName}']";
            var wmiNodes = xml.SelectNodes(xpath + "/add");
            if (wmiNodes == null || wmiNodes.Count <= 0)
                return df;
            pick = Etx.IntNumber(0, wmiNodes.Count - 1);
            var wmiNode = wmiNodes[pick];
            if (wmiNode == null)
                return df;
            var wmiStr = wmiNode.Attributes?["value"]?.Value;
            if (String.IsNullOrWhiteSpace(wmiStr) || wmiStr.Length != 3)
                return df;
            var wmiChars = wmiStr.ToCharArray();
            wmiOut.Country = wmiChars[0];
            wmiOut.RegionMaker = wmiChars[1];
            wmiOut.VehicleType = wmiChars[2];

            df = new Tuple<WorldManufacturerId, string>(wmiOut, String.Empty);

            //pick a vehicle's common name
            xpath += "/vehicle-names/add";
            var vhNameNodes = xml.SelectNodes(xpath);
            if (vhNameNodes == null || vhNameNodes.Count <= 0)
                return df;

            pick = Etx.IntNumber(0, vhNameNodes.Count - 1);
            var vhNameNode = vhNameNodes[pick];
            if (vhNameNode == null)
                return df;

            var vhName = vhNameNode.Attributes?["value"]?.Value;
            if (String.IsNullOrWhiteSpace(vhName))
                return df;

            return new Tuple<WorldManufacturerId, string>(wmiOut, vhName);
        }


        /// <summary>
        /// Generates a random VIN
        /// </summary>
        /// <param name="allowForOldModel">
        /// Allow random to produce model years back to 1980
        /// </param>
        /// <param name="maxYear">
        /// Allows calling api to specify a max allowable year of make.
        /// </param>
        /// <returns></returns>
        public static Vin GetRandomVin(bool allowForOldModel = false, int? maxYear = null)
        {
            var wmiAndName = GetRandomManufacturerId();
            var wmiOut = wmiAndName.Item1;

            //when this is a digit it will allow for a much older make back to 1980
            var yearBaseDeterminer = allowForOldModel && Etx.TryAboveOrAt(66, Etx.Dice.OneHundred)
                ? Vin.DF_DIGIT
                : Vin.GetRandomVinChar();

            var vds = new VehicleDescription
            {
                Four = Vin.GetRandomVinChar(),
                Five = Vin.GetRandomVinChar(),
                Six = Vin.GetRandomVinChar(),
                Seven = yearBaseDeterminer,
                Eight = Vin.GetRandomVinChar()
            };

            var vis = VehicleIdSection.GetVehicleIdSection(Char.IsNumber(vds.Seven.GetValueOrDefault()), maxYear);

            var vin = new Vin { Wmi = wmiOut, Vds = vds, Vis = vis };

            if (!String.IsNullOrWhiteSpace(wmiAndName.Item2))
                vin.Description = wmiAndName.Item2;

            return vin;
        }
    }
}
