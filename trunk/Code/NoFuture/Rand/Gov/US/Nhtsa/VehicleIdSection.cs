using System;
using System.Text;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Gov.US.Nhtsa
{
    /// <summary>
    /// Represents the chars 10-17 of a VIN 
    /// see http://vpic.nhtsa.dot.gov/
    /// </summary>
    [Serializable]
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
        /// Defaults to <see cref="Vin.DF_DIGIT"/>
        /// </remarks>
        public override string ToString()
        {
            var str = new StringBuilder();
            str.Append(ModelYear ?? Vin.DF_DIGIT);
            str.Append(PlantCode ?? Vin.DF_DIGIT);
            str.Append(SequentialNumber ?? new string(Vin.DF_DIGIT, 6));
            return str.ToString();
        }

        /// <summary>
        /// Random gen for the vehicle id section 
        /// </summary>
        /// <param name="isLateModel"></param>
        /// <param name="maxYear"></param>
        /// <returns></returns>
        public static VehicleIdSection GetVehicleIdSection(bool isLateModel = false, int? maxYear = null)
        {

            var yy = maxYear ?? DateTime.Today.Year;

            if (yy < Vin.BASE_YEAR)
                yy = Vin.BASE_YEAR;

            var pick = Etx.RandomInteger(0, yy - Vin.AMENDED_BASE_YEAR); 

            if (isLateModel)
                pick = Etx.RandomInteger(0, Vin.YearIdx.Length-1);

            var vis = new VehicleIdSection
            {
                ModelYear = Vin.YearIdx[pick],
                PlantCode = Vin.GetRandomVinChar(),
                SequentialNumber = new string(Etx.RandomChars(0x30, 0x39, 6))
            };

            return vis;
        }
    }
}