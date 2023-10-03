namespace NoFuture.Law.Property.US.Terms.CoOwnership
{
    /// <summary>
    /// Cotenants are not obligated to pay rent to each other unless one has been ousted the other. 
    /// </summary>
    public class NoCotenantRentTerm : CoOwnerBaseTerm
    {
        public NoCotenantRentTerm(string name, ILegalProperty reference) : base(name, reference)
        {
        }

        public NoCotenantRentTerm(string name, ILegalProperty reference, params ITermCategory[] categories) : base(name, reference, categories)
        {
        }
    }
}
