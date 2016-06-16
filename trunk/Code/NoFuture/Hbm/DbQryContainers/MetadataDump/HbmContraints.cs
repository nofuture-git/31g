using System.IO;
using System.Reflection;
using NoFuture.Globals;

namespace NoFuture.Hbm.DbQryContainers.MetadataDump
{
    public class HbmContraints : HbmBase
    {
        public override string OutputPath
        {
            get
            {
                return Path.Combine(Settings.HbmDirectory,
                    string.Format("{0}.{1}.constraints.json", NfConfig.SqlServer, NfConfig.SqlCatalog));
            }
        }

        public override string SelectStatement
        {
            get
            {
                var liSteam =
                    Assembly.GetExecutingAssembly().GetManifestResourceStream("NoFuture.Hbm.MsSql.Constraints.sql");
                if (liSteam == null)
                {
                    throw new InvalidHbmNameException("The sql embedded resource named Constraints.sql is missing.");
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

