using System.IO;
using System.Linq;
using System.Reflection;
using NoFuture.Hbm.SortingContainers;
using NoFuture.Shared;

namespace NoFuture.Hbm.DbQryContainers.MetadataDump
{
    public class HbmStoredProcsAndParams
    {
        public virtual string[] QueryKeysNames
        {
            get
            {
                return typeof(StoredProcParamItem).GetFields(BindingFlags.NonPublic | BindingFlags.Instance).Select(fld => fld.Name).ToArray();
            }
        }

        private StoredProcParamItem[] _data;
        public virtual StoredProcParamItem[] Data
        {
            get
            {
                if (_data != null && _data.Length > 0)
                    return _data;

                if (!File.Exists(OutputPath))
                    throw new InvalidHbmNameException(string.Format("The file containing the json data at '{0}' is missing.", OutputPath));
                var text = File.ReadAllText(OutputPath);

                var bytes = System.Text.Encoding.UTF8.GetBytes(text);
                var serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(StoredProcParamItem[]));
                var ms = new MemoryStream(bytes);
                _data = (StoredProcParamItem[])serializer.ReadObject(ms);
                return _data;
            }
        }
        public string SelectStatement
        {
            get
            {
                var liSteam =
                  Assembly.GetExecutingAssembly().GetManifestResourceStream("NoFuture.Hbm.MsSql.StoredProcsAndParams.sql");
                if (liSteam == null)
                {
                    throw new InvalidHbmNameException("The sql embedded resource named StoredProcsAndParams.sql is missing.");
                }
                var txtSr = new StreamReader(liSteam);
                return txtSr.ReadToEnd();
            }
        }

        public string MsSql2kSelectStatement
        {
            get { return SelectStatement; }
        }


        public string OutputPath
        {
            get
            {
                return Path.Combine(Settings.HbmDirectory,
                  string.Format("{0}.{1}.sp.json", NfConfig.SqlServer, NfConfig.SqlCatalog));
            }
        }
    }
}
