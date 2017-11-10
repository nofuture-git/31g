using System;
using System.Text;
using NoFuture.Rand.Core;
using NoFuture.Rand.Data;

namespace NoFuture.Rand.Gov.Nhtsa
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

        public static Tuple<WorldManufacturerId, string> GetRandomManufacturerId()
        {
            var df = new Tuple<WorldManufacturerId, string>(CreateRandomManufacturerId(), string.Empty);
            if (TreeData.VinWmi == null)
            {
                return df;
            }

            //pick the kind of vehicle
            var xml = TreeData.VinWmi;
            var wmiOut = new WorldManufacturerId();
            var xpath = "//vehicle-type";
            var pick = Etx.IntNumber(1, 3);
            switch (pick)
            {
                case 1:
                    xpath += "[@name='car']/wmi";
                    break;
                case 2:
                    xpath += "[@name='truck']/wmi";
                    break;
                case 3:
                    xpath += "[@name='suv']/wmi";
                    break;
            }

            // pick a manufacturer for vehicle type
            var mfNodes = xml.SelectNodes(xpath);
            if (mfNodes == null || mfNodes.Count <= 0)
                return df;
            pick = Etx.IntNumber(0, mfNodes.Count - 1);
            var mfNode = mfNodes[pick];
            if (mfNode == null)
                return df;
            var mfName = mfNode.Attributes?["id"]?.Value;
            if (string.IsNullOrWhiteSpace(mfName))
                return df;
            xpath += $"[@id='{mfName}']";
            var wmiNodes = xml.SelectNodes(xpath + "/add");
            if (wmiNodes == null || wmiNodes.Count <= 0)
                return df;
            pick = Etx.IntNumber(0, wmiNodes.Count - 1);
            var wmiNode = wmiNodes[pick];
            if (wmiNode == null)
                return df;
            var wmiStr = wmiNode.Attributes?["value"]?.Value;
            if (string.IsNullOrWhiteSpace(wmiStr) || wmiStr.Length != 3)
                return df;
            var wmiChars = wmiStr.ToCharArray();
            wmiOut.Country = wmiChars[0];
            wmiOut.RegionMaker = wmiChars[1];
            wmiOut.VehicleType = wmiChars[2];

            df = new Tuple<WorldManufacturerId, string>(wmiOut, string.Empty);

            //pick a vehicle's common name
            xpath += "/vehicle-names/add";
            var vhNameNodes = xml.SelectNodes(xpath);
            if (vhNameNodes == null || vhNameNodes.Count <= 0)
                return df;

            pick = Etx.IntNumber(0, vhNameNodes.Count - 1);
            var vhNameNode = vhNameNodes[pick];
            if (vhNameNode == null)
                return df;

            var vhName = vhNameNode.Attributes?["value"]?.Value;
            if (string.IsNullOrWhiteSpace(vhName))
                return df;

            return new Tuple<WorldManufacturerId, string>(wmiOut, vhName);
        }

        internal static WorldManufacturerId CreateRandomManufacturerId()
        {
            var wmiOut = new WorldManufacturerId();
            //JA-J0 Japan
            //KL-KR Korea
            //LA-L0 China
            //1A-10, 4A-40, 5A-50 US
            //3A-37 Mexico
            var pick = Etx.IntNumber(1, 12);

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

            pick = Etx.IntNumber(1, 3);
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