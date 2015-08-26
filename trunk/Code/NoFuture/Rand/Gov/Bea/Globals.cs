using System.Collections.Generic;

namespace NoFuture.Rand.Gov.Bea
{
    /// <summary>
    /// Disclaimer: "This product uses the Bureau of Economic Analysis (BEA) Data API but is not endorsed or certified by BEA."
    /// </summary>
    public class Globals
    {
        private static List<BeaDataSet> _dataSets;

        internal const string ALL_STRING = "ALL";

        public static List<BeaDataSet> DataSets
        {
            get
            {
                if (_dataSets != null)
                    return _dataSets;

                _dataSets = new List<BeaDataSet>
                {
                    new GDPbyIndustry(),
                    new Iip(),
                    new Ita(),
                    new NIUnderlyingDetail(),
                    new Nipa(),
                    new RegionalData()
                };

                return _dataSets;
            }
        }
    }//end Globals
}//end NoFuture.Rand.Gov.Bea

namespace NoFuture.Rand.Gov.Bea.Parameters
{
    public class Frequency : BeaParameter
    {
        public override string Description { get; set; }
        public override string Val { get; set; }
        private static List<Frequency> _values;
        public static List<Frequency> Values
        {
            get
            {
                if (_values != null)
                    return _values;
                _values = new List<Frequency>
                {
                    new Frequency
                    {
                        Val = "A",
                        Description = "Annual"
                    },
                    new Frequency
                    {
                        Val = "Q",
                        Description = "Quarterly"
                    },
                    new Frequency
                    {
                        Val = "QNSA",
                        Description = "Quarterly not seasonally adjusted"
                    },
                    new Frequency
                    {
                        Val = "M",
                        Description = "Monthly"
                    }
                };
                return _values;
            }
        }
    }

    public class BeaYear : BeaParameter
    {
        public override string Description { get; set; }
        public override string Val { get; set; }
    }
}//end NoFuture.Rand.Gov.Bea.Parameters
