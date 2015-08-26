using System;
using System.Collections.Generic;
namespace NoFuture.Rand.Gov.Bea.Parameters.Nipa
{
    public class TableID : NoFuture.Rand.Gov.Bea.BeaParameter
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
                               Description = "Table 1.1.1. Percent Change From Preceding Period in Real Gross Domestic Product (A) (Q)",
                               Val = "1",
                           },
                           new TableID
                           {
                               Description = "Table 1.1.2. Contributions to Percent Change in Real Gross Domestic Product (A) (Q)",
                               Val = "2",
                           },
                           new TableID
                           {
                               Description = "Table 1.1.3. Real Gross Domestic Product, Quantity Indexes (A) (Q)",
                               Val = "3",
                           },
                           new TableID
                           {
                               Description = "Table 1.1.4. Price Indexes for Gross Domestic Product (A) (Q)",
                               Val = "4",
                           },
                           new TableID
                           {
                               Description = "Table 1.1.5. Gross Domestic Product (A) (Q)",
                               Val = "5",
                           },
                           new TableID
                           {
                               Description = "Table 1.1.6. Real Gross Domestic Product, Chained Dollars (A) (Q)",
                               Val = "6",
                           },
                           new TableID
                           {
                               Description = "Table 1.1.6A. Real Gross Domestic Product, Chained (1937) Dollars (A)",
                               Val = "7",
                           },
                           new TableID
                           {
                               Description = "Table 1.1.6B. Real Gross Domestic Product, Chained (1952) Dollars (A) (Q)",
                               Val = "8",
                           },
                           new TableID
                           {
                               Description = "Table 1.1.6C. Real Gross Domestic Product, Chained (1972) Dollars (A) (Q)",
                               Val = "9",
                           },
                           new TableID
                           {
                               Description = "Table 1.1.6D. Real Gross Domestic Product, Chained (1992) Dollars (A) (Q)",
                               Val = "10",
                           },
                           new TableID
                           {
                               Description = "Table 1.1.7. Percent Change From Preceding Period in Prices for Gross Domestic Product (A) (Q)",
                               Val = "11",
                           },
                           new TableID
                           {
                               Description = "Table 1.1.8. Contributions to Percent Change in the Gross Domestic Product Price Index (A) (Q)",
                               Val = "12",
                           },
                           new TableID
                           {
                               Description = "Table 1.1.9. Implicit Price Deflators for Gross Domestic Product (A) (Q)",
                               Val = "13",
                           },
                           new TableID
                           {
                               Description = "Table 1.1.10. Percentage Shares of Gross Domestic Product (A) (Q)",
                               Val = "14",
                           },
                           new TableID
                           {
                               Description = "Table 1.1.11. Real Gross Domestic Product: Percent Change From Quarter One Year Ago (Q)",
                               Val = "310",
                           },
                           new TableID
                           {
                               Description = "Table 1.2.1. Percent Change From Preceding Period in Real Gross Domestic Product by Major Type of Product (A) (Q)",
                               Val = "15",
                           },
                           new TableID
                           {
                               Description = "Table 1.2.2. Contributions to Percent Change in Real Gross Domestic Product by Major Type of Product (A) (Q)",
                               Val = "16",
                           },
                           new TableID
                           {
                               Description = "Table 1.2.3. Real Gross Domestic Product by Major Type of Product, Quantity Indexes (A) (Q)",
                               Val = "17",
                           },
                           new TableID
                           {
                               Description = "Table 1.2.4. Price Indexes for Gross Domestic Product by Major Type of Product (A) (Q)",
                               Val = "18",
                           },
                           new TableID
                           {
                               Description = "Table 1.2.5. Gross Domestic Product by Major Type of Product (A) (Q)",
                               Val = "19",
                           },
                           new TableID
                           {
                               Description = "Table 1.2.6. Real Gross Domestic Product by Major Type of Product, Chained Dollars (A) (Q)",
                               Val = "20",
                           },
                           new TableID
                           {
                               Description = "Table 1.3.1. Percent Change From Preceding Period in Real Gross Value Added by Sector (A) (Q)",
                               Val = "21",
                           },
                           new TableID
                           {
                               Description = "Table 1.3.3. Real Gross Value Added by Sector, Quantity Indexes (A) (Q)",
                               Val = "22",
                           },
                           new TableID
                           {
                               Description = "Table 1.3.4. Price Indexes for Gross Value Added by Sector (A) (Q)",
                               Val = "23",
                           },
                           new TableID
                           {
                               Description = "Table 1.3.5. Gross Value Added by Sector (A) (Q)",
                               Val = "24",
                           },
                           new TableID
                           {
                               Description = "Table 1.3.6. Real Gross Value Added by Sector, Chained Dollars (A) (Q)",
                               Val = "25",
                           },
                           new TableID
                           {
                               Description = "Table 1.4.1. Percent Change From Preceding Period in Real Gross Domestic Product, Real Gross Domestic Purchases, and Real Final Sales to Domestic Purchasers (A) (Q)",
                               Val = "26",
                           },
                           new TableID
                           {
                               Description = "Table 1.4.3. Real Gross Domestic Product, Real Gross Domestic Purchases, and Real Final Sales to Domestic Purchasers, Quantity Indexes (A) (Q)",
                               Val = "27",
                           },
                           new TableID
                           {
                               Description = "Table 1.4.4. Price Indexes for Gross Domestic Product, Gross Domestic Purchases, and Final Sales to Domestic Purchasers (A) (Q)",
                               Val = "28",
                           },
                           new TableID
                           {
                               Description = "Table 1.4.5. Relation of Gross Domestic Product, Gross Domestic Purchases, and Final Sales to Domestic Purchasers (A) (Q)",
                               Val = "29",
                           },
                           new TableID
                           {
                               Description = "Table 1.4.6. Relation of Real Gross Domestic Product, Real Gross Domestic Purchases, and Real Final Sales to Domestic Purchasers, Chained Dollars (A) (Q)",
                               Val = "30",
                           },
                           new TableID
                           {
                               Description = "Table 1.5.1. Percent Change From Preceding Period in Real Gross Domestic Product, Expanded Detail (A) (Q)",
                               Val = "31",
                           },
                           new TableID
                           {
                               Description = "Table 1.5.2. Contributions to Percent Change in Real Gross Domestic Product, Expanded Detail (A) (Q)",
                               Val = "32",
                           },
                           new TableID
                           {
                               Description = "Table 1.5.3. Real Gross Domestic Product, Expanded Detail, Quantity Indexes (A) (Q)",
                               Val = "33",
                           },
                           new TableID
                           {
                               Description = "Table 1.5.4. Price Indexes for Gross Domestic Product, Expanded Detail (A) (Q)",
                               Val = "34",
                           },
                           new TableID
                           {
                               Description = "Table 1.5.5. Gross Domestic Product, Expanded Detail (A) (Q)",
                               Val = "35",
                           },
                           new TableID
                           {
                               Description = "Table 1.5.6. Real Gross Domestic Product, Expanded Detail, Chained Dollars (A) (Q)",
                               Val = "36",
                           },
                           new TableID
                           {
                               Description = "Table 1.6.4. Price Indexes for Gross Domestic Purchases (A) (Q)",
                               Val = "37",
                           },
                           new TableID
                           {
                               Description = "Table 1.6.7. Percent Change From Preceding Period in Prices for Gross Domestic Purchases (A) (Q)",
                               Val = "38",
                           },
                           new TableID
                           {
                               Description = "Table 1.6.8. Contributions to Percent Change in the Gross Domestic Purchases Price Index (A) (Q)",
                               Val = "39",
                           },
                           new TableID
                           {
                               Description = "Table 1.7.1. Percent Change from Preceding Period in Real Gross Domestic Product, Real Gross National Product, and Real Net National Product (A) (Q)",
                               Val = "40",
                           },
                           new TableID
                           {
                               Description = "Table 1.7.3. Real Gross Domestic Product, Real Gross National Product, and Real Net National Product, Quantity Indexes (A) (Q)",
                               Val = "41",
                           },
                           new TableID
                           {
                               Description = "Table 1.7.4. Price Indexes for Gross Domestic Product, Gross National Product, and Net National Product (A) (Q)",
                               Val = "42",
                           },
                           new TableID
                           {
                               Description = "Table 1.7.5. Relation of Gross Domestic Product, Gross National Product, Net National Product, National Income, and Personal Income (A) (Q)",
                               Val = "43",
                           },
                           new TableID
                           {
                               Description = "Table 1.7.6. Relation of Real Gross Domestic Product, Real Gross National Product, and Real Net National Product, Chained Dollars (A) (Q)",
                               Val = "44",
                           },
                           new TableID
                           {
                               Description = "Table 1.8.3. Command-Basis Real Gross Domestic Product and Gross National Product, Quantity Indexes (A) (Q)",
                               Val = "45",
                           },
                           new TableID
                           {
                               Description = "Table 1.8.6. Command-Basis Real Gross Domestic Product and Gross National Product, Chained Dollars (A) (Q)",
                               Val = "46",
                           },
                           new TableID
                           {
                               Description = "Table 1.9.3. Real Net Value Added by Sector, Quantity Indexes (A)",
                               Val = "47",
                           },
                           new TableID
                           {
                               Description = "Table 1.9.4. Price Indexes for Net Value Added by Sector (A)",
                               Val = "48",
                           },
                           new TableID
                           {
                               Description = "Table 1.9.5. Net Value Added by Sector (A)",
                               Val = "49",
                           },
                           new TableID
                           {
                               Description = "Table 1.9.6. Real Net Value Added by Sector, Chained Dollars (A)",
                               Val = "50",
                           },
                           new TableID
                           {
                               Description = "Table 1.10. Gross Domestic Income by Type of Income (A) (Q)",
                               Val = "51",
                           },
                           new TableID
                           {
                               Description = "Table 1.11. Percentage Shares of Gross Domestic Income (A)",
                               Val = "52",
                           },
                           new TableID
                           {
                               Description = "Table 1.12. National Income by Type of Income (A) (Q)",
                               Val = "53",
                           },
                           new TableID
                           {
                               Description = "Table 1.13. National Income by Sector, Legal Form of Organization, and Type of Income (A)",
                               Val = "54",
                           },
                           new TableID
                           {
                               Description = "Table 1.14. Gross Value Added of Domestic Corporate Business in Current Dollars and Gross Value Added of Nonfinancial Domestic Corporate Business in Current and Chained Dollars (A) (Q)",
                               Val = "55",
                           },
                           new TableID
                           {
                               Description = "Table 1.15. Price, Costs, and Profit Per Unit of Real Gross Value Added of Nonfinancial Domestic Corporate Business (A) (Q)",
                               Val = "56",
                           },
                           new TableID
                           {
                               Description = "Table 1.16. Sources and Uses of Private Enterprise Income (A)",
                               Val = "57",
                           },
                           new TableID
                           {
                               Description = "Table 1.17.1. Percent Change From Preceding Period in Real Gross Domestic Product, Real Gross Domestic Income, and Other Major NIPA Aggregates (A) (Q)",
                               Val = "316",
                           },
                           new TableID
                           {
                               Description = "Table 1.17.5. Gross Domestic Product, Gross Domestic Income, and Other Major NIPA Aggregates (A) (Q)",
                               Val = "317",
                           },
                           new TableID
                           {
                               Description = "Table 1.17.6. Real Gross Domestic Product, Real Gross Domestic Income, and Other Major NIPA Aggregates, Chained Dollars (A) (Q)",
                               Val = "318",
                           },
                           new TableID
                           {
                               Description = "Table 2.1. Personal Income and Its Disposition (A) (Q)",
                               Val = "58",
                           },
                           new TableID
                           {
                               Description = "Table 2.2A. Wages and Salaries by Industry (A) (Q)",
                               Val = "59",
                           },
                           new TableID
                           {
                               Description = "Table 2.2B. Wages and Salaries by Industry (A) (Q)",
                               Val = "60",
                           },
                           new TableID
                           {
                               Description = "Table 2.3.1. Percent Change From Preceding Period in Real Personal Consumption Expenditures by Major Type of Product (A) (Q)",
                               Val = "61",
                           },
                           new TableID
                           {
                               Description = "Table 2.3.2. Contributions to Percent Change in Real Personal Consumption Expenditures by Major Type of Product (A) (Q)",
                               Val = "62",
                           },
                           new TableID
                           {
                               Description = "Table 2.3.3. Real Personal Consumption Expenditures by Major Type of Product, Quantity Indexes (A) (Q)",
                               Val = "63",
                           },
                           new TableID
                           {
                               Description = "Table 2.3.4. Price Indexes for Personal Consumption Expenditures by Major Type of Product (A) (Q)",
                               Val = "64",
                           },
                           new TableID
                           {
                               Description = "Table 2.3.5. Personal Consumption Expenditures by Major Type of Product (A) (Q)",
                               Val = "65",
                           },
                           new TableID
                           {
                               Description = "Table 2.3.6. Real Personal Consumption Expenditures by Major Type of Product, Chained Dollars (A) (Q)",
                               Val = "66",
                           },
                           new TableID
                           {
                               Description = "Table 2.3.7. Percent Change from Preceding Period in Prices for Personal Consumption Expenditures by Major Type of Product (A) (Q)",
                               Val = "67",
                           },
                           new TableID
                           {
                               Description = "Table 2.4.3. Real Personal Consumption Expenditures by Type of Product, Quantity Indexes (A)",
                               Val = "68",
                           },
                           new TableID
                           {
                               Description = "Table 2.4.4. Price Indexes for Personal Consumption Expenditures by Type of Product (A)",
                               Val = "69",
                           },
                           new TableID
                           {
                               Description = "Table 2.4.5. Personal Consumption Expenditures by Type of Product (A)",
                               Val = "70",
                           },
                           new TableID
                           {
                               Description = "Table 2.4.6. Real Personal Consumption Expenditures by Type of Product, Chained Dollars (A)",
                               Val = "71",
                           },
                           new TableID
                           {
                               Description = "Table 2.5.3. Real Personal Consumption Expenditures by Function, Quantity Indexes (A)",
                               Val = "72",
                           },
                           new TableID
                           {
                               Description = "Table 2.5.4. Price Indexes for Personal Consumption Expenditures by Function (A)",
                               Val = "73",
                           },
                           new TableID
                           {
                               Description = "Table 2.5.5. Personal Consumption Expenditures by Function (A)",
                               Val = "74",
                           },
                           new TableID
                           {
                               Description = "Table 2.5.6. Real Personal Consumption Expenditures by Function, Chained Dollars (A)",
                               Val = "75",
                           },
                           new TableID
                           {
                               Description = "Table 2.6. Personal Income and Its Disposition, Monthly (M)",
                               Val = "76",
                           },
                           new TableID
                           {
                               Description = "Table 2.7A. Wages and Salaries by Industry, Monthly (M)",
                               Val = "77",
                           },
                           new TableID
                           {
                               Description = "Table 2.7B. Wages and Salaries by Industry, Monthly (M)",
                               Val = "78",
                           },
                           new TableID
                           {
                               Description = "Table 2.8.1. Percent Change From Preceding Period in Real Personal Consumption Expenditures by Major Type of Product, Monthly (M)",
                               Val = "79",
                           },
                           new TableID
                           {
                               Description = "Table 2.8.3. Real Personal Consumption Expenditures by Major Type of Product, Monthly, Quantity Indexes (M)",
                               Val = "80",
                           },
                           new TableID
                           {
                               Description = "Table 2.8.4. Price Indexes for Personal Consumption Expenditures by Major Type of Product, Monthly (M)",
                               Val = "81",
                           },
                           new TableID
                           {
                               Description = "Table 2.8.5. Personal Consumption Expenditures by Major Type of Product, Monthly (M)",
                               Val = "82",
                           },
                           new TableID
                           {
                               Description = "Table 2.8.6. Real Personal Consumption Expenditures by Major Type of Product, Monthly, Chained Dollars (M)",
                               Val = "83",
                           },
                           new TableID
                           {
                               Description = "Table 2.8.7. Percent Change from Preceding Period in Prices for Personal Consumption Expenditures by Major Type of Product, Monthly (M)",
                               Val = "84",
                           },
                           new TableID
                           {
                               Description = "Table 2.9. Personal Income and Its Disposition by Households and by Nonprofit Institutions Serving Households (A)",
                               Val = "85",
                           },
                           new TableID
                           {
                               Description = "Table 3.1. Government Current Receipts and Expenditures (A) (Q)",
                               Val = "86",
                           },
                           new TableID
                           {
                               Description = "Table 3.2. Federal Government Current Receipts and Expenditures (A) (Q)",
                               Val = "87",
                           },
                           new TableID
                           {
                               Description = "Table 3.3. State and Local Government Current Receipts and Expenditures (A) (Q)",
                               Val = "88",
                           },
                           new TableID
                           {
                               Description = "Table 3.4. Personal Current Tax Receipts (A)",
                               Val = "89",
                           },
                           new TableID
                           {
                               Description = "Table 3.5. Taxes on Production and Imports (A)",
                               Val = "90",
                           },
                           new TableID
                           {
                               Description = "Table 3.6. Contributions for Government Social Insurance (A)",
                               Val = "91",
                           },
                           new TableID
                           {
                               Description = "Table 3.7. Government Current Transfer Receipts (A)",
                               Val = "92",
                           },
                           new TableID
                           {
                               Description = "Table 3.8. Current Surplus of Government Enterprises (A)",
                               Val = "93",
                           },
                           new TableID
                           {
                               Description = "Table 3.9.1. Percent Change From Preceding Period in Real Government Consumption Expenditures and Gross Investment (A) (Q)",
                               Val = "94",
                           },
                           new TableID
                           {
                               Description = "Table 3.9.2. Contributions to Percent Change in Real Government Consumption Expenditures and Gross Investment (A) (Q)",
                               Val = "95",
                           },
                           new TableID
                           {
                               Description = "Table 3.9.3. Real Government Consumption Expenditures and Gross Investment, Quantity Indexes (A) (Q)",
                               Val = "96",
                           },
                           new TableID
                           {
                               Description = "Table 3.9.4. Price Indexes for Government Consumption Expenditures and Gross Investment (A) (Q)",
                               Val = "97",
                           },
                           new TableID
                           {
                               Description = "Table 3.9.5. Government Consumption Expenditures and Gross Investment (A) (Q)",
                               Val = "98",
                           },
                           new TableID
                           {
                               Description = "Table 3.9.6. Real Government Consumption Expenditures and Gross Investment, Chained Dollars (A) (Q)",
                               Val = "99",
                           },
                           new TableID
                           {
                               Description = "Table 3.10.1. Percent Change From Preceding Period in Real Government Consumption Expenditures and General Government Gross Output (A) (Q)",
                               Val = "100",
                           },
                           new TableID
                           {
                               Description = "Table 3.10.3. Real Government Consumption Expenditures and General Government Gross Output, Quantity Indexes (A) (Q)",
                               Val = "101",
                           },
                           new TableID
                           {
                               Description = "Table 3.10.4. Price Indexes for Government Consumption Expenditures and General Government Gross Output (A) (Q)",
                               Val = "102",
                           },
                           new TableID
                           {
                               Description = "Table 3.10.5. Government Consumption Expenditures and General Government Gross Output (A) (Q)",
                               Val = "103",
                           },
                           new TableID
                           {
                               Description = "Table 3.10.6. Real Government Consumption Expenditures and General Government Gross Output, Chained Dollars (A) (Q)",
                               Val = "104",
                           },
                           new TableID
                           {
                               Description = "Table 3.11.1. Percent Change From Preceding Period in Real National Defense Consumption Expenditures and Gross Investment by Type (A) (Q)",
                               Val = "105",
                           },
                           new TableID
                           {
                               Description = "Table 3.11.2. Contributions to Percent Change From Preceding Period in Real National Defense Consumption Expenditures and Gross Investment by Type (A) (Q)",
                               Val = "326",
                           },
                           new TableID
                           {
                               Description = "Table 3.11.3. Real National Defense Consumption Expenditures and Gross Investment by Type, Quantity Indexes (A) (Q)",
                               Val = "106",
                           },
                           new TableID
                           {
                               Description = "Table 3.11.4. Price Indexes for National Defense Consumption Expenditures and Gross Investment by Type (A) (Q)",
                               Val = "107",
                           },
                           new TableID
                           {
                               Description = "Table 3.11.5. National Defense Consumption Expenditures and Gross Investment by Type (A) (Q)",
                               Val = "108",
                           },
                           new TableID
                           {
                               Description = "Table 3.11.6. Real National Defense Consumption Expenditures and Gross Investment by Type, Chained Dollars (A) (Q)",
                               Val = "109",
                           },
                           new TableID
                           {
                               Description = "Table 3.12. Government Social Benefits (A)",
                               Val = "110",
                           },
                           new TableID
                           {
                               Description = "Table 3.13. Subsidies (A)",
                               Val = "111",
                           },
                           new TableID
                           {
                               Description = "Table 3.14. Government Social Insurance Funds Current Receipts and Expenditures (A)",
                               Val = "112",
                           },
                           new TableID
                           {
                               Description = "Table 3.15.1. Percent Change From Preceding Period in Real Government Consumption Expenditures and Gross Investment by Function (A)",
                               Val = "113",
                           },
                           new TableID
                           {
                               Description = "Table 3.15.2. Contributions to Percent Change in Real Government Consumption Expenditures and Gross Investment by Function (A)",
                               Val = "114",
                           },
                           new TableID
                           {
                               Description = "Table 3.15.3. Real Government Consumption Expenditures and Gross Investment by Function, Quantity Indexes (A)",
                               Val = "115",
                           },
                           new TableID
                           {
                               Description = "Table 3.15.4. Price Indexes for Government Consumption Expenditures and Gross Investment by Function (A)",
                               Val = "116",
                           },
                           new TableID
                           {
                               Description = "Table 3.15.5. Government Consumption Expenditures and Gross Investment by Function (A)",
                               Val = "117",
                           },
                           new TableID
                           {
                               Description = "Table 3.15.6. Real Government Consumption Expenditures and Gross Investment by Function, Chained Dollars (A)",
                               Val = "118",
                           },
                           new TableID
                           {
                               Description = "Table 3.16. Government Current Expenditures by Function (A)",
                               Val = "119",
                           },
                           new TableID
                           {
                               Description = "Table 3.17. Selected Government Current and Capital Expenditures by Function (A)",
                               Val = "120",
                           },
                           new TableID
                           {
                               Description = "Table 3.18A. Relation of Federal Government Current Receipts and Expenditures in the National Income and Product Accounts to the Consolidated Cash Statement, Fiscal Years and Quarters (A) (Q)",
                               Val = "121",
                           },
                           new TableID
                           {
                               Description = "Table 3.18B. Relation of Federal Government Current Receipts and Expenditures in the National Income and Product Accounts to the Budget, Fiscal Years and Quarters (A) (Q)",
                               Val = "122",
                           },
                           new TableID
                           {
                               Description = "Table 3.20. State Government Current Receipts and Expenditures (A)",
                               Val = "124",
                           },
                           new TableID
                           {
                               Description = "Table 3.21. Local Government Current Receipts and Expenditures (A)",
                               Val = "125",
                           },
                           new TableID
                           {
                               Description = "Table 3.22. Federal Government Current Receipts and Expenditures, Not Seasonally Adjusted (Q)",
                               Val = "126",
                           },
                           new TableID
                           {
                               Description = "Table 3.23. State and Local Government Current Receipts and Expenditures, Not Seasonally Adjusted (Q)",
                               Val = "127",
                           },
                           new TableID
                           {
                               Description = "Table 4.1. Foreign Transactions in the National Income and Product Accounts (A) (Q)",
                               Val = "128",
                           },
                           new TableID
                           {
                               Description = "Table 4.2.1. Percent Change From Preceding Period in Real Exports and in Real Imports of Goods and Services by Type of Product (A) (Q)",
                               Val = "129",
                           },
                           new TableID
                           {
                               Description = "Table 4.2.2. Contributions to Percent Change in Real Exports and in Real Imports of Goods and Services by Type of Product (A) (Q)",
                               Val = "130",
                           },
                           new TableID
                           {
                               Description = "Table 4.2.3. Real Exports and Imports of Goods and Services by Type of Product, Quantity Indexes (A) (Q)",
                               Val = "131",
                           },
                           new TableID
                           {
                               Description = "Table 4.2.4. Price Indexes for Exports and Imports of Goods and Services by Type of Product (A) (Q)",
                               Val = "132",
                           },
                           new TableID
                           {
                               Description = "Table 4.2.5. Exports and Imports of Goods and Services by Type of Product (A) (Q)",
                               Val = "133",
                           },
                           new TableID
                           {
                               Description = "Table 4.2.6. Real Exports and Imports of Goods and Services by Type of Product, Chained Dollars (A) (Q)",
                               Val = "134",
                           },
                           new TableID
                           {
                               Description = "Table 4.3A. Relation of Foreign Transactions in the National Income and Product Accounts to the Corresponding Items in the International Transactions Accounts (A)",
                               Val = "135",
                           },
                           new TableID
                           {
                               Description = "Table 4.3B. Relation of Foreign Transactions in the National Income and Product Accounts to the Corresponding Items in the International Transactions Accounts (A)",
                               Val = "136",
                           },
                           new TableID
                           {
                               Description = "Table 5.1. Saving and Investment by Sector (A) (Q)",
                               Val = "137",
                           },
                           new TableID
                           {
                               Description = "Table 5.2.3. Real Gross and Net Domestic Investment by Major Type, Quantity Indexes (A)",
                               Val = "138",
                           },
                           new TableID
                           {
                               Description = "Table 5.2.5. Gross and Net Domestic Investment by Major Type (A)",
                               Val = "139",
                           },
                           new TableID
                           {
                               Description = "Table 5.2.6. Real Gross and Net Domestic Investment by Major Type, Chained Dollars (A)",
                               Val = "140",
                           },
                           new TableID
                           {
                               Description = "Table 5.3.1. Percent Change From Preceding Period in Real Private Fixed Investment by Type (A) (Q)",
                               Val = "141",
                           },
                           new TableID
                           {
                               Description = "Table 5.3.2. Contributions to Percent Change in Real Private Fixed Investment by Type (A) (Q)",
                               Val = "142",
                           },
                           new TableID
                           {
                               Description = "Table 5.3.3. Real Private Fixed Investment by Type, Quantity Indexes (A) (Q)",
                               Val = "143",
                           },
                           new TableID
                           {
                               Description = "Table 5.3.4. Price Indexes for Private Fixed Investment by Type (A) (Q)",
                               Val = "144",
                           },
                           new TableID
                           {
                               Description = "Table 5.3.5. Private Fixed Investment by Type (A) (Q)",
                               Val = "145",
                           },
                           new TableID
                           {
                               Description = "Table 5.3.6. Real Private Fixed Investment by Type, Chained Dollars (A) (Q)",
                               Val = "146",
                           },
                           new TableID
                           {
                               Description = "Table 5.4.1. Percent Change From Preceding Period in Real Private Fixed Investment in Structures by Type (A)",
                               Val = "147",
                           },
                           new TableID
                           {
                               Description = "Table 5.4.2. Contributions to Percent Change in Real Private Fixed Investment in Structures by Type (A)",
                               Val = "148",
                           },
                           new TableID
                           {
                               Description = "Table 5.4.3. Real Private Fixed Investment in Structures by Type, Quantity Indexes (A)",
                               Val = "149",
                           },
                           new TableID
                           {
                               Description = "Table 5.4.4. Price Indexes for Private Fixed Investment in Structures by Type (A)",
                               Val = "150",
                           },
                           new TableID
                           {
                               Description = "Table 5.4.5. Private Fixed Investment in Structures by Type (A)",
                               Val = "151",
                           },
                           new TableID
                           {
                               Description = "Table 5.4.6. Real Private Fixed Investment in Structures by Type, Chained Dollars (A)",
                               Val = "152",
                           },
                           new TableID
                           {
                               Description = "Table 5.5.1. Percent Change From Preceding Period in Real Private Fixed Investment in Equipment by Type (A)",
                               Val = "153",
                           },
                           new TableID
                           {
                               Description = "Table 5.5.2. Contributions to Percent Change in Real Private Fixed Investment in Equipment by Type (A)",
                               Val = "154",
                           },
                           new TableID
                           {
                               Description = "Table 5.5.3. Real Private Fixed Investment in Equipment by Type, Quantity Indexes (A)",
                               Val = "155",
                           },
                           new TableID
                           {
                               Description = "Table 5.5.4. Price Indexes for Private Fixed Investment in Equipment by Type (A)",
                               Val = "156",
                           },
                           new TableID
                           {
                               Description = "Table 5.5.5. Private Fixed Investment in Equipment by Type (A)",
                               Val = "157",
                           },
                           new TableID
                           {
                               Description = "Table 5.5.6. Real Private Fixed Investment in Equipment by Type, Chained Dollars (A)",
                               Val = "158",
                           },
                           new TableID
                           {
                               Description = "Table 5.6.1. Percent Change from Preceding Period in Real Private Fixed Investment in Intellectual Property Products by Type (A)",
                               Val = "327",
                           },
                           new TableID
                           {
                               Description = "Table 5.6.2. Contributions to Percent Change in Private Fixed Investment in Intellectual Property Products by Type (A)",
                               Val = "328",
                           },
                           new TableID
                           {
                               Description = "Table 5.6.3. Real Private Fixed Investment in Intellectual Property Products by Type, Quantity Indexes (A)",
                               Val = "329",
                           },
                           new TableID
                           {
                               Description = "Table 5.6.4. Price Indexes for Private Fixed Investment in Intellectual Property Products by Type (A)",
                               Val = "330",
                           },
                           new TableID
                           {
                               Description = "Table 5.6.5. Private Fixed Investment in Intellectual Property Products by Type (A)",
                               Val = "331",
                           },
                           new TableID
                           {
                               Description = "Table 5.6.6. Real Private Fixed Investment in Intellectual Property Products by Type, Chained Dollars (A)",
                               Val = "332",
                           },
                           new TableID
                           {
                               Description = "Table 5.7.5A. Change in Private Inventories by Industry (A) (Q)",
                               Val = "163",
                           },
                           new TableID
                           {
                               Description = "Table 5.7.5B. Change in Private Inventories by Industry (A) (Q)",
                               Val = "164",
                           },
                           new TableID
                           {
                               Description = "Table 5.7.6A. Change in Real Private Inventories by Industry, Chained Dollars (A) (Q)",
                               Val = "165",
                           },
                           new TableID
                           {
                               Description = "Table 5.7.6B. Change in Real Private Inventories by Industry, Chained Dollars (A) (Q)",
                               Val = "166",
                           },
                           new TableID
                           {
                               Description = "Table 5.8.5A. Private Inventories and Domestic Final Sales of Business by Industry (Q)",
                               Val = "173",
                           },
                           new TableID
                           {
                               Description = "Table 5.8.5B. Private Inventories and Domestic Final Sales by Industry (Q)",
                               Val = "174",
                           },
                           new TableID
                           {
                               Description = "Table 5.8.6A. Real Private Inventories and Real Domestic Final Sales of Business by Industry, Chained Dollars (Q)",
                               Val = "175",
                           },
                           new TableID
                           {
                               Description = "Table 5.8.6B. Real Private Inventories and Real Domestic Final Sales by Industry, Chained Dollars (Q)",
                               Val = "176",
                           },
                           new TableID
                           {
                               Description = "Table 5.8.9A. Implicit Price Deflators for Private Inventories by Industry (Q)",
                               Val = "333",
                           },
                           new TableID
                           {
                               Description = "Table 5.8.9B. Implicit Price Deflators for Private Inventories by Industry (Q)",
                               Val = "334",
                           },
                           new TableID
                           {
                               Description = "Table 5.9.3A. Real Gross Government Fixed Investment by Type, Quantity Indexes (A)",
                               Val = "335",
                           },
                           new TableID
                           {
                               Description = "Table 5.9.3B. Real Gross Government Fixed Investment by Type, Quantity Indexes (A)",
                               Val = "336",
                           },
                           new TableID
                           {
                               Description = "Table 5.9.4A. Price Indexes for Gross Government Fixed Investment by Type (A)",
                               Val = "337",
                           },
                           new TableID
                           {
                               Description = "Table 5.9.4B. Price Indexes for Gross Government Fixed Investment by Type (A)",
                               Val = "338",
                           },
                           new TableID
                           {
                               Description = "Table 5.9.5A. Gross Government Fixed Investment by Type (A)",
                               Val = "339",
                           },
                           new TableID
                           {
                               Description = "Table 5.9.5B. Gross Government Fixed Investment by Type (A)",
                               Val = "340",
                           },
                           new TableID
                           {
                               Description = "Table 5.9.6B. Real Gross Government Fixed Investment by Type, Chained Dollars (A)",
                               Val = "342",
                           },
                           new TableID
                           {
                               Description = "Table 5.10. Changes in Net Stock of Produced Assets (Fixed Assets and Inventories) (A)",
                               Val = "178",
                           },
                           new TableID
                           {
                               Description = "Table 5.11. Capital Transfers Paid and Received, by Sector and by Type (A)",
                               Val = "343",
                           },
                           new TableID
                           {
                               Description = "Table 6.1B. National Income Without Capital Consumption Adjustment by Industry (A) (Q)",
                               Val = "179",
                           },
                           new TableID
                           {
                               Description = "Table 6.1C. National Income Without Capital Consumption Adjustment by Industry (A) (Q)",
                               Val = "180",
                           },
                           new TableID
                           {
                               Description = "Table 6.1D. National Income Without Capital Consumption Adjustment by Industry (A) (Q)",
                               Val = "181",
                           },
                           new TableID
                           {
                               Description = "Table 6.2A. Compensation of Employees by Industry (A)",
                               Val = "182",
                           },
                           new TableID
                           {
                               Description = "Table 6.2B. Compensation of Employees by Industry (A)",
                               Val = "183",
                           },
                           new TableID
                           {
                               Description = "Table 6.2C. Compensation of Employees by Industry (A)",
                               Val = "184",
                           },
                           new TableID
                           {
                               Description = "Table 6.2D. Compensation of Employees by Industry (A)",
                               Val = "185",
                           },
                           new TableID
                           {
                               Description = "Table 6.3A. Wages and Salaries by Industry (A)",
                               Val = "186",
                           },
                           new TableID
                           {
                               Description = "Table 6.3B. Wages and Salaries by Industry (A)",
                               Val = "187",
                           },
                           new TableID
                           {
                               Description = "Table 6.3C. Wages and Salaries by Industry (A)",
                               Val = "188",
                           },
                           new TableID
                           {
                               Description = "Table 6.3D. Wages and Salaries by Industry (A)",
                               Val = "189",
                           },
                           new TableID
                           {
                               Description = "Table 6.4A. Full-Time and Part-Time Employees by Industry (A)",
                               Val = "190",
                           },
                           new TableID
                           {
                               Description = "Table 6.4B. Full-Time and Part-Time Employees by Industry (A)",
                               Val = "191",
                           },
                           new TableID
                           {
                               Description = "Table 6.4C. Full-Time and Part-Time Employees by Industry (A)",
                               Val = "192",
                           },
                           new TableID
                           {
                               Description = "Table 6.4D. Full-Time and Part-Time Employees by Industry (A)",
                               Val = "193",
                           },
                           new TableID
                           {
                               Description = "Table 6.5A. Full-Time Equivalent Employees by Industry (A)",
                               Val = "194",
                           },
                           new TableID
                           {
                               Description = "Table 6.5B. Full-Time Equivalent Employees by Industry (A)",
                               Val = "195",
                           },
                           new TableID
                           {
                               Description = "Table 6.5C. Full-Time Equivalent Employees by Industry (A)",
                               Val = "196",
                           },
                           new TableID
                           {
                               Description = "Table 6.5D. Full-Time Equivalent Employees by Industry (A)",
                               Val = "197",
                           },
                           new TableID
                           {
                               Description = "Table 6.6A. Wages and Salaries Per Full-Time Equivalent Employee by Industry (A)",
                               Val = "198",
                           },
                           new TableID
                           {
                               Description = "Table 6.6B. Wages and Salaries Per Full-Time Equivalent Employee by Industry (A)",
                               Val = "199",
                           },
                           new TableID
                           {
                               Description = "Table 6.6C. Wages and Salaries Per Full-Time Equivalent Employee by Industry (A)",
                               Val = "200",
                           },
                           new TableID
                           {
                               Description = "Table 6.6D. Wages and Salaries Per Full-Time Equivalent Employee by Industry (A)",
                               Val = "201",
                           },
                           new TableID
                           {
                               Description = "Table 6.7A. Self-Employed Persons by Industry (A)",
                               Val = "202",
                           },
                           new TableID
                           {
                               Description = "Table 6.7B. Self-Employed Persons by Industry (A)",
                               Val = "203",
                           },
                           new TableID
                           {
                               Description = "Table 6.7C. Self-Employed Persons by Industry (A)",
                               Val = "204",
                           },
                           new TableID
                           {
                               Description = "Table 6.7D. Self-Employed Persons by Industry (A)",
                               Val = "205",
                           },
                           new TableID
                           {
                               Description = "Table 6.8A. Persons Engaged in Production by Industry (A)",
                               Val = "206",
                           },
                           new TableID
                           {
                               Description = "Table 6.8B. Persons Engaged in Production by Industry (A)",
                               Val = "207",
                           },
                           new TableID
                           {
                               Description = "Table 6.8C. Persons Engaged in Production by Industry (A)",
                               Val = "208",
                           },
                           new TableID
                           {
                               Description = "Table 6.8D. Persons Engaged in Production by Industry (A)",
                               Val = "209",
                           },
                           new TableID
                           {
                               Description = "Table 6.9B. Hours Worked by Full-Time and Part-Time Employees by Industry (A)",
                               Val = "210",
                           },
                           new TableID
                           {
                               Description = "Table 6.9C. Hours Worked by Full-Time and Part-Time Employees by Industry (A)",
                               Val = "211",
                           },
                           new TableID
                           {
                               Description = "Table 6.9D. Hours Worked by Full-Time and Part-Time Employees by Industry (A)",
                               Val = "212",
                           },
                           new TableID
                           {
                               Description = "Table 6.10B. Employer Contributions for Government Social Insurance by Industry (A)",
                               Val = "213",
                           },
                           new TableID
                           {
                               Description = "Table 6.10C. Employer Contributions for Government Social Insurance by Industry (A)",
                               Val = "214",
                           },
                           new TableID
                           {
                               Description = "Table 6.10D. Employer Contributions for Government Social Insurance by Industry (A)",
                               Val = "215",
                           },
                           new TableID
                           {
                               Description = "Table 6.11A. Employer Contributions for Employee Pension and Insurance Funds by Industry and by Type (A)",
                               Val = "216",
                           },
                           new TableID
                           {
                               Description = "Table 6.11B. Employer Contributions for Employee Pension and Insurance Funds by Industry and by Type (A)",
                               Val = "217",
                           },
                           new TableID
                           {
                               Description = "Table 6.11C. Employer Contributions for Employee Pension and Insurance Funds by Industry and by Type (A)",
                               Val = "218",
                           },
                           new TableID
                           {
                               Description = "Table 6.11D. Employer Contributions for Employee Pension and Insurance Funds by Industry and by Type (A)",
                               Val = "219",
                           },
                           new TableID
                           {
                               Description = "Table 6.12A. Nonfarm Proprietors' Income by Industry (A)",
                               Val = "220",
                           },
                           new TableID
                           {
                               Description = "Table 6.12B. Nonfarm Proprietors' Income by Industry (A)",
                               Val = "221",
                           },
                           new TableID
                           {
                               Description = "Table 6.12C. Nonfarm Proprietors' Income by Industry (A)",
                               Val = "222",
                           },
                           new TableID
                           {
                               Description = "Table 6.12D. Nonfarm Proprietors' Income by Industry (A)",
                               Val = "223",
                           },
                           new TableID
                           {
                               Description = "Table 6.13A. Noncorporate Capital Consumption Allowances by Industry (A)",
                               Val = "224",
                           },
                           new TableID
                           {
                               Description = "Table 6.13B. Noncorporate Capital Consumption Allowances by Industry (A)",
                               Val = "225",
                           },
                           new TableID
                           {
                               Description = "Table 6.13C. Noncorporate Capital Consumption Allowances by Industry (A)",
                               Val = "226",
                           },
                           new TableID
                           {
                               Description = "Table 6.13D. Noncorporate Capital Consumption Allowances by Industry (A)",
                               Val = "227",
                           },
                           new TableID
                           {
                               Description = "Table 6.14A. Inventory Valuation Adjustment to Nonfarm Incomes by Legal Form of Organization and by Industry (A)",
                               Val = "228",
                           },
                           new TableID
                           {
                               Description = "Table 6.14B. Inventory Valuation Adjustment to Nonfarm Incomes by Legal Form of Organization and by Industry (A)",
                               Val = "229",
                           },
                           new TableID
                           {
                               Description = "Table 6.14C. Inventory Valuation Adjustment to Nonfarm Incomes by Legal Form of Organization and by Industry (A)",
                               Val = "230",
                           },
                           new TableID
                           {
                               Description = "Table 6.14D. Inventory Valuation Adjustment to Nonfarm Incomes by Legal Form of Organization and by Industry (A)",
                               Val = "231",
                           },
                           new TableID
                           {
                               Description = "Table 6.15A. Net Interest by Industry (A)",
                               Val = "232",
                           },
                           new TableID
                           {
                               Description = "Table 6.15B. Net Interest by Industry (A)",
                               Val = "233",
                           },
                           new TableID
                           {
                               Description = "Table 6.15C. Net Interest by Industry (A)",
                               Val = "234",
                           },
                           new TableID
                           {
                               Description = "Table 6.15D. Net Interest by Industry (A)",
                               Val = "235",
                           },
                           new TableID
                           {
                               Description = "Table 6.16A. Corporate Profits by Industry (A)",
                               Val = "236",
                           },
                           new TableID
                           {
                               Description = "Table 6.16B. Corporate Profits by Industry (A) (Q)",
                               Val = "237",
                           },
                           new TableID
                           {
                               Description = "Table 6.16C. Corporate Profits by Industry (A) (Q)",
                               Val = "238",
                           },
                           new TableID
                           {
                               Description = "Table 6.16D. Corporate Profits by Industry (A) (Q)",
                               Val = "239",
                           },
                           new TableID
                           {
                               Description = "Table 6.17A. Corporate Profits Before Tax by Industry (A)",
                               Val = "240",
                           },
                           new TableID
                           {
                               Description = "Table 6.17B. Corporate Profits Before Tax by Industry (A)",
                               Val = "241",
                           },
                           new TableID
                           {
                               Description = "Table 6.17C. Corporate Profits Before Tax by Industry (A)",
                               Val = "242",
                           },
                           new TableID
                           {
                               Description = "Table 6.17D. Corporate Profits Before Tax by Industry (A)",
                               Val = "243",
                           },
                           new TableID
                           {
                               Description = "Table 6.18A. Taxes on Corporate Income by Industry (A)",
                               Val = "244",
                           },
                           new TableID
                           {
                               Description = "Table 6.18B. Taxes on Corporate Income by Industry (A)",
                               Val = "245",
                           },
                           new TableID
                           {
                               Description = "Table 6.18C. Taxes on Corporate Income by Industry (A)",
                               Val = "246",
                           },
                           new TableID
                           {
                               Description = "Table 6.18D. Taxes on Corporate Income by Industry (A)",
                               Val = "247",
                           },
                           new TableID
                           {
                               Description = "Table 6.19A. Corporate Profits After Tax by Industry (A)",
                               Val = "248",
                           },
                           new TableID
                           {
                               Description = "Table 6.19B. Corporate Profits After Tax by Industry (A)",
                               Val = "249",
                           },
                           new TableID
                           {
                               Description = "Table 6.19C. Corporate Profits After Tax by Industry (A)",
                               Val = "250",
                           },
                           new TableID
                           {
                               Description = "Table 6.19D. Corporate Profits After Tax by Industry (A)",
                               Val = "251",
                           },
                           new TableID
                           {
                               Description = "Table 6.20A. Net Corporate Dividend Payments by Industry (A)",
                               Val = "252",
                           },
                           new TableID
                           {
                               Description = "Table 6.20B. Net Corporate Dividend Payments by Industry (A)",
                               Val = "253",
                           },
                           new TableID
                           {
                               Description = "Table 6.20C. Net Corporate Dividend Payments by Industry (A)",
                               Val = "254",
                           },
                           new TableID
                           {
                               Description = "Table 6.20D. Net Corporate Dividend Payments by Industry (A)",
                               Val = "255",
                           },
                           new TableID
                           {
                               Description = "Table 6.21A. Undistributed Corporate Profits by Industry (A)",
                               Val = "256",
                           },
                           new TableID
                           {
                               Description = "Table 6.21B. Undistributed Corporate Profits by Industry (A)",
                               Val = "257",
                           },
                           new TableID
                           {
                               Description = "Table 6.21C. Undistributed Corporate Profits by Industry (A)",
                               Val = "258",
                           },
                           new TableID
                           {
                               Description = "Table 6.21D. Undistributed Corporate Profits by Industry (A)",
                               Val = "259",
                           },
                           new TableID
                           {
                               Description = "Table 6.22A. Corporate Capital Consumption Allowances by Industry (A)",
                               Val = "260",
                           },
                           new TableID
                           {
                               Description = "Table 6.22B. Corporate Capital Consumption Allowances by Industry (A)",
                               Val = "261",
                           },
                           new TableID
                           {
                               Description = "Table 6.22C. Corporate Capital Consumption Allowances by Industry (A)",
                               Val = "262",
                           },
                           new TableID
                           {
                               Description = "Table 6.22D. Corporate Capital Consumption Allowances by Industry (A)",
                               Val = "263",
                           },
                           new TableID
                           {
                               Description = "Table 7.1. Selected Per Capita Product and Income Series in Current and Chained Dollars (A) (Q)",
                               Val = "264",
                           },
                           new TableID
                           {
                               Description = "Table 7.2.1A. Percent Change From Preceding Period in Real Auto Output (A) (Q)",
                               Val = "265",
                           },
                           new TableID
                           {
                               Description = "Table 7.2.1B. Percent Change from Preceding Period in Real Motor Vehicle Output (A) (Q)",
                               Val = "266",
                           },
                           new TableID
                           {
                               Description = "Table 7.2.3A. Real Auto Output, Quantity Indexes (A) (Q)",
                               Val = "267",
                           },
                           new TableID
                           {
                               Description = "Table 7.2.3B. Real Motor Vehicle Output, Quantity Indexes (A) (Q)",
                               Val = "268",
                           },
                           new TableID
                           {
                               Description = "Table 7.2.4A. Price Indexes for Auto Output (A) (Q)",
                               Val = "269",
                           },
                           new TableID
                           {
                               Description = "Table 7.2.4B. Price Indexes for Motor Vehicle Output (A) (Q)",
                               Val = "270",
                           },
                           new TableID
                           {
                               Description = "Table 7.2.5A. Auto Output (A) (Q)",
                               Val = "271",
                           },
                           new TableID
                           {
                               Description = "Table 7.2.5B. Motor Vehicle Output (A) (Q)",
                               Val = "272",
                           },
                           new TableID
                           {
                               Description = "Table 7.2.6B. Real Motor Vehicle Output, Chained Dollars (A) (Q)",
                               Val = "273",
                           },
                           new TableID
                           {
                               Description = "Table 7.3.3. Real Farm Sector Output, Real Gross Value Added, and Real Net Value Added, Quantity Indexes (A)",
                               Val = "274",
                           },
                           new TableID
                           {
                               Description = "Table 7.3.4. Price Indexes for Farm Sector Output, Gross Value Added, and Net Value Added (A)",
                               Val = "275",
                           },
                           new TableID
                           {
                               Description = "Table 7.3.5. Farm Sector Output, Gross Value Added, and Net Value Added (A)",
                               Val = "276",
                           },
                           new TableID
                           {
                               Description = "Table 7.3.6. Real Farm Sector Output, Real Gross Value Added, and Real Net Value Added, Chained Dollars (A)",
                               Val = "277",
                           },
                           new TableID
                           {
                               Description = "Table 7.4.3. Real Housing Sector Output, Real Gross Value Added, and Real Net Value Added, Quantity Indexes (A)",
                               Val = "278",
                           },
                           new TableID
                           {
                               Description = "Table 7.4.4. Price Indexes for Housing Sector Output, Gross Value Added, and Net Value Added (A)",
                               Val = "279",
                           },
                           new TableID
                           {
                               Description = "Table 7.4.5. Housing Sector Output, Gross Value Added, and Net Value Added (A)",
                               Val = "280",
                           },
                           new TableID
                           {
                               Description = "Table 7.4.6. Real Housing Sector Output, Real Gross Value Added, and Real Net Value Added, Chained Dollars (A)",
                               Val = "281",
                           },
                           new TableID
                           {
                               Description = "Table 7.5. Consumption of Fixed Capital by Legal Form of Organization and Type of Income (A) (Q)",
                               Val = "282",
                           },
                           new TableID
                           {
                               Description = "Table 7.6. Capital Consumption Adjustment by Legal Form of Organization and Type of Adjustment (A)",
                               Val = "283",
                           },
                           new TableID
                           {
                               Description = "Table 7.7. Business Current Transfer Payments by Type (A)",
                               Val = "284",
                           },
                           new TableID
                           {
                               Description = "Table 7.8. Supplements to Wages and Salaries by Type (A)",
                               Val = "285",
                           },
                           new TableID
                           {
                               Description = "Table 7.9. Rental Income of Persons by Legal Form of Organization and by Type of Income (A)",
                               Val = "286",
                           },
                           new TableID
                           {
                               Description = "Table 7.10. Dividends Paid and Received by Sector (A)",
                               Val = "287",
                           },
                           new TableID
                           {
                               Description = "Table 7.11. Interest Paid and Received by Sector and Legal Form of Organization (A)",
                               Val = "288",
                           },
                           new TableID
                           {
                               Description = "Table 7.12. Imputations in the National Income and Product Accounts (A)",
                               Val = "289",
                           },
                           new TableID
                           {
                               Description = "Table 7.13. Relation of Consumption of Fixed Capital in the National Income and Product Accounts to Depreciation and Amortization as Published by the Internal Revenue Service (A)",
                               Val = "290",
                           },
                           new TableID
                           {
                               Description = "Table 7.14. Relation of Nonfarm Proprietors' Income in the National Income and Product Accounts to Corresponding Measures as Published by the Internal Revenue Service (A)",
                               Val = "291",
                           },
                           new TableID
                           {
                               Description = "Table 7.15. Relation of Net Farm Income in the National Income and Product Accounts to Net Farm Income as Published by the U.S. Department of Agriculture (A)",
                               Val = "292",
                           },
                           new TableID
                           {
                               Description = "Table 7.16. Relation of Corporate Profits, Taxes, and Dividends in the National Income and Product Accounts to Corresponding Measures as Published by the Internal Revenue Service (A)",
                               Val = "293",
                           },
                           new TableID
                           {
                               Description = "Table 7.17. Relation of Monetary Interest Paid and Received in the National Income and Product Accounts to Corresponding Measures as Published by the Internal Revenue Service (A)",
                               Val = "294",
                           },
                           new TableID
                           {
                               Description = "Table 7.18. Relation of Wages and Salaries in the National Income and Product Accounts to Wages and Salaries as Published by the Bureau of Labor Statistics (A)",
                               Val = "295",
                           },
                           new TableID
                           {
                               Description = "Table 7.19. Comparison of Income and Outlays of Nonprofit Institutions Serving Households with Revenue and Expenses as Published by the Internal Revenue Service (A)",
                               Val = "297",
                           },
                           new TableID
                           {
                               Description = "Table 7.20. Transactions of Defined Benefit and Defined Contribution Pension Plans (A)",
                               Val = "296",
                           },
                           new TableID
                           {
                               Description = "Table 7.21. Transactions of Defined Benefit Pension Plans (A)",
                               Val = "392",
                           },
                           new TableID
                           {
                               Description = "Table 7.22. Transactions of Private Defined Benefit Pension Plans (A)",
                               Val = "393",
                           },
                           new TableID
                           {
                               Description = "Table 7.23. Transactions of Federal Government Defined Benefit Pension Plans (A)",
                               Val = "394",
                           },
                           new TableID
                           {
                               Description = "Table 7.24. Transactions of State and Local Government Defined Benefit Pension Plans (A)",
                               Val = "395",
                           },
                           new TableID
                           {
                               Description = "Table 7.25. Transactions of Defined Contribution Pension Plans (A)",
                               Val = "397",
                           },

                       };
                return _values;
            }
        }
	}//end TableID
}//end NoFuture.Rand.Gov.Bea.Parameters.Nipa