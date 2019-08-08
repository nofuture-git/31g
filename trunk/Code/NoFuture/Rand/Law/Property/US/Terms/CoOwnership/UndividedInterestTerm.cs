namespace NoFuture.Rand.Law.Property.US.Terms.CoOwnership
{
    /// <summary>
    /// Each owner has full rights to occupy and use all the property regardless of the size of their share (e.g. 25/75)
    /// </summary>
    public class UndividedInterestTerm : CoOwnerBaseTerm
    {
        public UndividedInterestTerm(string name, ILegalProperty reference) : base(name, reference)
        {
        }

        public UndividedInterestTerm(string name, ILegalProperty reference, params ITermCategory[] categories) : base(name, reference, categories)
        {
        }

        /// <summary>
        /// Each cotenant has equal rights to posses and use all of the property
        /// </summary>
        /// <param name="lp"></param>
        /// <returns></returns>
        public virtual bool IsRightToOccupyUseWholeProperty(ILegalPerson lp)
        {
            return RefersTo.IsEntitledTo(lp);
        }
    }
}
