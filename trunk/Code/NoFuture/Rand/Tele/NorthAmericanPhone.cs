﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Data.Endo
{
    /// <summary>
    /// Telephone numbers in North America based on the  North American Numbering Plan
    /// </summary>
    /// <remarks>
    /// see [https://en.wikipedia.org/wiki/North_American_Numbering_Plan]
    /// </remarks>
    [Serializable]
    public class NorthAmericanPhone : Phone
    {
        #region fields
        private string _areaCode;
        private string _centralOfficeCode;
        private string _subscriberCode;
        #endregion

        #region inner types
        [Serializable]
        public enum PhoneCodes
        {
            AreaCode,
            CentralOfficeCode,
            SubscriberCode
        }
        #endregion

        #region ctor

        public NorthAmericanPhone(params Tuple<PhoneCodes, string>[] parts)
        {
            _areaCode = parts.FirstOrDefault(x => x.Item1 == PhoneCodes.AreaCode)?.Item2 ?? GetRandomAreaCode();
            _centralOfficeCode = parts.FirstOrDefault(x => x.Item1 == PhoneCodes.CentralOfficeCode)?.Item2 ??
                                 GetRandomCentralOfficeCode();
            _subscriberCode = parts.FirstOrDefault(x => x.Item1 == PhoneCodes.SubscriberCode)?.Item2 ??
                              $"{Etx.MyRand.Next(1, 9999):0000}";
            if (!IsValid(AreaCode, 3))
                _areaCode = GetRandomAreaCode();
            if (!IsValid(CentralOfficeCode, 3))
                _centralOfficeCode = GetRandomCentralOfficeCode();
            if (!IsValid(SubscriberNumber, 4))
                _subscriberCode = $"{Etx.MyRand.Next(1, 9999):0000}";
        }
        public NorthAmericanPhone()
        {
            _areaCode = GetRandomAreaCode();
            _centralOfficeCode = GetRandomCentralOfficeCode();
            _subscriberCode = $"{Etx.MyRand.Next(1, 9999):0000}";
        }
        #endregion

        #region properties
        /// <summary>
        /// The first 3 digits of a telephone number
        /// </summary>
        public string AreaCode => _areaCode;

        /// <summary>
        /// The middle 3 digits of a telephone number
        /// </summary>
        public string CentralOfficeCode => _centralOfficeCode;

        /// <summary>
        /// The last 4 digits of a telephone number
        /// </summary>
        public string SubscriberNumber => _subscriberCode;

        public static IEnumerable<string> TollFreeAreaCodes = new[] { "800", "888", "877", "866", "855", "844" };

        public override string Value
        {
            get => Formatted;
            set
            {
                if(!TryParse(value, out var phout))
                    throw new InvalidOperationException($"Cannot parse the value '{value}' into " +
                                                        "a North American phone number");
                _areaCode = phout.AreaCode;
                _centralOfficeCode = phout.CentralOfficeCode;
                _subscriberCode = phout.SubscriberNumber;
            }
        }

        public override string Abbrev => "PH";

        /// <summary>
        /// Formatted as (###) ###-####
        /// the typical &apos;1&apos; before the area code is never included
        /// </summary>
        public override string Formatted => $"({AreaCode}) {CentralOfficeCode}-{SubscriberNumber}";

        /// <summary>
        /// All 10 digits of the telephone number with no additional chars
        /// </summary>
        public override string Unformatted => $"{AreaCode}{CentralOfficeCode}{SubscriberNumber}";

        public override string Notes { get; set; }
        #endregion

        #region methods
        public virtual Uri ToUri()
        {
            return new Uri("tel:" + Unformatted);
        }

        public override bool Equals(object obj)
        {
            var ph = obj as NorthAmericanPhone;
            if (ph == null)
                return false;

            var p = new[]
            {
                string.Equals(ph.AreaCode, AreaCode, StringComparison.OrdinalIgnoreCase),
                string.Equals(ph.CentralOfficeCode, CentralOfficeCode, StringComparison.OrdinalIgnoreCase),
                string.Equals(ph.SubscriberNumber, SubscriberNumber, StringComparison.OrdinalIgnoreCase)
            };
            return p.All(x => x);
        }

        public override int GetHashCode()
        {
            return (AreaCode?.ToLower().GetHashCode() ?? 0) +
                   (CentralOfficeCode?.ToLower().GetHashCode() ?? 0) +
                   (SubscriberNumber?.ToLower().GetHashCode() ?? 0);
        }

        /// <summary>
        /// Attempts to parse a string into a <see cref="NorthAmericanPhone"/>
        /// </summary>
        /// <param name="phNumber"></param>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static bool TryParse(string phNumber, out NorthAmericanPhone phone)
        {
            string con;
            string sn;
            if (string.IsNullOrWhiteSpace(phNumber))
            {
                phone = null;
                return false;
            }

            var numerials = new List<char>();
            foreach (var c in phNumber.ToCharArray())
            {
                if (!char.IsNumber(c))
                    continue;
                numerials.Add(c);
            }

            if (numerials.Count < 7)
            {
                phone = null;
                return false;
            }

            if (numerials.Count == 7)
            {
                con = new string(numerials.GetRange(0, 3).ToArray());
                sn = new string(numerials.GetRange(3, 4).ToArray());
                phone = new NorthAmericanPhone(new Tuple<PhoneCodes, string>(PhoneCodes.CentralOfficeCode, con),
                    new Tuple<PhoneCodes, string>(PhoneCodes.SubscriberCode, sn));
                return true;
            }

            if (numerials.Count == 11 && numerials[0] == '1')
                numerials = numerials.GetRange(1, 10);

            var ac = new string(numerials.GetRange(0, 3).ToArray());
            con = new string(numerials.GetRange(3, 3).ToArray());
            sn = new string(numerials.GetRange(6, 4).ToArray());
            phone = new NorthAmericanPhone(new Tuple<PhoneCodes, string>(PhoneCodes.AreaCode, ac),
                new Tuple<PhoneCodes, string>(PhoneCodes.CentralOfficeCode, con),
                new Tuple<PhoneCodes, string>(PhoneCodes.SubscriberCode, sn));

            return true;

        }

        /// <summary>
        /// Creates a random Area Code.
        /// Does not return any value ending in &apos;11&apos; nor
        /// any value in the <see cref="TollFreeAreaCodes"/>
        /// </summary>
        /// <returns></returns>
        protected internal static string GetRandomAreaCode()
        {
            var phstr = new StringBuilder();
            phstr.Append($"{Etx.MyRand.Next(2, 9)}");
            phstr.Append($"{Etx.MyRand.Next(12, 99):00}");
            var areaCode = phstr.ToString();
            if(TollFreeAreaCodes.Any(x => x == areaCode))
                areaCode = (Convert.ToInt32(areaCode) - 1).ToString(CultureInfo.InvariantCulture);

            return areaCode;
        }

        /// <summary>
        /// Creates a random Central Office Code.
        /// Does not return any value ending in &apos;11&apos;
        /// </summary>
        /// <returns></returns>
        protected internal static string GetRandomCentralOfficeCode()
        {
            var phstr = new StringBuilder();
            phstr.Append($"{Etx.MyRand.Next(2, 9)}");
            var subscriberRand = Etx.MyRand.Next(1, 100);
            switch (subscriberRand)
            {
                case 100:
                    phstr.Append("00");
                    break;
                case 1 - 49:
                    phstr.Append($"{Etx.MyRand.Next(1, 9)}");
                    phstr.Append($"{Etx.MyRand.Next(2, 9)}");
                    break;
                default:
                    phstr.Append($"{Etx.MyRand.Next(2, 9)}");
                    phstr.Append($"{Etx.MyRand.Next(1, 9)}");
                    break;
            }
            return phstr.ToString();
        }

        private bool IsValid(string v, int len)
        {
            return !string.IsNullOrWhiteSpace(v) && v.ToCharArray().All(char.IsDigit) && v.ToCharArray().Length == len;
        }
        #endregion
    }
}