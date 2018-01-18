using System;
using System.Globalization;
using NoFuture.Rand.Core;
using NoFuture.Rand.Geo.US;

namespace NoFuture.Rand.Geo
{
    /// <summary>
    /// Base type representing the first half of a typical Postal Address
    /// </summary>
    [Serializable]
    public abstract class StreetPo : ICited
    {
        protected readonly AddressData data;

        protected StreetPo(AddressData d) { data = d; }

        public virtual string Src { get; set; }
        public AddressData Data => data;

        /// <summary>
        /// Prints the address as it would appear as post marked.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Join(" ", data.AddressNumber, data.StreetNameDirectional, data.StreetName, 
                data.StreetType, data.SecondaryUnitDesignator,
                data.SecondaryUnitId).Trim();
        }

        public override bool Equals(object obj)
        {
            var addr = obj as StreetPo;
            return addr != null && Data.Equals(addr.Data);
        }

        public override int GetHashCode()
        {
            return Data.GetHashCode();
        }

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
                    addressData.StreetType = "St";
                    break;
                case 1:
                    addressData.StreetType = "Rd";
                    break;
                case 2:
                    addressData.StreetType = "Blvd";
                    break;
                case 3:
                    addressData.StreetType = "Ln";
                    break;
                case 4:
                    addressData.StreetType = "Drive";
                    break;
                case 5:
                    addressData.StreetType = "Ct";
                    break;
                case 6:
                    addressData.StreetType = $"Unit #{Etx.MyRand.Next(1, 99)}";
                    break;
                case 7:
                    addressData.StreetType = "Hwy";
                    break;
                case 8:
                    addressData.StreetType = "Avenue";
                    break;
                case 9:
                    addressData.StreetType = $"Alt {Etx.MyRand.Next(0, 99):000}";
                    break;
                default:
                    addressData.StreetType = string.Empty;
                    break;
            }

            switch (pickAnother)
            {
                case 0:
                    addressData.StreetName = "Second";
                    break;
                case 1:
                    addressData.StreetName = "Third";
                    break;
                case 2:
                    addressData.StreetName = "First";
                    break;
                case 3:
                    addressData.StreetName = "Fourth";
                    break;
                case 4:
                    addressData.StreetName = "Park";
                    break;
                case 5:
                    addressData.StreetName = "Main";
                    break;
                case 6:
                    addressData.StreetName = "Sixth";
                    break;
                case 7:
                    addressData.StreetName = "Oak";
                    break;
                case 8:
                    addressData.StreetName = "Seventh";
                    break;
                case 9:
                    addressData.StreetName = "Pine";
                    break;
                case 10:
                    addressData.StreetName = "Maple";
                    break;
                case 11:
                    addressData.StreetName = "Cedar";
                    break;
                case 12:
                    addressData.StreetName = "Eighth";
                    break;
                case 13:
                    addressData.StreetName = "Elm";
                    break;
                case 14:
                    addressData.StreetName = "View";
                    break;
                case 15:
                    addressData.StreetName = "Washington";
                    break;
                case 16:
                    addressData.StreetName = "Ninth";
                    break;
                case 17:
                    addressData.StreetName = "Lake";
                    break;
                case 18:
                    addressData.StreetName = "Hill";
                    break;
                case 19:
                    addressData.StreetName = "Manor";
                    break;
                case 20:
                    addressData.StreetName = "Jefferson";
                    break;
                default:
                    addressData.StreetName = string.Empty;
                    break;
            }

            addressData.AddressNumber = Etx.MyRand.Next(0, 999).ToString(CultureInfo.InvariantCulture);

            return new UsStreetPo(addressData);

        }
    }
}
