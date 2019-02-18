namespace NoFuture.Rand.Law.US.Criminal
{
    public interface IProsecution
    {
        ILegalPerson GetDefendant(ILegalPerson[] persons);
    }
}
