﻿using System;
using System.Data;
using NoFuture.Tokens.Gia;
using NoFuture.Util;
using NoFuture.Util.Core;
using NoFuture.Util.NfType;
using NfString = NoFuture.Util.Core.NfString;

namespace NoFuture.Hbm.SortingContainers
{
    [Serializable]
    public class StoredProcParamItem
    {
        protected internal string procSchema;
        protected internal string procName;

        protected internal string paramName;
        protected internal string dataType;
        protected internal bool isOutput;
        protected internal int stringLength;
        protected internal bool isNullable;
        protected internal int ordinal;
        protected internal bool isUserDefinedType;
        protected internal string hbmDataType;
        protected internal string sqlUdtTypeName;
        protected internal string dotNetType;
        protected internal bool? isOpenToSqlInj;

        public string ProcSchema { get { return procSchema; } set { procSchema = value; } }
        public string ProcName { get { return procName; } set { procName = value; } }
        
        public string ParamName { get { return paramName; } set { paramName = value; } }
        public string DataType { get { return dataType; } set { dataType = value; } }
        public bool IsOutput { get { return isOutput; } set { isOutput = value; } }
        public int StringLength { get { return stringLength; } set { stringLength = value; } }
        public bool IsNullable { get { return isNullable; } set { isNullable = value; }}
        public int Ordinal { get { return ordinal; } set { ordinal = value; } }
        public bool IsUserDefinedType { get { return isUserDefinedType; } set { isUserDefinedType = value; } }
        public string SqlUdtTypeName { get { return sqlUdtTypeName; } set { sqlUdtTypeName = value; } }
        public bool? IsOpenToSqlInj { get { return isOpenToSqlInj; } set { isOpenToSqlInj = value; } }

        public string HbmDataType
        {
            get
            {
                if (Lexicon.Mssql2HbmTypes.ContainsKey(dataType))
                    hbmDataType = Util.Lexicon.Mssql2HbmTypes[dataType];
                return hbmDataType;
            }
        }

        public string DotNetType
        {
            get
            {
                if (HbmDataType != null && Lexicon.Hbm2NetTypes.ContainsKey(HbmDataType))
                {
                    dotNetType = Lexicon.Hbm2NetTypes[HbmDataType];
                }

                if (IsNullable && !string.IsNullOrWhiteSpace(dotNetType) && !string.Equals("System.String", dotNetType) &&
                    FlattenedItem.ValueTypesList.Contains(dotNetType))
                {
                    dotNetType = string.Format("System.Nullable<{0}>", dotNetType);
                }
                return dotNetType;
            }
        }

        public bool IsDataTable()
        {
            return !string.IsNullOrWhiteSpace(DataType) && NfReflect.IsAssemblyFullName(DataType) &&
                   DataType.StartsWith("System.Data.DataTable");
        }

        public string GetDotNetPropertyName()
        {
            if (string.IsNullOrWhiteSpace(ParamName))
                return null;
            var cName = ParamName.StartsWith("@") ? ParamName.Substring(1, ParamName.Length - 1) : ParamName;
            cName = NfString.SafeDotNetIdentifier(cName);
            cName = NfString.CapWords(cName, null);
            if (GetSqlDataType() == SqlDbType.Bit && !cName.StartsWith("Is"))
                cName = string.Format("Is{0}", cName);
            if (cName.EndsWith("ID"))
                cName = string.Format("{0}Id", cName.Substring(0, cName.Length - 2));

            return cName;
        }

        public SqlDbType GetSqlDataType()
        {
            if (IsUserDefinedType && NfReflect.IsAssemblyFullName(DataType))
            {
                return SqlDbType.Udt;
            }

            if(NfReflect.IsAssemblyFullName(DataType))
                return SqlDbType.NVarChar;

            SqlDbType dbTypeOut;
            Enum.TryParse(dataType, true, out dbTypeOut);
            return dbTypeOut;
        }
    }
}
