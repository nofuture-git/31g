using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Data.Sp;

namespace NoFuture.Rand.Domus.Opes.Options
{
    public class OpesOptions
    {
        private double _derivativeSlope;

        public bool HasVehicle { get; set; }
        public bool IsVehiclePaidOff { get; set; }
        public bool IsRenting { get; set; }
        public int NumberOfVehicles { get; set; }
        public int NumberOfCreditCards { get; set; }

        public bool HasCreditCards => NumberOfCreditCards > 0;
        public bool HasVehicles => NumberOfVehicles > 0;
        public bool HasChildren => ChildrenAges != null && ChildrenAges.Any();

        public Pecuniam SumTotal { get; set; }
        public List<IMereo> GivenDirectly { get; } = new List<IMereo>();

        public List<int> ChildrenAges { get; set; } = new List<int>();

        public double DerivativeSlope
        {
            get
            {
                if (_derivativeSlope <= 0.0001 && _derivativeSlope >= -0.0001)
                    _derivativeSlope = -1.0D;

                return _derivativeSlope;
            }
            set => _derivativeSlope = value;
        }
    }
}
