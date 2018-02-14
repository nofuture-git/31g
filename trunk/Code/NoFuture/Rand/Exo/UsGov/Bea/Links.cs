using NoFuture.Shared.Core;

namespace NoFuture.Rand.Exo.UsGov.Bea
{
    /// <summary>
    /// Contains the links used to make RESTful calls to the BEA's API
    /// </summary>
    public class Links
    {
        public const string BeaBaseAccessUri = "https://www.bea.gov/api/data";
        public static string BeaDataApiKey { get; set; }
        public static string BeaStdUri
        {
            get
            {
                if (string.IsNullOrWhiteSpace(BeaDataApiKey))
                    throw new RahRowRagee(
                        "The 'NoFuture.Globals.SecurityKeys.BeaDataApiKey' must be set before calling this property.");
                return $"{BeaBaseAccessUri}?&UserID={BeaDataApiKey}&";
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
            return $"{BeaStdUri}method=getparameterlist&datasetname={dataSetName}&";
        }

        public static string GetBeaParameterValuesUri(string dataSetName, string parameterName)
        {
            return $"{BeaStdUri}method=GetParameterValues&datasetname={dataSetName}&ParameterName={parameterName}&";
        }
    }
}
