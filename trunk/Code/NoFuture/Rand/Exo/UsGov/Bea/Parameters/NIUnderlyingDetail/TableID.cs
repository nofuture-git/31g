using System.Collections.Generic;

namespace NoFuture.Rand.Data.Exo.UsGov.Bea.Parameters.NIUnderlyingDetail
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
                               Description = "Table 1AU. Real Manufacturing and Trade Inventories, Seasonally Adjusted, End of Period [Chained 1996 dollars, 1967-96, SIC] (Q) (M)",
                               Val = "1",
                           },
                           new TableID
                           {
                               Description = "Table 1AU2. Real Manufacturing and Trade Inventories, Seasonally Adjusted, End of Period [Chained 2009 dollars, 1967-97, SIC] (Q) (M)",
                               Val = "12",
                           },
                           new TableID
                           {
                               Description = "Table 1BU. Real Manufacturing and Trade Inventories, Seasonally Adjusted, End of Period [Chained 2009 dollars, 1997 forward, NAICS] (A) (Q) (M)",
                               Val = "23",
                           },
                           new TableID
                           {
                               Description = "Table 1BUC. Current Dollar Manufacturing and Trade Inventories, Seasonally Adjusted, End of Period [1997 forward, NAICS] (A) (Q) (M)",
                               Val = "34",
                           },
                           new TableID
                           {
                               Description = "Table 2AU. Real Manufacturing and Trade Sales, Seasonally Adjusted at Monthly Rate [Chained 1996 dollars, 1967-96, SIC] (Q) (M)",
                               Val = "45",
                           },
                           new TableID
                           {
                               Description = "Table 2AUI. Implicit Price Deflators for Manufacturing and Trade Sales [Index base 1996, 1967-96, SIC] (Q) (M)",
                               Val = "56",
                           },
                           new TableID
                           {
                               Description = "Table 2BU. Real Manufacturing and Trade Sales, Seasonally Adjusted at Monthly Rate [Chained 2009 dollars, 1997 forward, NAICS] (A) (Q) (M)",
                               Val = "59",
                           },
                           new TableID
                           {
                               Description = "Table 2BUI. Implicit Price Deflators for Manufacturing and Trade Sales [Index base 2009, 1997 forward, NAICS] (A) (Q) (M)",
                               Val = "60",
                           },
                           new TableID
                           {
                               Description = "Table 3AU. Real Inventory-Sales Ratios for Manufacturing and Trade, Seasonally Adjusted [Based on chained 1996 dollars, 1967-96, SIC] (Q) (M)",
                               Val = "61",
                           },
                           new TableID
                           {
                               Description = "Table 3BU. Real Inventory-Sales Ratios for Manufacturing and Trade, Seasonally Adjusted [Based on chained 2009 dollars, 1997 forward, NAICS] (A) (Q) (M)",
                               Val = "2",
                           },
                           new TableID
                           {
                               Description = "Table 4AU1. Real Manufacturing Inventories, by Stage of Fabrication (Materials and supplies), Seasonally Adjusted, End of Period [Chained 2009 dollars, 1967-97, SIC] (Q) (M)",
                               Val = "3",
                           },
                           new TableID
                           {
                               Description = "Table 4AU2. Real Manufacturing Inventories, by Stage of Fabrication, Seasonally Adjusted (Work-in-process), End of Period [Chained 2009 dollars, 1967-97, SIC] (Q) (M)",
                               Val = "4",
                           },
                           new TableID
                           {
                               Description = "Table 4AU3. Real Manufacturing Inventories, by Stage of Fabrication (Finished goods), Seasonally Adjusted, End of Period [Chained 2009 dollars, 1967-97, SIC] (Q) (M)",
                               Val = "5",
                           },
                           new TableID
                           {
                               Description = "Table 4BU1. Real Manufacturing Inventories, by Stage of Fabrication (Materials and supplies), Seasonally Adjusted, End of Period [Chained 2009 dollars, 1997 forward, NAICS] (A) (Q) (M)",
                               Val = "6",
                           },
                           new TableID
                           {
                               Description = "Table 4BU2. Real Manufacturing Inventories, by Stage of Fabrication (Work-in-process), Seasonally Adjusted, End of Period [Chained 2009 dollars, 1997 forward, NAICS] (A) (Q) (M)",
                               Val = "7",
                           },
                           new TableID
                           {
                               Description = "Table 4BU3. Real Manufacturing Inventories, by Stage of Fabrication (Finished goods), Seasonally Adjusted, End of Period [Chained 2009 dollars, 1997 forward, NAICS] (A) (Q) (M)",
                               Val = "8",
                           },
                           new TableID
                           {
                               Description = "Table 5U. BEA Retail and Food Service Sales (A) (Q) (M)",
                               Val = "9",
                           },
                           new TableID
                           {
                               Description = "Table 6U. Real BEA Retail and Food Service Sales (A) (Q) (M)",
                               Val = "10",
                           },
                           new TableID
                           {
                               Description = "Table 7U. Chain-Type Price Indexes for BEA Retail and Food Service Sales (A) (Q) (M)",
                               Val = "11",
                           },
                           new TableID
                           {
                               Description = "Table 2.3.4U. Price Indexes for Personal Consumption Expenditures by Major Type of Product and by Major Function (A) (Q) (M)",
                               Val = "13",
                           },
                           new TableID
                           {
                               Description = "Table 2.3.5U. Personal Consumption Expenditures by Major Type of Product and by Major Function (A) (Q) (M)",
                               Val = "14",
                           },
                           new TableID
                           {
                               Description = "Table 2.3.6U. Real Personal Consumption Expenditures by Major Type of Product and by Major Function (A) (Q) (M)",
                               Val = "15",
                           },
                           new TableID
                           {
                               Description = "Table 2.4.3U. Real Personal Consumption Expenditures by Type of Product, Quantity Indexes (A) (Q) (M)",
                               Val = "74",
                           },
                           new TableID
                           {
                               Description = "Table 2.4.4U. Price Indexes for Personal Consumption Expenditures by Type of Product (A) (Q) (M)",
                               Val = "16",
                           },
                           new TableID
                           {
                               Description = "Table 2.4.5U. Personal Consumption Expenditures by Type of Product (A) (Q) (M)",
                               Val = "17",
                           },
                           new TableID
                           {
                               Description = "Table 2.4.6U. Real Personal Consumption Expenditures by Type of Product, Chained Dollars (A) (Q) (M)",
                               Val = "18",
                           },
                           new TableID
                           {
                               Description = "Table 3.4U. Personal Current Tax Receipts (Q)",
                               Val = "19",
                           },
                           new TableID
                           {
                               Description = "Table 3.5U. Taxes on Production and Imports (Q)",
                               Val = "20",
                           },
                           new TableID
                           {
                               Description = "Table 3.6U. Contributions for Government Social Insurance (Q)",
                               Val = "21",
                           },
                           new TableID
                           {
                               Description = "Table 3.7U. Government Current Transfer Receipts (Q)",
                               Val = "22",
                           },
                           new TableID
                           {
                               Description = "Table 3.8U. Current Surplus of Government Enterprises (Q)",
                               Val = "24",
                           },
                           new TableID
                           {
                               Description = "Table 3.12U. Government Social Benefits (Q)",
                               Val = "25",
                           },
                           new TableID
                           {
                               Description = "Table 3.13U. Subsidies (Q)",
                               Val = "26",
                           },
                           new TableID
                           {
                               Description = "Table 3.24U. Federal Grants-in-Aid to State and Local Governments (Q)",
                               Val = "27",
                           },
                           new TableID
                           {
                               Description = "Table 3.25U. Compensation of General Government Employees (A)",
                               Val = "28",
                           },
                           new TableID
                           {
                               Description = "Table 4.2.3U. Real Exports and Imports of Goods and Services by Type of Product, Quantity Indexes (A) (Q)",
                               Val = "114",
                           },
                           new TableID
                           {
                               Description = "Table 4.2.4U. Price Indexes for Exports and Imports of Goods and Services by Type of Product (A) (Q)",
                               Val = "111",
                           },
                           new TableID
                           {
                               Description = "Table 4.2.5U. Exports and Imports of Goods and Services by Type of Product (A) (Q)",
                               Val = "113",
                           },
                           new TableID
                           {
                               Description = "Table 4.2.6U. Real Exports and Imports of Goods and Services by Type of Product, Chained Dollars (A) (Q)",
                               Val = "112",
                           },
                           new TableID
                           {
                               Description = "Table 4.3BU. Relation of Foreign Transactions in the National Income and Product Accounts to the Corresponding Items in the International Transactions Accounts (Q)",
                               Val = "73",
                           },
                           new TableID
                           {
                               Description = "Table 5.2.3U. Real Gross and Net Domestic Investment by Major Type, Quantity Indexes (Q)",
                               Val = "70",
                           },
                           new TableID
                           {
                               Description = "Table 5.2.5U. Gross and Net Domestic Investment by Major Type (Q)",
                               Val = "71",
                           },
                           new TableID
                           {
                               Description = "Table 5.2.6U. Real Gross and Net Domestic Investment by Major Type, Chained Dollars (Q)",
                               Val = "72",
                           },
                           new TableID
                           {
                               Description = "Table 5.4.4U. Price Indexes for Private Fixed Investment in Structures by Type (A) (Q)",
                               Val = "29",
                           },
                           new TableID
                           {
                               Description = "Table 5.4.5U. Private Fixed Investment in Structures by Type (A) (Q)",
                               Val = "30",
                           },
                           new TableID
                           {
                               Description = "Table 5.4.6U. Real Private Fixed Investment in Structures by Type, Chained Dollars (A) (Q)",
                               Val = "31",
                           },
                           new TableID
                           {
                               Description = "Table 5.5.4U. Price Indexes for Private Fixed Investment in Equipment by Type (A) (Q)",
                               Val = "32",
                           },
                           new TableID
                           {
                               Description = "Table 5.5.5U. Private Fixed Investment in Equipment by Type (A) (Q)",
                               Val = "33",
                           },
                           new TableID
                           {
                               Description = "Table 5.5.6U. Real Private Fixed Investment in Equipment by Type, Chained Dollars (A) (Q)",
                               Val = "35",
                           },
                           new TableID
                           {
                               Description = "Table 5.7.5AM1. Change in Private Inventories by Industry (M)",
                               Val = "81",
                           },
                           new TableID
                           {
                               Description = "Table 5.7.5AM2. Change in Book Value by Industry (M)",
                               Val = "82",
                           },
                           new TableID
                           {
                               Description = "Table 5.7.5AM3. Inventory Valuation Adjustment by Industry (M)",
                               Val = "83",
                           },
                           new TableID
                           {
                               Description = "Table 5.7.5AU1. Change in Private Inventories by Industry (Q)",
                               Val = "84",
                           },
                           new TableID
                           {
                               Description = "Table 5.7.5AU2. Change in Book Value by Industry (Q)",
                               Val = "85",
                           },
                           new TableID
                           {
                               Description = "Table 5.7.5AU3. Inventory Valuation Adjustment by Industry (Q)",
                               Val = "86",
                           },
                           new TableID
                           {
                               Description = "Table 5.7.5BM1. Change in Private Inventories by Industry (M)",
                               Val = "87",
                           },
                           new TableID
                           {
                               Description = "Table 5.7.5BM2. Change in Book Value by Industry (M)",
                               Val = "88",
                           },
                           new TableID
                           {
                               Description = "Table 5.7.5BM3. Inventory Valuation Adjustment by Industry (M)",
                               Val = "89",
                           },
                           new TableID
                           {
                               Description = "Table 5.7.5BU1. Change in Private Inventories by Industry (A) (Q)",
                               Val = "90",
                           },
                           new TableID
                           {
                               Description = "Table 5.7.5BU2. Change in Book Value by Industry (A) (Q)",
                               Val = "91",
                           },
                           new TableID
                           {
                               Description = "Table 5.7.5BU3. Inventory Valuation Adjustment by Industry (A) (Q)",
                               Val = "92",
                           },
                           new TableID
                           {
                               Description = "Table 5.7.6AM. Change in Real Private Inventories by Industry (M)",
                               Val = "93",
                           },
                           new TableID
                           {
                               Description = "Table 5.7.6AU. Change in Real Private Inventories by Industry (Q)",
                               Val = "94",
                           },
                           new TableID
                           {
                               Description = "Table 5.7.6BM. Change in Real Private Inventories by Industry (M)",
                               Val = "95",
                           },
                           new TableID
                           {
                               Description = "Table 5.7.6BU. Change in Real Private Inventories by Industry (A) (Q)",
                               Val = "96",
                           },
                           new TableID
                           {
                               Description = "Table 5.11U. Capital Transfers Paid and Received, by Sector and by Type (Q)",
                               Val = "53",
                           },
                           new TableID
                           {
                               Description = "Table 7.2.4U. Price Indexes for Motor Vehicle Output (A) (Q)",
                               Val = "54",
                           },
                           new TableID
                           {
                               Description = "Table 7.2.5S. Auto and Truck Unit Sales, Production, Inventories, Expenditures, and Price (M)",
                               Val = "55",
                           },
                           new TableID
                           {
                               Description = "Table 7.2.5U. Motor Vehicle Output (A) (Q)",
                               Val = "57",
                           },
                           new TableID
                           {
                               Description = "Table 7.2.6U. Real Motor Vehicle Output, Chained Dollars (A) (Q)",
                               Val = "58",
                           },
                           new TableID
                           {
                               Description = "Table 9.1U. Reconciliation of Percent Change in the CPI with Percent Change in the PCE Price Index (Q) (M)",
                               Val = "75",
                           },
                           new TableID
                           {
                               Description = "Table 9.2U. Final Sales of Domestic Computers (A) (Q)",
                               Val = "76",
                           },
                           new TableID
                           {
                               Description = "Table 9.3U. Gross Domestic Product and Final Sales of Software (A)",
                               Val = "77",
                           },
                           new TableID
                           {
                               Description = "Table 9.4U. Software Investment and Prices (A)",
                               Val = "78",
                           },
                           new TableID
                           {
                               Description = "Table 9.5U. Contributions to Percent Change in Real Gross Domestic Product from Final Sales of Computers, Software, and Communications Equipment (A)",
                               Val = "79",
                           },

                       };
                return _values;
            }
        }
	}//end TableID
}//end NoFuture.Rand.Gov.Bea.Parameters.NIUnderlyingDetail