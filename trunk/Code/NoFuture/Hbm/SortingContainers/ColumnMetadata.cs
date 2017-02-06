using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using NoFuture.Util.NfType;

namespace NoFuture.Hbm.SortingContainers
{
    [Serializable]
    public class ColumnMetadata
    {
        public string schema_name;
        public string table_name;
        public string column_name;
        public int? column_ordinal;
        public string data_type;
        public int? string_length;
        public bool? is_nullable;
        public string constraint_name;
        public string unique_constraint_name;
        public string unique_constraint_schema;
        public bool? is_auto_increment;
        public string precision;

        public void CopyFrom(System.Data.DataColumn col)
        {
            if (!string.IsNullOrWhiteSpace(col.ColumnName))
                column_name = col.ColumnName;
            if (col.Ordinal > 0 )
                column_ordinal = col.Ordinal;
            if (col.DataType != null && !string.IsNullOrWhiteSpace(col.DataType.FullName))
            {
                data_type = NfTypeName.GetLastTypeNameFromArrayAndGeneric(col.DataType.FullName);
                if (data_type == "System.String")
                    string_length = Globals.MSSQL_MAX_VARCHAR;
            }

            is_auto_increment = col.AutoIncrement;
            is_nullable = col.AllowDBNull;
            
        }

        public void CopyFrom(ColumnMetadata col)
        {
            if (col == null)
                return;
            if (!string.IsNullOrWhiteSpace(col.schema_name))
                schema_name = col.schema_name;
            if (!string.IsNullOrWhiteSpace(col.table_name))
                table_name = col.table_name;
            if (!string.IsNullOrWhiteSpace(col.column_name))
                column_name = col.column_name;
            if (col.column_ordinal != null)
                column_ordinal = col.column_ordinal;
            if (!string.IsNullOrWhiteSpace(col.data_type))
                data_type = col.data_type;
            if (col.is_nullable != null)
                is_nullable = col.is_nullable;
            if (!string.IsNullOrWhiteSpace(col.constraint_name))
                constraint_name = col.constraint_name;
            if (!string.IsNullOrWhiteSpace(col.unique_constraint_name))
                unique_constraint_name = col.unique_constraint_name;
            if (!string.IsNullOrWhiteSpace(col.unique_constraint_schema))
                unique_constraint_schema = col.unique_constraint_schema;
            if (col.is_auto_increment != null)
                is_auto_increment = col.is_auto_increment;
            if (col.string_length != null)
                string_length = col.string_length;
            if (!string.IsNullOrWhiteSpace(col.precision))
                precision = col.precision;
        }

        public string ToJsonString()
        {
            var dk = new System.Runtime.Serialization.Json.DataContractJsonSerializer(this.GetType());
            var ms = new MemoryStream();
            dk.WriteObject(ms, this);
            var rdr = new StreamReader(ms);
            ms.Position = 0;
            return rdr.ReadToEnd();
        }

        public static bool TryParseJson(string jsonText, out ColumnMetadata columnOut)
        {
            columnOut = null;
            if (string.IsNullOrWhiteSpace(jsonText))
                return false;
            try
            {
                var data = Encoding.UTF8.GetBytes(jsonText);
                var jsonSerializer = new DataContractJsonSerializer(typeof(ColumnMetadata));
                using (var ms = new MemoryStream(data))
                {
                    columnOut = (ColumnMetadata)jsonSerializer.ReadObject(ms);
                    return true;
                }
            }
            catch
            {
                columnOut = null;
                return false;
            }  
        }
    }
}