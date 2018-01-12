using System;
using System.Text;

namespace NoFuture.Rand.Gov.Nhtsa
{
    /// <summary>
    /// Represents the chars 4-8 of a VIN
    /// see http://vpic.nhtsa.dot.gov/
    /// </summary>
    [Serializable]
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
        /// Defaults to <see cref="Vin.DF_DIGIT"/>
        /// </remarks>
        public override string ToString()
        {
            var str = new StringBuilder();
            str.Append(Four ?? Vin.DF_DIGIT);
            str.Append(Five ?? Vin.DF_DIGIT);
            str.Append(Six ?? Vin.DF_DIGIT);
            str.Append(Seven ?? Vin.DF_LETTER);
            str.Append(Eight ?? Vin.DF_DIGIT);
            return str.ToString();
        }
    }
}