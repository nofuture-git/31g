namespace NoFuture.Rand.Law.Criminal.US.Defense
{
    public abstract class DefenseBase : LegalConcept
    {
        public ICrime Crime { get; }

        protected DefenseBase(ICrime crime)
        {
            Crime = crime;
        }
    }
}
