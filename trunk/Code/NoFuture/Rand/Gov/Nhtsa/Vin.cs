﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NoFuture.Rand.Core;
using NoFuture.Util.Core;

namespace NoFuture.Rand.Gov.Nhtsa
{
    /// <summary>
    /// Vehicle Identification Number
    /// </summary>
    /// <remarks>
    /// NHTSA Public API Reference
    /// http://vpic.nhtsa.dot.gov/api/
    /// 
    /// Web Description
    /// http://vpic.nhtsa.dot.gov/
    /// 
    /// % Americans with Auto
    /// Src: [https://people.hofstra.edu/geotrans/eng/ch6en/conc6en/USAownershipcars.html]
    /// </remarks>
    [Serializable]
    public class Vin : Identifier
    {
        #region inner types

        public enum VehicleType
        {
            Unknown,
            Car,
            Truck,
            Suv
        }
        #endregion

        #region fields
        public static readonly char[] YearIdx = new[]
        {
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'P', 'R', 'S', 'T', 'V', 'W', 'X', 'Y',
            '1', '2', '3', '4', '5', '6', '7', '8', '9'
        };

        public static readonly Dictionary<char, int> VinLetter2NumberValues = new Dictionary<char, int>
        {
            {'A', 1}, {'B', 2}, {'C', 3}, {'D', 4}, {'E', 5}, {'F', 6}, {'G', 7}, {'H', 8},
            {'J', 1}, {'K', 2}, {'L', 3}, {'M', 4}, {'N', 5}, {'P', 7}, {'R', 9},
            {'S', 2}, {'T', 3}, {'U', 4}, {'V', 5}, {'W', 6}, {'X', 7}, {'Y', 8}, {'Z', 9}
        };

        private readonly int[] _vinCharPosWeight = new[]
        {
            8, 7, 6, 5, 4, 3, 2, 10, 0, 9, 8, 7, 6, 5, 4, 3, 2
        };

        private const string _allZeros = "00000000000000000";

        private char? _chkDigit;
        #endregion

        #region constants
        public const string PUBLIC_WEB_API_ROOT_URI = "http://vpic.nhtsa.dot.gov/api/vehicles/";
        public const char DF_DIGIT = '0';
        public const char DF_LETTER = 'A';
        internal const int BASE_YEAR = 1980;
        internal const int AMENDED_BASE_YEAR = 2010;
        #endregion

        #region properties
        public WorldManufacturerId Wmi { get; set; }
        public VehicleDescription Vds { get; set; }
        public char? CheckDigit => _chkDigit;
        public VehicleIdSection Vis { get; set; }
        public string Description { get; set; }
        public bool IsNhtsaValid { get; set; }

        public override string Value
        {
            get { return ToString(); }
            set
            {
                var vinIn = value;
                if (string.IsNullOrWhiteSpace(vinIn))
                {
                    Wmi = new WorldManufacturerId();
                    Vds = new VehicleDescription();
                    Vis = new VehicleIdSection();
                    return;
                }

                vinIn = Etc.BinaryMergeString(vinIn, _allZeros);

                var vinChars = vinIn.ToCharArray();
                Wmi = new WorldManufacturerId
                {
                    Country = vinChars[0],
                    RegionMaker = vinChars[1],
                    VehicleType = vinChars[2]
                };
                Vds = new VehicleDescription
                {
                    Four = vinChars[3],
                    Five = vinChars[4],
                    Six = vinChars[5],
                    Seven = vinChars[6],
                    Eight = vinChars[7]
                };

                Vis = new VehicleIdSection
                {
                    ModelYear = vinChars[9],
                    PlantCode = vinChars[10],
                    SequentialNumber = vinIn.Substring(11, 6)
                };

            }
        }

        #endregion

        #region methods

        public static char GetRandomVinChar()
        {
            var d = Etx.IntNumber(0, VinLetter2NumberValues.Count-1);
            var f = VinLetter2NumberValues.Select(x => x.Key).ToArray()[d];
            return f;
        }

        /// <summary>
        /// See https://en.wikipedia.org/wiki/Vehicle_identification_number#Check_digit_calculation
        /// </summary>
        /// <returns></returns>
        protected internal virtual char GetCheckDigit()
        {
            var vin = GetVinWithChkDigit(null);

            if (String.IsNullOrWhiteSpace(vin))
                return DF_DIGIT;
            if (vin.Length != 17)
                return DF_DIGIT;
            if (ContainsInvalidChars(vin))
                return DF_DIGIT;

            var vinChars = vin.ToCharArray();
            var workingArray = new int[17];

            for (var i = 0; i < 17; i++)
            {
                if (Char.IsLetter(vinChars[i]))
                {
                    workingArray[i] = VinLetter2NumberValues[vinChars[i]];
                }
                if (Char.IsNumber(vinChars[i]))
                {
                    workingArray[i] = Convert.ToInt32(vinChars[i]) - 0x30;
                }
            }

            var workingSum = 0;
            for (var j = 0; j < 17; j++)
            {
                workingSum += workingArray[j]*_vinCharPosWeight[j];
            }

            var cd = workingSum%11;

            return cd == 10 ? 'X' : Convert.ToChar(cd + 0x30);
        }

