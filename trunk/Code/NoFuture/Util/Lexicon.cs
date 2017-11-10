using System.Collections.Generic;

namespace NoFuture.Util
{
    public class Lexicon
    {
        private static Dictionary<string,string> _hbm2NetTypes;
        private static Dictionary<string, string> _mssql2HbmTypes;
        private static Dictionary<string, string> _dotnet2HbmTypes;


        /// <summary>
        /// Lexicon for NHibernate types to C# type (or .NET type when not applicable).
        /// </summary>
        public static Dictionary<string,string> Hbm2NetTypes
        {
            get
            {
                return _hbm2NetTypes ?? (_hbm2NetTypes = new Dictionary<string, string>
                {
                    {"Int64", "System.Int64"},
                    {
                        "NoFuture.Hbm.Sid.SidUserType, NoFuture.Hbm.Sid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null"
                        , "NoFuture.Hbm.Sid.BinaryId"
                    },
                    {
                        "Microsoft.SqlServer.Types.SqlHierarchyId, Microsoft.SqlServer.Types, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91"
                        , "NoFuture.Hbm.Sid.BinaryId"
                    },
                    {
                        "Microsoft.SqlServer.Types.SqlGeometry, Microsoft.SqlServer.Types, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91",
                        "NoFuture.Hbm.Sid.BinaryId"
                    },
                    {
                        "Microsoft.SqlServer.Types.SqlGeography, Microsoft.SqlServer.Types, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91",
                        "NoFuture.Hbm.Sid.BinaryId"
                    },
                    {"Boolean", "System.Boolean"},
                    {"Char", "System.Char"},
                    {"DateTime", "System.DateTime"},
                    {"Decimal", "System.Decimal"},
                    {"Double", "System.Double"},
                    {"Int32", "System.Int32"},
                    {"AnsiString", "System.String"},
                    {"Int16", "System.Int16"},
                    {"Byte", "System.Byte"},
                    {"Guid", "System.Guid"}
                });
            }
        }

        public static Dictionary<string, string> DotNet2HbmTypes
        {
            get
            {
                return _dotnet2HbmTypes ?? (_dotnet2HbmTypes = new Dictionary<string, string>
                {
                    {"System.Char", "AnsiChar"},
                    {"System.Boolean", "Boolean"},
                    {"System.Byte", "Byte"},
                    {"System.DateTime", "DateTime"},
                    {"System.Decimal", "Decimal"},
                    {"System.Double", "Double"},
                    {"System.Guid", "Guid"},
                    {"System.Int16", "Int16"},
                    {"System.Int32", "Int32"},
                    {"System.Int64", "Int64"},
                    {"System.Enum", "PersistentEnum"},
                    {"System.Single", "Single"},
                    {"System.TimeSpan", "TimeSpan"},
                    {"System.String","AnsiString"},
                    {"System.Globalization.CultureInfo","CultureInfo"},
                    {"System.Byte[]","Binary"},
                    {"System.Type","Type"},
                    {"System.Object","Serializable"},
                    {"System.DateTimeOffset", "DateTime"}
                });
            }

        }

        /// <summary>
        /// Lexicon of SqlServer database types to .NET value type 
        /// </summary>
        /// <remarks>
        /// Unlike the other lexicons herein, this one does not include the 'System.' 
        /// on the .NET value types.
        /// binary types return the custom NoFuture.Hbm.SidUserType
        /// </remarks>
        public static Dictionary<string, string> Mssql2HbmTypes
        {
            get
            {
                return _mssql2HbmTypes ?? (_mssql2HbmTypes = new Dictionary<string, string>
                {
                    {"bigint", "Int64"},
                    {"bit", "Boolean"},
                    {"varchar", "AnsiString"},
                    {"nvarchar", "AnsiString"},
                    {"int", "Int32"},
                    {"double", "Double"},
                    {"smallint", "Int16"},
                    {"numeric", "Int32"},
                    {"char", "AnsiString"},
                    {"nchar", "AnsiString"},
                    {"ntext", "AnsiString"},
                    {"sql_variant", "AnsiString"},
                    {"sysname", "AnsiString"},
                    {"text", "AnsiString"},
                    {"date", "DateTime"},
                    {"datetime", "DateTime"},
                    {"datetime2", "DateTime"},
                    {"datetimeoffset", "DateTime"},
                    {"time", "DateTime"},
                    {"smalldatetime", "DateTime"},
                    {"decimal", "Decimal"},
                    {"float", "Decimal"},
                    {"money", "Decimal"},
                    {"smallmoney", "Decimal"},
                    {"real", "Decimal"},
                    {
                        "binary",
                        "NoFuture.Hbm.Sid.SidUserType, NoFuture.Hbm.Sid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null"
                    },
                    {
                        "geography",
                        "Microsoft.SqlServer.Types.SqlGeography, Microsoft.SqlServer.Types, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91"
                    },
                    {
                        "geometry",
                        "Microsoft.SqlServer.Types.SqlGeometry, Microsoft.SqlServer.Types, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91"
                    },
                    {
                        "hierarchyid",
                        "Microsoft.SqlServer.Types.SqlHierarchyId, Microsoft.SqlServer.Types, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91"
                    },
                    {
                        "image",
                        "NoFuture.Hbm.Sid.SidUserType, NoFuture.Hbm.Sid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null"
                    },
                    {
                        "rowversion",
                        "NoFuture.Hbm.Sid.SidUserType, NoFuture.Hbm.Sid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null"
                    },
                    {
                        "timestamp",
                        "NoFuture.Hbm.Sid.SidUserType, NoFuture.Hbm.Sid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null"
                    },
                    {"tinyint", "Byte"},
                    {
                        "varbinary",
                        "NoFuture.Hbm.Sid.SidUserType, NoFuture.Hbm.Sid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null"
                    },
                    {"xml", "AnsiString"},
                    {"uniqueidentifier", "Guid"},
                    {"cursor", "Int32"},
                });
            }
        }
    }
}
