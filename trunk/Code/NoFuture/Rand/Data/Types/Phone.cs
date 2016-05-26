using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml;

namespace NoFuture.Rand.Data.Types
{
    [Serializable]
    public abstract class Phone : Identifier
    {
        public abstract string Notes { get; set; }
        public abstract string Formatted { get; }
        public abstract string Unformatted { get; }
        /// <summary>
        /// Gets a <see cref="NorthAmericanPhone"/> whose Area Code, Central Office and 
        /// Subscriber number are all random values.
        /// </summary>
        /// <remarks>
        /// Area Codes are of grammer [2-9][1-9][2-9].
        /// Central Office are of grammer [2-9] ('0' '0' | [1-9] [2-9] | [2-9] [1-9] )
        /// Subscriber number is of grammer [0-9][0-9][0-9][1-9]
        /// </remarks>
        /// <returns></returns>
        public static NorthAmericanPhone American()
        {
            var usphone = new NorthAmericanPhone();

            var phstr = new StringBuilder();
            phstr.Append(string.Format("{0}", Etx.MyRand.Next(2, 9)));
            phstr.Append(string.Format("{0:00}", Etx.MyRand.Next(12, 99)));

            usphone.AreaCode = phstr.ToString();

            //check if randomly selected toll-free number
            if (usphone.AreaCode == "800" || 
                usphone.AreaCode == "888" || 
                usphone.AreaCode == "877" || 
                usphone.AreaCode == "866" || 
                usphone.AreaCode == "855" || 
                usphone.AreaCode == "844")
            {
                usphone.AreaCode = (Convert.ToInt32(usphone.AreaCode) - 1).ToString(CultureInfo.InvariantCulture);
            }
            phstr.Clear();

            phstr.Append(string.Format("{0}", Etx.MyRand.Next(2, 9)));
            var subscriberRand = Etx.MyRand.Next(1, 100);
            switch (subscriberRand)
            {
                case 100:
                    phstr.Append("00");
                    break;
                case 1-49:
                    phstr.Append(string.Format("{0}", Etx.MyRand.Next(1, 9)));
                    phstr.Append(string.Format("{0}", Etx.MyRand.Next(2, 9)));
                    break;
                default:
                    phstr.Append(string.Format("{0}", Etx.MyRand.Next(2, 9)));
                    phstr.Append(string.Format("{0}", Etx.MyRand.Next(1, 9)));
                    break;
            }
            usphone.CentralOfficeCode = phstr.ToString();
            usphone.SubscriberNumber = string.Format("{0:0000}", Etx.MyRand.Next(1, 9999));
            return usphone;
        }

        /// <summary>
        /// Gets a <see cref="NorthAmericanPhone"/> whose area code is pertinent 
        /// to the given US State.
        /// </summary>
        /// <param name="stateCode">The two-character US Postal Code for the given State.</param>
        /// <returns></returns>
        public static NorthAmericanPhone American(string stateCode)
        {
            var naPhone = American();
            var areaCode = GetAreaCode("us", stateCode);
            if (!string.IsNullOrWhiteSpace(areaCode))
                naPhone.AreaCode = areaCode;

            return naPhone;
        }

        /// <summary>
        /// Gets a <see cref="NorthAmericanPhone"/> whose area code is pertinent 
        /// to the given Canadian Providence.
        /// </summary>
        /// <param name="providenceCode">The two-character Canadian Postal Code for the given Providence.</param>
        /// <returns></returns>
        public static NorthAmericanPhone Canadian(string providenceCode)
        {
            var naPhone = American();
            var areaCode = GetAreaCode("ca", providenceCode);
            if (!string.IsNullOrWhiteSpace(areaCode))
                naPhone.AreaCode = areaCode;

            return naPhone;
        }

        //same code only the resource changes
        private static string GetAreaCode(string countryCode, string stateCode)
        {
            XmlNode state;
            if (string.IsNullOrWhiteSpace(countryCode))
                countryCode = "us";

            if(countryCode.ToLower() == "ca")
                state = Data.TreeData.CanadianAreaCodeData.SelectSingleNode(string.Format("area-codes/state[@abbreviation='{0}']", stateCode));
            else
                state = Data.TreeData.AmericanAreaCodeData.SelectSingleNode(string.Format("area-codes/state[@abbreviation='{0}']", stateCode));    
            
            if (state == null)
                return null;

            var areaCodes = state.ChildNodes;
            if (areaCodes.Count == 0)
                return null;

            var pickone = Etx.MyRand.Next(0, areaCodes.Count);
            return areaCodes[pickone].InnerText;
        }
    }
    [Serializable]
    public class NorthAmericanPhone : Phone
    {
        public string AreaCode { get; set; }
        public string CentralOfficeCode { get; set; }
        public string SubscriberNumber { get; set; }

        public override string Value
        {
            get
            {
                return Formatted;
            }
            set
            {
                NorthAmericanPhone phout;
                if(!TryParse(value, out phout))
                    throw new InvalidOperationException(string.Format("Cannot parse the value '{0}' into " +
                                                        "a North American phone number", value));
                AreaCode = phout.AreaCode;
                CentralOfficeCode = phout.CentralOfficeCode;
                SubscriberNumber = phout.SubscriberNumber;
            }
        }

        public override string Abbrev
        {
            get { return "PH"; }
        }

        public override string Formatted
        {
            get { return string.Format("({0}) {1}-{2}", AreaCode, CentralOfficeCode, SubscriberNumber); }
        }

        public override string Unformatted
        {
            get { return string.Format("{0}{1}{2}", AreaCode, CentralOfficeCode, SubscriberNumber); }
        }

        public virtual Uri ToUri()
        {
            return new Uri("tel:" + Unformatted);
        }

        public override string Notes { get; set; }

        public override bool Equals(object obj)
        {
            var ph = obj as NorthAmericanPhone;
            if (ph == null)
                return false;

            var p = new bool[]
            {
                string.Equals(ph.AreaCode, AreaCode, StringComparison.OrdinalIgnoreCase),
                string.Equals(ph.CentralOfficeCode, CentralOfficeCode, StringComparison.OrdinalIgnoreCase),
                string.Equals(ph.SubscriberNumber, SubscriberNumber, StringComparison.OrdinalIgnoreCase)
            };
            return p.All(x => x);
        }

        public override int GetHashCode()
        {
            return (AreaCode == null ? 0 : AreaCode.ToLower().GetHashCode()) +
                   (CentralOfficeCode == null ? 0 : CentralOfficeCode.ToLower().GetHashCode()) +
                   (SubscriberNumber == null ? 0 : SubscriberNumber.ToLower().GetHashCode());
        }

        public static bool TryParse(string phNumber, out NorthAmericanPhone phone)
        {
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
                phone = new NorthAmericanPhone
                        {
                            CentralOfficeCode = new string(numerials.GetRange(0, 3).ToArray()),
                            SubscriberNumber = new string(numerials.GetRange(3, 4).ToArray())
                        };
                return true;
            }

            if (numerials.Count == 11 && numerials[0] == '1')
                numerials = numerials.GetRange(1, 10);

            phone = new NorthAmericanPhone
            {
                AreaCode = new string(numerials.GetRange(0, 3).ToArray()),
                CentralOfficeCode = new string(numerials.GetRange(3, 3).ToArray()),
                SubscriberNumber = new string(numerials.GetRange(6, 4).ToArray())
            };
            return true;

        }
    }
}
