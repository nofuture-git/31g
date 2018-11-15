using System;
using System.Collections.Generic;
using System.Globalization;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Geo.US;
using NoFuture.Util.Core;

namespace NoFuture.Rand.Geo
{
    /// <inheritdoc cref="GeoBase"/>
    /// <summary>
    /// Base type representing the first half of a typical Postal Address
    /// </summary>
    [Serializable]
    public abstract class StreetPo : GeoBase, ICited, IObviate
    {
        protected StreetPo(AddressData d) : base(d)
        {
        }

        public virtual string Src { get; set; }

        /// <summary>
        /// Prints the address as it would appear as post marked.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var data = GetData();
            return Etc.DistillSpaces(string.Join(" ", data.ThoroughfareNumber, data.ThoroughfareDirectional, data.ThoroughfareName, 
                data.ThoroughfareType, data.SecondaryUnitDesignator,
                data.SecondaryUnitId).Trim());
        }

        public override bool Equals(object obj)
        {
            var addr = obj as StreetPo;
            return addr != null && GetData().Equals(addr.GetData());
        }

        public override int GetHashCode()
        {
            return GetData().GetHashCode();
        }

        public abstract IDictionary<string, object> ToData(KindsOfTextCase txtCase);

        /// <summary>
        /// Generates at random a street address in the typical American form
        /// like '1600 Pennesylvania Ave'.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// The street type is limited to ten choices or empty.
        /// The street name is one of top twenty in the United States.
        /// </remarks>
        [RandomFactory]
        public static UsStreetPo RandomAmericanStreet()
        {
            var addressData = new AddressData();
            var pickOne = Etx.MyRand.Next(0, 10);
            var pickAnother = Etx.MyRand.Next(0, 20);
            switch (pickOne)
            {
                case 0:
                    addressData.ThoroughfareType = "St";
                    break;
                case 1:
                    addressData.ThoroughfareType = "Rd";
                    break;
                case 2:
                    addressData.ThoroughfareType = "Blvd";
                    break;
                case 3:
                    addressData.ThoroughfareType = "Ln";
                    break;
                case 4:
                    addressData.ThoroughfareType = "Drive";
                    break;
                case 5:
                    addressData.ThoroughfareType = "Ct";
                    break;
                case 6:
                    addressData.ThoroughfareType = $"Unit #{Etx.MyRand.Next(1, 99)}";
                    break;
                case 7:
                    addressData.ThoroughfareType = "Hwy";
                    break;
                case 8:
                    addressData.ThoroughfareType = "Avenue";
                    break;
                case 9:
                    addressData.ThoroughfareType = $"Alt {Etx.MyRand.Next(0, 99):000}";
                    break;
                default:
                    addressData.ThoroughfareType = string.Empty;
                    break;
            }

            switch (pickAnother)
            {
                case 0:
                    addressData.ThoroughfareName = "Second";
                    break;
                case 1:
                    addressData.ThoroughfareName = "Third";
                    break;
                case 2:
                    addressData.ThoroughfareName = "First";
                    break;
                case 3:
                    addressData.ThoroughfareName = "Fourth";
                    break;
                case 4:
                    addressData.ThoroughfareName = "Park";
                    break;
                case 5:
                    addressData.ThoroughfareName = "Main";
                    break;
                case 6:
                    addressData.ThoroughfareName = "Sixth";
                    break;
                case 7:
                    addressData.ThoroughfareName = "Oak";
                    break;
                case 8:
                    addressData.ThoroughfareName = "Seventh";
                    break;
                case 9:
                    addressData.ThoroughfareName = "Pine";
                    break;
                case 10:
                    addressData.ThoroughfareName = "Maple";
                    break;
                case 11:
                    addressData.ThoroughfareName = "Cedar";
                    break;
                case 12:
                    addressData.ThoroughfareName = "Eighth";
                    break;
                case 13:
                    addressData.ThoroughfareName = "Elm";
                    break;
                case 14:
                    addressData.ThoroughfareName = "View";
                    break;
                case 15:
                    addressData.ThoroughfareName = "Washington";
                    break;
                case 16:
                    addressData.ThoroughfareName = "Ninth";
                    break;
                case 17:
                    addressData.ThoroughfareName = "Lake";
                    break;
                case 18:
                    addressData.ThoroughfareName = "Hill";
                    break;
                case 19:
                    addressData.ThoroughfareName = "Manor";
                    break;
                case 20:
                    addressData.ThoroughfareName = "Jefferson";
                    break;
                default:
                    addressData.ThoroughfareName = string.Empty;
                    break;
            }

            addressData.ThoroughfareNumber = Etx.MyRand.Next(0, 999).ToString(CultureInfo.InvariantCulture);

            return new UsStreetPo(addressData);
        }
    }
}
