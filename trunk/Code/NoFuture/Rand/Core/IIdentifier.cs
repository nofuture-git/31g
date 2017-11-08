namespace NoFuture.Rand.Core
{
    /// <summary>
    /// A preeminent identity type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IIdentifier<T> : ICited
    {
        string Abbrev { get; }
        T Value { get; set; }
    }
}