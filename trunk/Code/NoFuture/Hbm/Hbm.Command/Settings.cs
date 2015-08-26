using System.IO;
using NHibernate;
using NHibernate.Cfg;

namespace NoFuture.Hbm.Command
{
    public class Settings
    {
        private readonly static Configuration _hbmCfg = new Configuration();
        public static Configuration HbmCfg
        {
            get { return _hbmCfg; }
        }

        public static void HbmCfgConfigure(string hbmCfgXml)
        {
            if (File.Exists(hbmCfgXml))
                HbmCfg.Configure(hbmCfgXml);
        }

        public static void AddHbmAsm(System.Reflection.Assembly hbmXmlAsm)
        {
            HbmCfg.AddAssembly(hbmXmlAsm);
        }

        private readonly static object _sessFactoryLock = new object();
        private static ISessionFactory _hbmSessionFactory;
        public static ISessionFactory HbmSessionFactory
        {
            get
            {
                lock (_sessFactoryLock)
                {
                    return _hbmSessionFactory ?? (_hbmSessionFactory = HbmCfg.BuildSessionFactory());
                }
            }
        }

        public static ISession OpenHbmSession
        {
            get { return HbmSessionFactory.OpenSession(); }
        }
    }
}
