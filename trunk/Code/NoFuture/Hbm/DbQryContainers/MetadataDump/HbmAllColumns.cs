using System.IO;
using System.Reflection;
using NoFuture.Shared;

namespace NoFuture.Hbm.DbQryContainers.MetadataDump
{
    public class HbmAllColumns : HbmBase
    {
        public override string OutputPath
        {
            get
            {
                return Path.Combine(Settings.HbmDirectory,
                    string.Format("{0}.{1}.cols.json", NfConfig.SqlServer, NfConfig.SqlCatalog));
            }
        }

        public override string SelectStatement
        {
            get
            {
                var liSteam =
                    Assembly.GetExecutingAssembly().GetManifestResourceStream("NoFuture.Hbm.MsSql.AllColumns.sql");
                if (liSteam == null)
                {
                    throw new InvalidHbmNameException("The sql embedded resource named AllColumns.sql is missing.");
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
                    Assembly.GetExecutingAssembly().GetManifestResourceStream("NoFuture.Hbm.MsSql.AllColumns2K.sql");
                if (liSteam == null)
                {
                    throw new InvalidHbmNameException("The sql embedded resource named AllColumns2K.sql is missing.");
                }
                var txtSr = new StreamReader(liSteam);
                return txtSr.ReadToEnd();
            }
        }

    }
}

