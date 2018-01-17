using System;
using System.Text;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Gov.US.Nhtsa
{
    /// <summary>
    /// Represents the chars 1-3 of a VIN 
    /// see http://vpic.nhtsa.dot.gov/
    /// </summary>
    [Serializable]
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
        /// Defaults to <see cref="Vin.DF_DIGIT"/>
        /// </remarks>
        public override string ToString()
        {
            var str = new StringBuilder();
            str.Append(Country ?? Vin.DF_DIGIT);
            str.Append(RegionMaker ?? Vin.DF_DIGIT);
            str.Append(VehicleType ?? Vin.DF_DIGIT);
            return str.ToString();
        }

        public static WorldManufacturerId CreateRandomManufacturerId()
        {
            var wmiOut = new WorldManufacturerId();
            //JA-J0 Japan
            //KL-KR Korea
            //LA-L0 China
            //1A-10, 4A-40, 5A-50 US
            //3A-37 Mexico
            var pick = Etx.RandomInteger(1, 12);

            switch (pick)
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

            wmiOut.RegionMaker = Vin.GetRandomVinChar();

            pick = Etx.RandomInteger(1, 3);
            switch (pick)
            {
                case 1:
                    wmiOut.VehicleType = '2';
                    break;
                case 2:
                    wmiOut.VehicleType = '3';
                    break;
                case 3:
                    wmiOut.VehicleType = '7';
                    break;
            }

            return wmiOut;
        }
    }
}