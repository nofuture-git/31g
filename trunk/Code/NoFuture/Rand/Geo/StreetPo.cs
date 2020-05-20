using System;
using System.Collections.Generic;
using System.Globalization;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Geo.US;
using NoFuture.Util.Core;
using NfString = NoFuture.Util.Core.NfString;

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
            return NfString.DistillSpaces(string.Join(" ", data.ThoroughfareNumber, data.ThoroughfareDirectional, data.ThoroughfareName, 
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
        /// like &quot;1600 Pennsylvania Ave&quot;.
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

            addressData.ThoroughfareNumber = Etx.RandomCoinToss()
                ? Etx.MyRand.Next(0, 2999).ToString(CultureInfo.InvariantCulture)
                : Etx.MyRand.Next(0, 29999).ToString(CultureInfo.InvariantCulture);

            addressData.ThoroughfareName = UsStreetPo.RandomAmericanStreetName();
            if (!Etx.RandomRollBelowOrAt(6, Etx.Dice.OneHundred))
            {
                addressData.ThoroughfareType = UsStreetPo.RandomAmericanStreetKind();
            }

            if (Etx.RandomRollBelowOrAt(20, Etx.Dice.OneHundred))
            {
                addressData.ThoroughfareDirectional = Etx.RandomPickOne(UsStreetPo.UsPostalDirectionalAbbrev);
            }

            if (Etx.RandomRollBelowOrAt(13, Etx.Dice.OneHundred))
            {
                addressData.SecondaryUnitDesignator = UsStreetPo.RandomAmericanAddressLine2();
            }

            return new UsStreetPo(addressData);
        }
    }
}
