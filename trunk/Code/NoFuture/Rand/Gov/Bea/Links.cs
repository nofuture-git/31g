using NoFuture.Globals;
namespace NoFuture.Rand.Gov.Bea
{
    /// <summary>
    /// Contains the links used to make RESTful calls to the BEA's API
    /// </summary>
    public class Links
    {
        public const string BeaBaseAccessUri = "http://www.bea.gov/api/data";

        public static string BeaStdUri
        {
            get
            {
                if (string.IsNullOrWhiteSpace(SecurityKeys.BeaDataApiKey))
                    throw new Exceptions.RahRowRagee(
                        "The '[NoFuture.Keys]::BeaDataApiKey' must be set before calling this property.");
                return string.Format("{0}?&UserID={1}&", BeaBaseAccessUri, SecurityKeys.BeaDataApiKey);
            }
        }

        public static string BeaGetDatasetListUri
        {
            get { return string.Format("{0}method=GETDATASETLIST&", BeaStdUri); }
        }

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
