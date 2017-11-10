using NoFuture.Shared.Core;

namespace NoFuture.Rand.Data.Exo.UsGov.Bea
{
    /// <summary>
    /// Contains the links used to make RESTful calls to the BEA's API
    /// </summary>
    public class Links
    {
        public const string BeaBaseAccessUri = "https://www.bea.gov/api/data";

        public static string BeaStdUri
        {
            get
            {
                if (string.IsNullOrWhiteSpace(NfConfig.SecurityKeys.BeaDataApiKey))
                    throw new RahRowRagee(
                        "The 'NoFuture.Globals.SecurityKeys.BeaDataApiKey' must be set before calling this property.");
                return string.Format("{0}?&UserID={1}&", BeaBaseAccessUri, NfConfig.SecurityKeys.BeaDataApiKey);
            }
        }

        public static string BeaRegionalDataPJEARN_MI(bool byCounty = false)
        {
            var geo = byCounty ? "COUNTY" : "MSA";
            return
                $"{BeaStdUri}&method=GetData&datasetname=RegionalData&KeyCode=PJEARN_CI&Year=ALL&GeoFips={geo}&ResultFormat=json";
        }

        public static string BeaGetDatasetListUri => $"{BeaStdUri}method=GETDATASETLIST&";

        public static string GetBeaParameterListUri(string dataSetName)
        {
            return string.Format("{0}method=getparameterlist&datasetname={1}&", BeaStdUri, dataSetName);
        }

        public static string GetBeaParameterValuesUri(string dataSetName, string parameterName)
        {
            return string.Format("{0}method=GetParameterValues&datasetname={1}&ParameterName={2}&",
                BeaStdUri, dataSetName, parameterName);
        }
    }
}
