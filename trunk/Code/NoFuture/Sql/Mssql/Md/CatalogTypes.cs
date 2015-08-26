using System;
using System.Collections.Generic;
using System.Data;

namespace NoFuture.Sql.Mssql.Md
{
    [Serializable]
    public class Common
    {
        public string SchemaName { get; set; }
        public string TableName { get; set; }
        public string ColumnName { get; set; }
        public string DataType { get; set; }
        public int? Length { get; set; }
        public int? Ordinal { get; set; }
        public bool? IsNullable { get; set; }
        public string DefaultValue { get; set; }
        public bool? IsIdentity { get; set; }

        public static List<Common> CopyFromDataRows(DataRow[] dataRows)
        {
            var lst = new List<Common>();
            if (dataRows == null || dataRows.Length <= 0)
                return lst;
            foreach (var dr in dataRows)
            {
                var com = new Common
                {
                    SchemaName = dr[Qry.Catalog.SCHEMA_NAME].ToString(),
                    TableName = dr[Qry.Catalog.TABLE_NAME].ToString(),
                    ColumnName = dr[Qry.Catalog.COLUMN_NAME].ToString(),
                    DataType = dr[Qry.Catalog.DATA_TYPE].ToString()
                };

                int lenOut;
                int ordinalOut;
                bool nullableOut;

                if (int.TryParse(dr[Qry.Catalog.LENGTH].ToString(), out lenOut))
                    com.Length = lenOut;
                if (int.TryParse(dr[Qry.Catalog.ORDINAL].ToString(), out ordinalOut))
                    com.Ordinal = ordinalOut;
                if (bool.TryParse(dr[Qry.Catalog.IS_NULLABLE].ToString(), out nullableOut))
                    com.IsNullable = nullableOut;
                if (!string.IsNullOrWhiteSpace(dr[Qry.Catalog.DEFAULT_VALUE].ToString()))
                    com.DefaultValue = dr[Qry.Catalog.DEFAULT_VALUE].ToString();

                lst.Add(com);
            }
            return lst;
        }
    }

    [Serializable]
    public class ProcParam
    {
        public string ProcName { get; set; }
        public string ParamName { get; set; }
        public string ProcText { get; set; }
    }
}
