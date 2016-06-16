using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NoFuture.Globals;
using NoFuture.Hbm.SortingContainers;

namespace NoFuture.Hbm.DbQryContainers
{
    [Serializable]
    public class SortedKeys : SortedBaseFile
    {
        private Dictionary<string, PkItem> _data;
        public override string Name
        {
            get { return "sorted hbm Json results"; }
        }

        public override string OutputPath
        {
            get
            {
                return Path.Combine(Settings.HbmDirectory,
                    string.Format("{0}.{1}.pk.hbm.json", NfConfig.SqlServer,
                        NfConfig.SqlCatalog));
            }
        }

        public Dictionary<string, PkItem> Data
        {
            get
            {
                if (_data != null && _data.Keys.Count > 0)
                    return _data;

                if (!File.Exists(OutputPath))
                    throw new InvalidHbmNameException(string.Format("The file containing the json data at '{0}' is missing.", OutputPath));
                var text = File.ReadAllText(OutputPath);

                var bytes = System.Text.Encoding.UTF8.GetBytes(text);
                var serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(Dictionary<string, PkItem>));
                var ms = new MemoryStream(bytes);
                _data = (Dictionary<string, PkItem>)serializer.ReadObject(ms);
                return _data;
            }

            set
            {
                _data = value;
                var pkhbmJsonFileName = OutputPath;

                var jsonSerializer =
                    new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(Dictionary<string, PkItem>));

                using (var fs = new FileStream(pkhbmJsonFileName, FileMode.Create))
                {
                    jsonSerializer.WriteObject(fs, value);
                }                
            }
        }

        public virtual bool GetKeyManyToOneColumns(string tableName, ref List<ColumnMetadata> constraintNames)
        {
            if(constraintNames == null)
                constraintNames = new List<ColumnMetadata>();

            var timeOfEntryCount = constraintNames.Count;
            var myData = this.Data;
            if (myData == null)
                return false;
            if (!myData.ContainsKey(tableName))
                return false;
            var myTableData = myData[tableName];
            if (myTableData.KeyManyToOne == null)
                return false;

            var myKeyManyToOne = myTableData.KeyManyToOne;
            foreach (var mypkData in myKeyManyToOne.Keys.Select(pk => myKeyManyToOne[pk]))
            {
                constraintNames.AddRange(mypkData);
            }

            return constraintNames.Count > timeOfEntryCount;
        }
    }
}
