using System.Collections.Generic;

namespace NoFuture.Rand.Data.Exo.UsGov.Bea.Parameters.GDPbyIndustry
{
    public class TableID : BeaParameter
    {
        public override string Description { get; set; }
        public override string Val { get; set; }
        private static List<TableID> _values;
        public static List<TableID> Values
        {
            get
            {
                if (_values != null)
                    return _values;
                _values = new List<TableID>
                           {
                           
                           new TableID
                           {
                               Val = "1",
                               Description = "Value Added by Industry (A) (Q)",
                           },
                           new TableID
                           {
                               Val = "5",
                               Description = "Value Added by Industry as a Percentage of Gross Domestic Product (A) (Q)",
                           },
                           new TableID
                           {
                               Val = "6",
                               Description = "Components of Value Added by Industry (A)",
                           },
                           new TableID
                           {
                               Val = "7",
                               Description = "Components of Value Added by Industry as a Percentage of Value Added (A)",
                           },
                           new TableID
                           {
                               Val = "8",
                               Description = "Chain-Type Quantity Indexes for Value Added by Industry (A) (Q)",
                           },
                           new TableID
                           {
                               Val = "9",
                               Description = "Percent Changes in Chain-Type Quantity Indexes for Value Added by Industry (A) (Q)",
                           },
                           new TableID
                           {
                               Val = "10",
                               Description = "Real Value Added by Industry (A) (Q)",
                           },
                           new TableID
                           {
                               Val = "11",
                               Description = "Chain-Type Price Indexes for Value Added by Industry (A) (Q)",
                           },
                           new TableID
                           {
                               Val = "12",
                               Description = "Percent Changes in Chain-Type Price Indexes for Value Added by Industry (A) (Q)",
                           },
                           new TableID
                           {
                               Val = "13",
                               Description = "Contributions to Percent Change in Real Gross Domestic Product by Industry (A) (Q)",
                           },
                           new TableID
                           {
                               Val = "14",
                               Description = "Contributions to Percent Change in the Chain-Type Price Index for Gross Domestic Product by Industry (A) (Q)",
                           },
                           new TableID
                           {
                               Val = "15",
                               Description = "Gross Output by Industry (A) (Q)",
                           },
                           new TableID
                           {
                               Val = "16",
                               Description = "Chain-Type Quantity Indexes for Gross Output by Industry (A) (Q)",
                           },
                           new TableID
                           {
                               Val = "17",
                               Description = "Percent Changes in Chain-Type Quantity Indexes for Gross Output by Industry (A) (Q)",
                           },
                           new TableID
                           {
                               Val = "18",
                               Description = "Chain-Type Price Indexes for Gross Output by Industry (A) (Q)",
                           },
                           new TableID
                           {
                               Val = "19",
                               Description = "Percent Changes in Chain-Type Price Indexes for Gross Output by Industry (A) (Q)",
                           },
                           new TableID
                           {
                               Val = "20",
                               Description = "Intermediate Inputs by Industry (A) (Q)",
                           },
                           new TableID
                           {
                               Val = "21",
                               Description = "Chain-Type Quantity Indexes for Intermediate Inputs by Industry (A) (Q)",
                           },
                           new TableID
                           {
                               Val = "22",
                               Description = "Percent Changes in Chain-Type Quantity Indexes for Intermediate Inputs by Industry (A) (Q)",
                           },
                           new TableID
                           {
                               Val = "23",
                               Description = "Chain-Type Price Indexes for Intermediate Inputs by Industry (A) (Q)",
                           },
                           new TableID
                           {
                               Val = "24",
                               Description = "Percent Changes in Chain-Type Price Indexes for Intermediate Inputs by Industry (A) (Q)",
                           },
                           new TableID
                           {
                               Val = "25",
                               Description = "Composition of Gross Output by Industry (A)",
                           },
                           new TableID
                           {
                               Val = "26",
                               Description = "Shares of Gross Output by Industry (A)",
                           },
                           new TableID
                           {
                               Val = "27",
                               Description = "Cost per Unit of Real Gross Output by Industry Group (A)",
                           },
                           new TableID
                           {
                               Val = "29",
                               Description = "Contributions to Percent Change in Chain-Type Quantity Indexes for Gross Output by Industry Group (A)",
                           },
                           new TableID
                           {
                               Val = "30",
                               Description = "Contributions to Percent Change in Chain-Type Price Indexes for Gross Output by Industry Group (A)",
                           },
                           new TableID
                           {
                               Val = "31",
                               Description = "Chain-Type Quantity Indexes for Energy Inputs by Industry (A)",
                           },
                           new TableID
                           {
                               Val = "32",
                               Description = "Contributions to Percent Change by Industry in the Chain-Type Quantity Index for All Industries Energy Inputs (A)",
                           },
                           new TableID
                           {
                               Val = "33",
                               Description = "Chain-Type Price Indexes for Energy Inputs by Industry (A)",
                           },
                           new TableID
                           {
                               Val = "34",
                               Description = "Contributions to Percent Change by Industry in the Chain-Type Price Index for All Industries Energy Inputs (A)",
                           },
                           new TableID
                           {
                               Val = "35",
                               Description = "Chain-Type Quantity Indexes for Materials Inputs by Industry (A)",
                           },
                           new TableID
                           {
                               Val = "36",
                               Description = "Contributions to Percent Change by Industry in the Chain-Type Quantity Index for All Industries Materials Inputs (A)",
                           },
                           new TableID
                           {
                               Val = "37",
                               Description = "Chain-Type Price Indexes for Materials Inputs by Industry (A)",
                           },
                           new TableID
                           {
                               Val = "38",
                               Description = "Contributions to Percent Change by Industry in the Chain-Type Price Index for All Industries Materials Inputs (A)",
                           },
                           new TableID
                           {
                               Val = "39",
                               Description = "Chain-Type Quantity Indexes for Purchased Service Inputs by Industry (A)",
                           },
                           new TableID
                           {
                               Val = "40",
                               Description = "Contributions to Percent Change by Industry in the Chain-Type Quantity Index for All Industries Purchased Service Inputs (A)",
                           },
                           new TableID
                           {
                               Val = "41",
                               Description = "Chain-Type Price Indexes for Purchased Service Inputs by Industry (A)",
                           },
                           new TableID
                           {
                               Val = "42",
                               Description = "Contributions to Percent Change by Industry in the Chain-Type Price Index for All Industries Purchased Service Inputs (A)",
                           },

                       };
                return _values;
            }
        }
	}//end TableID
}//end NoFuture.Rand.Gov.Bea.Parameters.GDPbyIndustry