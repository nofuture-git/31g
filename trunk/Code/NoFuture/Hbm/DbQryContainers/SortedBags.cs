using System;
using System.Collections.Generic;
using System.IO;
using NoFuture.Globals;
using NoFuture.Hbm.SortingContainers;

namespace NoFuture.Hbm.DbQryContainers
{
    [Serializable]
    public class SortedBags : SortedBaseFile
    {
        private Dictionary<string, List<ColumnMetadata>> _data;

        public override string Name
        {
            get { return "sorted hbm Json results"; }
        }

        public Dictionary<string, List<ColumnMetadata>> Data
        {
            get
            {
                if (_data != null && _data.Keys.Count > 0)
                    return _data;

                if (!File.Exists(OutputPath))
                    throw new InvalidHbmNameException(string.Format("The file containing the json data at '{0}' is missing.", OutputPath));
                var text = File.ReadAllText(OutputPath);

                var bytes = System.Text.Encoding.UTF8.GetBytes(text);
                var serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(Dictionary<string, List<ColumnMetadata>>));
                var ms = new MemoryStream(bytes);
                _data = (Dictionary<string, List<ColumnMetadata>>)serializer.ReadObject(ms);
                return _data;
            }
            set
            {
                _data = value;
                var hbmBagsJsonFileName = OutputPath;

                var jsonSerializer =
                    new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(Dictionary<string, List<ColumnMetadata>>));

                using (var fs = new FileStream(hbmBagsJsonFileName, FileMode.Create))
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
                    string.Format("{0}.{1}.bags.hbm.json", NfConfig.SqlServer,
                        NfConfig.SqlCatalog));
            }
        }
    }
}
