namespace NoFuture.Rand.Law.Criminal.US
{
    public interface IProsecution
    {
        ILegalPerson GetDefendant(ILegalPerson[] persons);
    }
}
