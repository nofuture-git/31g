using System;
using System.Configuration;
using System.IO;
using System.Xml;

namespace NoFuture.Util.NfConsole
{
    public class SysCfg
    {
        public static string GetAppCfgSetting(string name, string file = null)
        {
            //TODO: .NET Core doesn't have Configuration Manager
            return ConfigurationManager.AppSettings[name];
        }

        public static XmlDocument GetAspNetWebCfg()
        {
            //TODO: .NET Core doesn't have Configuration Manager
            var machineConfigFile = ConfigurationManager.OpenMachineConfiguration();
            if (machineConfigFile.HasFile == false)
                return null;

            var machineConfigPath = Path.GetDirectoryName(machineConfigFile.FilePath);
            if (String.IsNullOrWhiteSpace(machineConfigPath))
                return null;

            var globalWebConfigPath = Path.Combine(machineConfigPath, "web.config");

            var configFile = new XmlDocument();
            var buffer = File.ReadAllText(globalWebConfigPath);
            configFile.LoadXml(buffer);

            return configFile;
        }
    }
}
