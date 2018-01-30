using System.IO;
using System.Reflection;
using NoFuture.Shared;
using NoFuture.Shared.Core;
using NoFuture.Shared.Core.Cfg;

namespace NoFuture.Hbm.DbQryContainers.MetadataDump
{
    public class HbmPrimaryKeys : HbmBase
    {
        public override string OutputPath
        {
            get
            {
                return Path.Combine(Settings.HbmDirectory,
                    string.Format("{0}.{1}.pk.json", NfConfig.SqlServer, NfConfig.SqlCatalog));
            }
        }

        public override string SelectStatement
        {
            get
            {
                var liSteam =
                    Assembly.GetExecutingAssembly().GetManifestResourceStream("NoFuture.Hbm.MsSql.PrimaryKeys.sql");
                if (liSteam == null)
                {
                    throw new InvalidHbmNameException("The sql embedded resource named PrimaryKeys.sql is missing.");
                }
                var txtSr = new StreamReader(liSteam);
                return txtSr.ReadToEnd();
            }
        }

        public override string MsSql2kSelectStatement
        {
            get { return SelectStatement; }
        }

    }
}

