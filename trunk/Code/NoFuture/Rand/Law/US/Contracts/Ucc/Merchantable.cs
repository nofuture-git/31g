using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.US.Contracts.Ucc
{
    /// <summary>
    /// <![CDATA[
    /// Goods to be merchantable must be at least such as
    /// ]]>
    /// </summary>
    [Aka("UCC 2-314(2)")]
    public class Merchantable : ObjectiveLegalConcept, IUccItem
    {
        private readonly Goods _goods;

        public Merchantable(Goods goods)
        {
            _goods = goods;
        }

        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            if (!IsPassWithoutObjection)
            {
                AddReasonEntry($"{GetType().Name} does not 'pass without " +
                               "objection in the trade under " +
                               "the contract description'");
                return false;
            }

            if (!IsFairAvgQuality)
            {
                AddReasonEntry($"{GetType().Name} is not 'of fair " +
                               "average quality within the description'");
                return false;
            }

            if (!IsFit4OrdinaryPurpose)
            {
                AddReasonEntry($"{GetType().Name} is not 'fit for the " +
                               "ordinary purposes for which such " +
                               "goods are used'");
                return false;
            }

            if (!IsWithinPermittedVariations)
            {
                AddReasonEntry($"{GetType().Name} is not 'within the variations " +
                               "permitted by the agreement, of even kind, " +
                               "quality and quantity within each unit and among " +
                               "all units involved'");
                return false;
            }
            if (!IsPackagedAndLabeled)
            {
                AddReasonEntry($"{GetType().Name} is not 'adequately contained, " +
                               "packaged, and labeled as the agreement may require'");
                return false;
            }
            if (!(IsConformedAsLabeled ?? true))
            {
                AddReasonEntry($"{GetType().Name} does not 'conform to the promise or " +
                               "affirmations of fact made on the container or " +
                               "label if any'");
                return false;
            }

            return true;
        }

        [Aka("UCC 2-314(2)(a)")]
        public bool IsPassWithoutObjection { get; set; }

        [Aka("UCC 2-314(2)(b)")]
        public bool IsFairAvgQuality { get; set; }

        [Aka("UCC 2-314(2)(c)")]
        public bool IsFit4OrdinaryPurpose { get; set; }

        [Aka("UCC 2-314(2)(d)")]
        public bool IsWithinPermittedVariations { get; set; }

        [Aka("UCC 2-314(2)(e)")]
        public bool IsPackagedAndLabeled { get; set; }

        [Aka("UCC 2-314(2)(f)")]
        public bool? IsConformedAsLabeled { get; set; }

        public override bool IsEnforceableInCourt => true;
    }
}
