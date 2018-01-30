using System.IO;
using System.Reflection;
using NoFuture.Shared;
using NoFuture.Shared.Cfg;
using NoFuture.Shared.Core;

namespace NoFuture.Hbm.DbQryContainers.MetadataDump
{
    public class HbmAllIndex : HbmBase
    {
        public override string OutputPath
        {
            get
            {
                return Path.Combine(Settings.HbmDirectory,
                    string.Format("{0}.{1}.allIndex.json", NfConfig.SqlServer, NfConfig.SqlCatalog));
            }
        }


        public override string SelectStatement
        {
            get
            {
                var liSteam = Assembly.GetExecutingAssembly().GetManifestResourceStream("NoFuture.Hbm.MsSql.AllIndex.sql");
                if (liSteam == null)
                {
                    throw new InvalidHbmNameException("The sql embedded resource named AllIndex.sql is missing.");
                }
                var txtSr = new StreamReader(liSteam);
                return txtSr.ReadToEnd();
            }
        }

        public override string MsSql2kSelectStatement
        {
            get
            {
                var liSteam = Assembly.GetExecutingAssembly().GetManifestResourceStream("NoFuture.Hbm.MsSql.AllIndex2K.sql");
                if (liSteam == null)
                {
                    throw new InvalidHbmNameException("The sql embedded resource named AllIndex2K.sql is missing.");
                }
                var txtSr = new StreamReader(liSteam);
                return txtSr.ReadToEnd();
            }
        }
                   
    }
}
