using NoFuture.Rand.Core;

namespace NoFuture.Rand.Gov
{
    public interface IVitalRecord : ICited
    {
        string PersonFullName { get; set; }
    }
}