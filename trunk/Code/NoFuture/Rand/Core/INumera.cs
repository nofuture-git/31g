namespace NoFuture.Rand.Core
{
    /// <summary>
    /// Any type which has a count-of and an identifier
    /// </summary>
    /// <remarks>Latin for 'be counted'</remarks>
    public interface INumera
    {
        decimal Amount { get; }
        Identifier Id { get; }
    }
}