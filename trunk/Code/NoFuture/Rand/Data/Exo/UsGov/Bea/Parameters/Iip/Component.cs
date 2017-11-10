using System.Collections.Generic;

namespace NoFuture.Rand.Data.Exo.UsGov.Bea.Parameters.Iip
{
    public class Component : BeaParameter
    {
        public override string Description { get; set; }
        public override string Val { get; set; }
        private static List<Component> _values;
        public static List<Component> Values
        {
            get
            {
                if (_values != null)
                    return _values;
                _values = new List<Component>
                           {
                           
                           new Component
                           {
                               Val = "ChgPos",
                               Description = "Change in position",
                           },
                           new Component
                           {
                               Val = "ChgPosNie",
                               Description = "Change in position attributable to changes in volume and valuation n.i.e.",
                           },
                           new Component
                           {
                               Val = "ChgPosOth",
                               Description = "Change in position not attributable to financial-account transactions",
                           },
                           new Component
                           {
                               Val = "ChgPosPrice",
                               Description = "Change in position attributable to price changes",
                           },
                           new Component
                           {
                               Val = "ChgPosTrans",
                               Description = "Change in position attributable to financial-account transactions",
                           },
                           new Component
                           {
                               Val = "ChgPosXRate",
                               Description = "Change in position attributable to exchange-rate changes",
                           },
                           new Component
                           {
                               Val = "Pos",
                               Description = "Position",
                           },

                       };
                return _values;
            }
        }
	}//end Component
}//end NoFuture.Rand.Gov.Bea.Parameters.Iip