using System.IO;
using System.Linq;
using NoFuture.Hbm.SortingContainers;

namespace NoFuture.Hbm.DbQryContainers.MetadataDump
{
    public abstract class HbmBase
    {
        public abstract string SelectStatement { get; }
        public abstract string MsSql2kSelectStatement { get; }
        public abstract string OutputPath { get; }

        public virtual string[] QueryKeysNames
        {
            get
            {
                var cmdata = new ColumnMetadata();
                return cmdata.GetType().GetFields().Select(fld => fld.Name).ToArray();
            }
        }
        private ColumnMetadata[] _data;
        public virtual ColumnMetadata[] Data
        {
            get
            {
                if (_data != null && _data.Length > 0)
                    return _data;

                if (!File.Exists(OutputPath))
                    throw new InvalidHbmNameException(string.Format("The file containing the json data at '{0}' is missing.", OutputPath));
                var text = File.ReadAllText(OutputPath);

                var bytes = System.Text.Encoding.UTF8.GetBytes(text);
                var serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(ColumnMetadata[]));
                var ms = new MemoryStream(bytes);
                _data = (ColumnMetadata[])serializer.ReadObject(ms);
                return _data;
            }
        }
    }
}
