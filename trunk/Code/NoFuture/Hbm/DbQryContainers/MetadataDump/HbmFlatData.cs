using System.IO;
using System.Reflection;
using NoFuture.Shared;
using NoFuture.Shared.Core;
using NoFuture.Shared.Core.Cfg;

namespace NoFuture.Hbm.DbQryContainers.MetadataDump
{
    public class HbmFlatData : HbmBase
    {
        public override string OutputPath
        {
            get
            {
                return Path.Combine(Settings.HbmDirectory,
                    string.Format("{0}.{1}.flatData.json", NfConfig.SqlServer, NfConfig.SqlCatalog));
            }
        }

        public override string SelectStatement
        {
            get
            {
                var liSteam =
                    Assembly.GetExecutingAssembly().GetManifestResourceStream("NoFuture.Hbm.MsSql.FlatData.sql");
                if (liSteam == null)
                {
                    throw new InvalidHbmNameException("The sql embedded resource named FlatData.sql is missing.");
                }
                var txtSr = new StreamReader(liSteam);
                return txtSr.ReadToEnd();
            }
        }

        public override string MsSql2kSelectStatement
        {
            get
            {
                var liSteam =
                    Assembly.GetExecutingAssembly().GetManifestResourceStream("NoFuture.Hbm.MsSql.FlatData2K.sql");
                if (liSteam == null)
                {
                    throw new InvalidHbmNameException("The sql embedded resource named FlatData2K.sql is missing.");
                }
                var txtSr = new StreamReader(liSteam);
                return txtSr.ReadToEnd();
            }
        }

    }
}
