namespace NoFuture.Rand.Law.US.Persons
{
    /// <summary>
    /// <![CDATA[
    /// Per UCC 2-104(1) "deals in goods [...] knowledge or skill peculiar to [...] goods"
    /// ]]>
    /// </summary>
    public interface IMerchant<T> : IExpert<T> where T : ILegalConcept
    {
    }
}
