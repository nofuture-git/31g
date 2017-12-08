namespace NoFuture.Rand.Core
{
    /// <summary>
    /// Generic type for claiming something about <see cref="T"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IMine<T>
    {
        T My { get; }
    }
}
