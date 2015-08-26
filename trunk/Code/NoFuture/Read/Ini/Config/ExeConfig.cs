namespace NoFuture.Read.Config
{
    public class ExeConfig
    {
        //found syntax by disassembling the WcfTestClient application
        public static System.Configuration.Configuration GetExeConfig(string exeConfigPath)
        {
            var exeConfigFileMap = new System.Configuration.ExeConfigurationFileMap();
            var machineConfig = System.Configuration.ConfigurationManager.OpenMachineConfiguration();
            exeConfigFileMap.MachineConfigFilename = machineConfig.FilePath;
            exeConfigFileMap.ExeConfigFilename = exeConfigPath;
            return System.Configuration.ConfigurationManager.OpenMappedExeConfiguration(exeConfigFileMap,
                System.Configuration.ConfigurationUserLevel.None);
        }

        public static System.ServiceModel.Configuration.ServiceModelSectionGroup GetServiceModelSection(
            string exeConfigPath)
        {
            var svcConfig = GetExeConfig(exeConfigPath);
            return System.ServiceModel.Configuration.ServiceModelSectionGroup.GetSectionGroup(svcConfig);
        }

        public static System.ServiceModel.EndpointAddress GetEndpointAddress(string exeConfigPath, string upIdentity)
        {
            var svcSection = GetServiceModelSection(exeConfigPath);
            var endpointIdentity = System.ServiceModel.EndpointIdentity.CreateUpnIdentity(upIdentity);

            return new System.ServiceModel.EndpointAddress(svcSection.Client.Endpoints[0].Address, endpointIdentity);
        }
    }
}
