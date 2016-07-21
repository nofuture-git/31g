using System.ServiceModel;

namespace NoFuture.Util.Pos.WcfHost
{
    [ServiceContract]
    public interface IPosParser
    {
        [OperationContract]
        string TagString(string plainText);
    }
}