        public override string ToString()
        {
            if (_chkDigit == null)
                _chkDigit = GetCheckDigit();
            return GetVinWithChkDigit(_chkDigit);
        }

        public override string Abbrev
        {
            get { return "VIN"; }
        }

        protected internal virtual bool ContainsInvalidChars(string s)
        {
            return s.ToCharArray().Any(x => x == 'i' || x == 'I' || x == 'o' || x == 'O' || x == 'q' || x == 'Q');
        }

        protected internal virtual string GetVinWithChkDigit(char? chkDigit)
        {
            var str = new StringBuilder();

            str.Append(Wmi);

            str.Append(Vds);

            str.Append(chkDigit ?? '0');

            str.Append(Vis);

            return str.ToString();
        }

        /// <summary>
        /// Translates the single char code at position 10 of a VIN into
        /// the typical four-digit year.
        /// </summary>
        /// <returns></returns>
        public int? GetModelYearYyyy()
        {


            var baseYear = 0;

            if (Char.IsNumber(Vds.Seven.GetValueOrDefault()))
                baseYear = BASE_YEAR;
            if (Char.IsLetter(Vds.Seven.GetValueOrDefault()))
                baseYear = AMENDED_BASE_YEAR;

            if (baseYear == 0)
                return null;

            for (var i = 0; i < YearIdx.Length; i++)
            {
                if (YearIdx[i] == Vis.ModelYear)
                    return baseYear + i;

            }
            return null;
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
            var wmiAndName = WorldManufacturerId.GetRandomManufacturerId();
            var wmiOut = wmiAndName.Item1;

            //when this is a digit it will allow for a much older make back to 1980
            var yearBaseDeterminer = allowForOldModel && Etx.TryAboveOrAt(66, Etx.Dice.OneHundred)
                ? DF_DIGIT
                : GetRandomVinChar();

            var vds = new VehicleDescription
            {
                Four = GetRandomVinChar(),
                Five = GetRandomVinChar(),
                Six = GetRandomVinChar(),
                Seven = yearBaseDeterminer,
                Eight = GetRandomVinChar()
            };

            var vis = VehicleIdSection.GetVehicleIdSection(char.IsNumber(vds.Seven.GetValueOrDefault()), maxYear);

            var vin = new Vin {Wmi = wmiOut, Vds = vds, Vis = vis};

            if (!string.IsNullOrWhiteSpace(wmiAndName.Item2))
                vin.Description = wmiAndName.Item2;

            return vin;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            var vin = obj as Vin;
            if (vin == null)
                return false;

            return string.Equals(vin.ToString(), ToString(), StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        #endregion

        #region nhtsa public web api
        public Uri GetUriVpicDecodeVin()
        {
            return new Uri(PUBLIC_WEB_API_ROOT_URI + "DecodeVinValues/" + Wmi + Vds + Vis + "?format=json&modelyear=" + GetModelYearYyyy());
        }

        public Uri GetUriVpicDecodeWmi()
        {
            return new Uri(PUBLIC_WEB_API_ROOT_URI + "DecodeWMI/" + Wmi + "?format=json");
        }

        public static Uri GetUriVpicAllMakes()
        {
            return new Uri(PUBLIC_WEB_API_ROOT_URI + "getallmakes?format=json");
        }

        public static Uri GetUriVpicAllManufacturers()
        {
            return new Uri(PUBLIC_WEB_API_ROOT_URI + "getallmanufacturers?format=json");
        }

        public static Uri GetUriVpicMakes2Manufacturer(string manufacturerName)
        {
            return new Uri(PUBLIC_WEB_API_ROOT_URI + "getmakesformanufacturer/" + manufacturerName + "?format=json");
        }

        public static Uri GetUriVpicEquipPlantCodes(int? year)
        {
            var yyyy = year ?? DateTime.Now.Year;
            return new Uri(PUBLIC_WEB_API_ROOT_URI + "GetEquipmentPlantCodes/" + yyyy + "?format=json");
        }

        public static Uri GetVpicModels2MakesUrl(string manufacturerName)
        {
            return new Uri(PUBLIC_WEB_API_ROOT_URI + "getmodelsformake/" + manufacturerName + "?format=json");
        }

        public static Uri GetVpicModels2Makes2YearUrl(string manufacturerName, int? year)
        {
            var yyyy = year ?? DateTime.Now.Year;
            return new Uri(PUBLIC_WEB_API_ROOT_URI + "GetModelsForMakeYear/make/" + manufacturerName + "/modelyear/" + yyyy +
                           "?format=json");
        }
        #endregion
    }
}