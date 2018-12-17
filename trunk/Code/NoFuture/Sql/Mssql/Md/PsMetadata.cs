using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace NoFuture.Sql.Mssql.Md
{
    [Serializable]
    public class PsMetadata
    {
        /// <summary>
        /// Ctor instantiates various collections
        /// </summary>
        public PsMetadata()
        {
            TickKeys = new List<string>();
            UqIndexKeys = new List<string>();
            PkKeys = new Dictionary<string, string>();
            IsComputedKeys = new List<string>();
            AutoNumKeys = new List<string>();
            FkKeys = new Dictionary<string, string>();

        }
        public List<string> TickKeys { get; set; }
        public List<string> UqIndexKeys { get; set; }
        public Dictionary<string,string> PkKeys { get; set; }
        public List<string> IsComputedKeys { get; set; }
        public List<string> AutoNumKeys { get; set; }
        public Dictionary<string, string> FkKeys { get; set; }

        /// <summary>
        /// Distinct list of names from all collections herein
        /// </summary>
        public List<string> AllColumnNames
        {
            get
            {
                var names = new List<string>();
                names.AddRange(TickKeys);
                names.AddRange(UqIndexKeys);
                names.AddRange(PkKeys.Keys);
                names.AddRange(IsComputedKeys);
                names.AddRange(AutoNumKeys);
                names.AddRange(FkKeys.Keys);
                names = names.Distinct().ToList();
                return names;
            }
        }

        public List<string> TimestampColumns { get; set; }

        public bool IsIdentityInsert { get; set; }

        /// <summary>
        /// Utility function used to transpose the DataRow array's returned 
        /// by calls to the ADO.NET functionality from PowerShell.
        /// </summary>
        /// <param name="adoData"></param>
        /// <returns></returns>
        public static List<string> ColumnNameToList(DataRow[] adoData)
        {
            var colNameList = new List<string>();
            if(adoData== null || adoData.Length <= 0)
                return colNameList;//empty list, match the ctor
            colNameList.AddRange(
                adoData.Select(dr => dr[Qry.Catalog.COLUMN_NAME].ToString())
                    .Where(colName => !string.IsNullOrWhiteSpace(colName)));
            return colNameList;
        }

        /// <summary>
        /// Utility function to transpose the DataRow array's returned
        /// by calls to ADO.NET from PowerShell.
        /// </summary>
        /// <param name="adoData"></param>
        /// <returns></returns>
        public static Dictionary<string, string> KeysToDictionary(DataRow[] adoData)
        {
            var keyDicti = new Dictionary<string, string>();
            if (adoData == null || adoData.Length <= 0)
                return keyDicti;
            foreach (var dr in adoData)
            {
                var colName = dr[Qry.Catalog.COLUMN_NAME].ToString();
                var dataType = dr[Qry.Catalog.DATA_TYPE].ToString();
                if (string.IsNullOrWhiteSpace(colName) || string.IsNullOrWhiteSpace(dataType))
                    continue;
                if (keyDicti.ContainsKey(colName))
                    continue;
                keyDicti.Add(colName, dataType);
            }
            return keyDicti;
        }

        public int LongestColumnNameLen
        {
            get
            {
                return AllColumnNames.Max(x => (x?.Length ?? 0));
            }
        }
    }
}
