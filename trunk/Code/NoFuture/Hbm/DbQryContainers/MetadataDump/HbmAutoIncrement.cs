using System.IO;
using System.Reflection;
using NoFuture.Shared;
using NoFuture.Shared.Cfg;
using NoFuture.Shared.Core;

namespace NoFuture.Hbm.DbQryContainers.MetadataDump
{
    public class HbmAutoIncrement : HbmBase
    {
        public override string OutputPath
        {
            get
            {
                return Path.Combine(Settings.HbmDirectory,
                    string.Format("{0}.{1}.autoId.json", NfConfig.SqlServer, NfConfig.SqlCatalog));
            }
        }

        public override string SelectStatement
        {
            get
            {
                var liSteam =
                    Assembly.GetExecutingAssembly().GetManifestResourceStream("NoFuture.Hbm.MsSql.AutoIncrement.sql");
                if (liSteam == null)
                {
                    throw new InvalidHbmNameException("The sql embedded resource named AutoIncrement.sql is missing.");
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
                    Assembly.GetExecutingAssembly().GetManifestResourceStream("NoFuture.Hbm.MsSql.AutoIncrement2K.sql");
                if (liSteam == null)
                {
                    throw new InvalidHbmNameException("The sql embedded resource named AutoIncrement2K.sql is missing.");
                }
                var txtSr = new StreamReader(liSteam);
                return txtSr.ReadToEnd();
            }
        }

    }
}

