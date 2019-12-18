using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Contract.US.Ucc
{
    /// <summary>
    /// <![CDATA[
    /// Goods to be merchantable must be at least such as
    /// ]]>
    /// </summary>
    [Aka("UCC 2-314(2)")]
    public class Merchantable : LegalConcept, IUccItem
    {
        private readonly Goods _goods;

        public Merchantable(Goods goods)
        {
            _goods = goods;
        }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var offeror = this.Offeror(persons);
            var offeree = this.Offeree(persons);

            if (!IsPassWithoutObjection)
            {
                AddReasonEntry($"{_goods?.GetType().Name} does not 'pass without " +
                               "objection in the trade under " +
                               "the contract description'");
                return false;
            }

            if (!IsFairAvgQuality)
            {
                AddReasonEntry($"{_goods?.GetType().Name} is not 'of fair " +
                               "average quality within the description'");
                return false;
            }

            if (!IsFit4OrdinaryPurpose)
            {
                AddReasonEntry($"{_goods?.GetType().Name} is not 'fit for the " +
                               "ordinary purposes for which such " +
                               "goods are used'");
                return false;
            }

            if (!IsWithinPermittedVariations)
            {
                AddReasonEntry($"{_goods?.GetType().Name} is not 'within the variations " +
                               "permitted by the agreement, of even kind, " +
                               "quality and quantity within each unit and among " +
                               "all units involved'");
                return false;
            }
            if (!IsPackagedAndLabeled)
            {
                AddReasonEntry($"{_goods?.GetType().Name} is not 'adequately contained, " +
                               "packaged, and labeled as the agreement may require'");
                return false;
            }
            if (!(IsConformedAsLabeled ?? true))
            {
                AddReasonEntry($"{_goods?.GetType().Name} does not 'conform to the promise or " +
                               "affirmations of fact made on the container or " +
                               "label if any'");
                return false;
            }

            if ((IsBuyerRelyingOnSellerJudgement ?? false) && !(IsFit4ParticularPurpose ?? true))
            {
                AddReasonEntry($"{offeror?.Name} has reason to know the particular purpose " +
                               $"of {_goods?.GetType().Name} and the buyer {offeree?.Name} is " +
                               "relying seller's judgement");
                return false;

            }

            return true;
        }

        [Aka("UCC 2-314(2)(a)")]
        public bool IsPassWithoutObjection { get; set; } = true;

        [Aka("UCC 2-314(2)(b)")]
        public bool IsFairAvgQuality { get; set; } = true;

        [Aka("UCC 2-314(2)(c)")]
        public bool IsFit4OrdinaryPurpose { get; set; } = true;

        [Aka("UCC 2-315")]
        public bool? IsFit4ParticularPurpose { get; set; }

        [Aka("UCC 2-315")]
        public bool? IsBuyerRelyingOnSellerJudgement { get; set; }

        [Aka("UCC 2-314(2)(d)")]
        public bool IsWithinPermittedVariations { get; set; } = true;

        [Aka("UCC 2-314(2)(e)")]
        public bool IsPackagedAndLabeled { get; set; } = true;

        [Aka("UCC 2-314(2)(f)")]
        public bool? IsConformedAsLabeled { get; set; }

        public override bool IsEnforceableInCourt => true;

        public override bool Equals(object obj)
        {
            var m = obj as Merchantable;
            if (m == null)
                return false;
            return m.IsPassWithoutObjection == IsPassWithoutObjection
                   && m.IsFairAvgQuality == IsFairAvgQuality
                   && m.IsFit4OrdinaryPurpose == IsFit4OrdinaryPurpose
                   && m.IsFit4ParticularPurpose == IsFit4ParticularPurpose
                   && m.IsBuyerRelyingOnSellerJudgement == IsBuyerRelyingOnSellerJudgement
                   && m.IsWithinPermittedVariations == IsWithinPermittedVariations
                   && m.IsPackagedAndLabeled == IsPackagedAndLabeled
                   && m.IsConformedAsLabeled == IsConformedAsLabeled
                ;
        }

        public override int GetHashCode()
        {
            return IsPassWithoutObjection.GetHashCode() +
                   IsFairAvgQuality.GetHashCode() +
                   IsFit4OrdinaryPurpose.GetHashCode() +
                   IsFit4ParticularPurpose.GetHashCode() +
                   IsBuyerRelyingOnSellerJudgement.GetHashCode() +
                   IsWithinPermittedVariations.GetHashCode() +
                   IsPackagedAndLabeled.GetHashCode()+ 
                   IsConformedAsLabeled?.GetHashCode() ?? 0
                ;

        }
    }
}
