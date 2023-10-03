namespace NoFuture.Law.Property.US.Terms.CoOwnership
{
    /// <summary>
    /// Each cotenant is due a portion of any 3rd party rent payed according to their fraction-share
    /// </summary>
    public class ThirdPartyRentTerm : CoOwnerBaseTerm
    {
        public ThirdPartyRentTerm(string name, ILegalProperty reference) : base(name, reference)
        {
        }

        public ThirdPartyRentTerm(string name, ILegalProperty reference, params ITermCategory[] categories) : base(name, reference, categories)
        {
        }
    }
}
