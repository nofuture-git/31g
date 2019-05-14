using System;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Defense;

namespace NoFuture.Rand.Law.Contract.US.Defense.ToAssent
{
    /// <inheritdoc cref="IDuress"/>
    public class ByDuress<T> : DefenseBase<T>, IDuress where T : ILegalConcept
    {
        public ByDuress(IContract<T> contract) : base(contract) { }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var offeror = persons.Offeror();
            var offeree = persons.Offeree();

            if (!base.IsValid(offeror, offeree))
                return false;
            if (IsAssentByPhysicalCompulsion(offeror))
            {
                AddReasonEntry($"{offeror.Name} assent induced by physical compulsion");
                return true;
            }
            if (IsAssentByPhysicalCompulsion(offeree))
            {
                AddReasonEntry($"{offeree.Name} assent induced by physical compulsion");
                return true;
            }

            if (ImproperThreat == null)
                return false;

            AddReasonEntry($"there was an {nameof(ImproperThreat)}");

            var improperThreatRslt = ImproperThreat.IsValid(offeror, offeree);
            if (improperThreatRslt)
            {
                AddReasonEntryRange(ImproperThreat.GetReasonEntries());
            }

            return improperThreatRslt;
        }

        public Predicate<ILegalPerson> IsAssentByPhysicalCompulsion { get; set; } = llp => false;

        public IImproperThreat ImproperThreat { get; set; }
    }
}
