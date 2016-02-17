using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NoFuture.Exceptions;

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
    /// </remarks>
    public class Vin : Identifier
    {
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
        public const string PublicWebApiRootUri = "http://vpic.nhtsa.dot.gov/api/vehicles/";
        public const char DfDigit = '0';
        #endregion

        #region properties
        public WorldManufacturerId Wmi { get; set; }
        public VehicleDescription Vds { get; set; }
        public char? CheckDigit { get { return _chkDigit; }}
        public VehicleIdSection Vis { get; set; }

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

                vinIn = Util.Etc.BinaryMergeString(vinIn, _allZeros);

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
            var d = Etx.IntNumber(0, VinLetter2NumberValues.Count);
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
                return DfDigit;
            if (vin.Length != 17)
                return DfDigit;
            if (ContainsInvalidChars(vin))
                return DfDigit;

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
        public int GetModelYearYyyy()
        {
            const int BASE_YEAR = 1980;
            const int AMENDED_BASE_YEAR = 2010;

            var baseYear = 0;

            if (Char.IsNumber(Vds.Seven.GetValueOrDefault()))
                baseYear = BASE_YEAR;
            if (Char.IsLetter(Vds.Seven.GetValueOrDefault()))
                baseYear = AMENDED_BASE_YEAR;

            if(baseYear == 0)
                throw new RahRowRagee("The char at position [7] (one-based) of the VIN is neither a number nor a letter.");

            for (var i = 0; i < YearIdx.Length; i++)
            {
                if (YearIdx[i] == Vis.ModelYear)
                    return baseYear + i;

            }
            throw new ItsDeadJim(
                String.Format("Cannot determine the VIN Model Year from the ModelYear char value of '{0}'",
                    Vis.ModelYear));
        }

        /// <summary>
        /// Generates a random VIN
        /// </summary>
        /// <returns></returns>
        public static Vin GetRandomVin()
        {
            var wmiOut = new WorldManufacturerId();
            //JA-J0 Japan
            //KL-KR Korea
            //LA-L0 China
            //1A-10, 4A-40, 5A-50 US
            //3A-37 Mexico
            var country = Etx.IntNumber(1, 12);

            switch (country)
            {
                case 1:
                    wmiOut.Country = 'J';
                    break;
                case 2:
                    wmiOut.Country = 'K';
                    break;
                case 3:
                    wmiOut.Country = 'L';
                    break;
                case 4:
                case 5:
                case 6:
                    wmiOut.Country = '1';
                    break;
                case 7:
                    wmiOut.Country = '4';
                    break;
                case 8:
                    wmiOut.Country = '5';
                    break;
                case 9:
                case 10:
                case 11:
                case 12:
                    wmiOut.Country = '3';
                    break;
            }

            wmiOut.RegionMaker = GetRandomVinChar();
            wmiOut.VehicleType = GetRandomVinChar();

            var vds = new VehicleDescription
            {
                Four = GetRandomVinChar(),
                Five = GetRandomVinChar(),
                Six = GetRandomVinChar(),
                Seven = GetRandomVinChar(),
                Eight = GetRandomVinChar()
            };

            var vis = new VehicleIdSection
            {
                ModelYear = Vin.YearIdx[Etx.IntNumber(0, YearIdx.Length)],
                PlantCode = GetRandomVinChar(),
                SequentialNumber = Etx.Chars(0x30, 0x39, 6)
            };

            return new Vin {Wmi = wmiOut, Vds = vds, Vis = vis};
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
        public string GetVpicDecodeVinUrl()
        {
            return PublicWebApiRootUri + "DecodeVinValues/" + Wmi + Vds + Vis + "?format=json&modelyear=" + GetModelYearYyyy();
        }

        public string GetVpicDecodeWmiUrl()
        {
            return PublicWebApiRootUri + "DecodeWMI/" + Wmi + "?format=json";
        }

        public static string GetVpicAllMakesUrl()
        {
            return PublicWebApiRootUri + "getallmakes?format=json";
        }

        public static string GetVpicAllManufacturersUrl()
        {
            return PublicWebApiRootUri + "getallmanufacturers?format=json";
        }

        public static string GetVpicMakes2ManufacturerUrl(string manufacturerName)
        {
            return PublicWebApiRootUri + "getmakesformanufacturer/" + manufacturerName + "?format=json";
        }

        public static string GetVpicEquipPlantCodes(int? year)
        {
            var yyyy = year == null ? DateTime.Now.Year : year.Value;
            return PublicWebApiRootUri + "GetEquipmentPlantCodes/" + yyyy + "?format=json";
        }

        public static string GetVpicModels2MakesUrl(string manufacturerName)
        {
            return PublicWebApiRootUri + "getmodelsformake/" + manufacturerName + "?format=json";
        }

        public static string GetVpicModels2Makes2YearUrl(string manufacturerName, int? year)
        {
            var yyyy = year == null ? DateTime.Now.Year : year.Value;
            return PublicWebApiRootUri + "GetModelsForMakeYear/make/" + manufacturerName + "/modelyear/" + yyyy +
                   "?format=json";
        }
        #endregion
    }


    /// <summary>
    /// Represents the chars 1-3 of a VIN 
    /// see http://vpic.nhtsa.dot.gov/
    /// </summary>
    public struct WorldManufacturerId
    {
        [VinPosition(1,1)]
        public char? Country { get; set; }
        [VinPosition(2, 1)]
        public char? RegionMaker { get; set; }
        [VinPosition(3, 1)]
        public char? VehicleType { get; set; }

        /// <summary>
        /// String representation of this portion of the VIN
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Defaults to <see cref="Vin.DfDigit"/>
        /// </remarks>
        public override string ToString()
        {
            var str = new StringBuilder();
            str.Append(Country ?? Vin.DfDigit);
            str.Append(RegionMaker ?? Vin.DfDigit);
            str.Append(VehicleType ?? Vin.DfDigit);
            return str.ToString();
        }
    }

    /// <summary>
    /// Represents the chars 4-8 of a VIN
    /// see http://vpic.nhtsa.dot.gov/
    /// </summary>
    public struct VehicleDescription
    {
        [VinPosition(4, 1)]
        public char? Four { get; set; }
        [VinPosition(5, 1)]
        public char? Five { get; set; }
        [VinPosition(6, 1)]
        public char? Six { get; set; }
        [VinPosition(7, 1)]
        public char? Seven { get; set; }
        [VinPosition(8, 1)]
        public char? Eight { get; set; }

        /// <summary>
        /// String representation of this portion of the VIN
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Defaults to <see cref="Vin.DfDigit"/>
        /// </remarks>
        public override string ToString()
        {
            var str = new StringBuilder();
            str.Append(Four ?? Vin.DfDigit);
            str.Append(Five ?? Vin.DfDigit);
            str.Append(Six ?? Vin.DfDigit);
            str.Append(Seven ?? Vin.DfDigit);
            str.Append(Eight ?? Vin.DfDigit);
            return str.ToString();
        }
    }

    /// <summary>
    /// Represents the chars 10-17 of a VIN 
    /// see http://vpic.nhtsa.dot.gov/
    /// </summary>
    public struct VehicleIdSection
    {
        [VinPosition(10,1)]
        public char? ModelYear { get; set; }
        [VinPosition(11,1)]
        public char? PlantCode { get; set; }
        [VinPosition(12,6)]
        public string SequentialNumber { get; set; }

        /// <summary>
        /// String representation of this portion of the VIN
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Defaults to <see cref="Vin.DfDigit"/>
        /// </remarks>
        public override string ToString()
        {
            var str = new StringBuilder();
            str.Append(ModelYear ?? Vin.DfDigit);
            str.Append(PlantCode ?? Vin.DfDigit);
            str.Append(SequentialNumber ?? new string(Vin.DfDigit, 6));
            return str.ToString();
        }
    }

    /// <summary>
    /// Is One based index
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class VinPositionAttribute : Attribute
    {
        private readonly int _pos;
        private readonly int _len ;
        public int Position1Base { get { return _pos; }}
        public int Len { get { return _len; } }

        public VinPositionAttribute(int position, int length)
        {
            _pos = position;
            _len = length;
        }

    }
}
