using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NoFuture.Hbm.SortingContainers;
using NoFuture.Shared;
using NoFuture.Shared.Cfg;
using NoFuture.Shared.Core;

namespace NoFuture.Hbm.DbQryContainers
{
    [Serializable]
    public class SortedOneToMany : SortedBaseFile
    {
        private Dictionary<string, FkItem> _data;
        public override string Name
        {
            get { return "sorted hbm Json results"; }
        }

        public Dictionary<string, FkItem> Data
        {
            get
            {
                if (_data != null && _data.Keys.Count > 0)
                    return _data;

                if (!File.Exists(OutputPath))
                    throw new InvalidHbmNameException(string.Format("The file containing the json data at '{0}' is missing.", OutputPath));
                var text = File.ReadAllText(OutputPath);

                var bytes = System.Text.Encoding.UTF8.GetBytes(text);
                var serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(Dictionary<string, FkItem>));
                var ms = new MemoryStream(bytes);
                _data = (Dictionary<string, FkItem>)serializer.ReadObject(ms);
                return _data;
            }

            set
            {
                _data = value;
                var fkhbmJsonFileName = OutputPath;

                var jsonSerializer =
                    new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(Dictionary<string, FkItem>));

                using (var fs = new FileStream(fkhbmJsonFileName, FileMode.Create))
                {
                    jsonSerializer.WriteObject(fs, value);
                }                
            }
        }

        public override string OutputPath
        {
            get
            {
                return Path.Combine(Settings.HbmDirectory,
                    string.Format("{0}.{1}.fk.hbm.json", NfConfig.SqlServer,
                        NfConfig.SqlCatalog));
            }
        }

        public virtual bool GetManyToOneColumns(string tableName, ref List<ColumnMetadata> constraintNames)
        {
            if (constraintNames == null)
                constraintNames = new List<ColumnMetadata>();

            var timeOfEntryCount = constraintNames.Count;
            var myData = this.Data;
            if (myData == null)
                return false;
            if (!myData.ContainsKey(tableName))
                return false;
            var myTableData = myData[tableName];
            if (myTableData.ManyToOne == null)
                return false;

            var myManyToOne = myTableData.ManyToOne;
            foreach (var mypkData in myManyToOne.Keys.Select(pk => myManyToOne[pk]))
            {
                constraintNames.AddRange(mypkData);
            }

            return constraintNames.Count > timeOfEntryCount;
        }
    }
}
