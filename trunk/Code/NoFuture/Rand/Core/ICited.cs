namespace NoFuture.Rand.Core
{
    /// <summary>
    /// Any type whose source is explicit
    /// </summary>
    public interface ICited
    {
        string Src { get; set; }
    }
}