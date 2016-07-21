using System.ServiceModel;

namespace NoFuture.Util.Pos.Host
{
    [ServiceContract]
    public interface IPosParser
    {
        [OperationContract]
        string TagString(string plainText);
    }
}
