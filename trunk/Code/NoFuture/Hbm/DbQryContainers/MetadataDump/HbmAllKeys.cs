using System.IO;
using System.Reflection;

namespace NoFuture.Hbm.DbQryContainers.MetadataDump
{
    public class HbmAllKeys : HbmBase
    {
        public override string OutputPath
        {
            get
            {
                return Path.Combine(Settings.HbmDirectory,
                    string.Format("{0}.{1}.allkeys.json", Shared.Constants.SqlServer, Shared.Constants.SqlCatalog));
            }
        }

        public override string SelectStatement
        {
            get
            {
                var liSteam = Assembly.GetExecutingAssembly().GetManifestResourceStream("NoFuture.Hbm.MsSql.AllKeys.sql");
                if (liSteam == null)
                {
                    throw new InvalidHbmNameException("The sql embedded resource named AllKeys.sql is missing.");
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
