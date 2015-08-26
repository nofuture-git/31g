using System;
using System.Collections.Generic;
using System.Linq;

namespace NoFuture.Sql.Mssql.Md
{
    [Serializable]
    public class PsTypeInfo
    {
        public const string DEFAULT_PS_DB_TYPE = "varchar";
        public const string INT = "int";
        public const string DEC = "money";
        public const string DATE = "datetime";
        public const string BOOL = "bit";

        public PsTypeInfo()
        {
            MaxLength = 1;
            PsDbType = DEFAULT_PS_DB_TYPE;
        }

        public PsTypeInfo(string columnName, string[] data)
        {
            if (string.IsNullOrWhiteSpace(columnName) || data == null || data.Length <= 0)
                return;
            var parseScoring = new Dictionary<string, int> { { INT, 0 }, { DEC, 0 }, { DATE, 0 }, { BOOL, 0 } };
            MaxLength = 1;
            PsDbType = DEFAULT_PS_DB_TYPE;
            Name = columnName;
            foreach (var dataValue in data.Where(dataValue => !string.IsNullOrWhiteSpace(dataValue)))
            {
                //max length should be equal to the longest string in the list
                if (dataValue.Length > MaxLength)
                    MaxLength = dataValue.Length;

                //only need two classes of numbers viz. integers and rational
                long myIntOut;
                decimal myDecimalOut;
                DateTime myDateOut;
                bool myBoolOut;

                var intParseTrue = long.TryParse(dataValue, out myIntOut);

                if (intParseTrue)
                    parseScoring[INT] += 1;
                if (DateTime.TryParse(dataValue, out myDateOut))
                    parseScoring[DATE] += 1;
                if (Decimal.TryParse(dataValue, out myDecimalOut))
                    parseScoring[DEC] += 1;
                //this could be bool string or MSSQL bits (viz. 1,0)
                if (bool.TryParse(dataValue, out myBoolOut) || (intParseTrue && (myIntOut == 0 || myIntOut == 1)))
                    parseScoring[BOOL] += 1;
            }
            //using MSSQL bit values means that all rational and intergers also passed
            if (parseScoring[BOOL] == data.Length)
            {
                PsDbType = BOOL;
                return;
            }

            //all rational numbers are also integers - int parse will fail "1.0"; decimal parse will pass both "1.0" and "1"
            if (parseScoring[DEC] == data.Length && parseScoring[INT] != data.Length)
            {
                PsDbType = DEC;
                return;
            }

            if (parseScoring[DATE] == data.Length)
            {
                PsDbType = DATE;
                return;
            }

            if (parseScoring[INT] == data.Length)
            {
                PsDbType = INT;
                return;
            }

            PsDbType = string.Format("{0}({1})", DEFAULT_PS_DB_TYPE, MaxLength);

        }
        public string Name { get; set; }
        public int MaxLength { get; set; }
        public bool IsInt { get; set; }
        public bool IsDate { get; set; }
        public bool IsMoney { get; set; }
        public bool IsBit { get; set; }
        public string PsDbType { get; set; }
        public bool IsBinaryData { get; set; }

    }
}
