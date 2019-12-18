using System;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Contract.US.Defense.ToAssent
{
    /// <summary>
    /// <![CDATA[
    /// threat of force or other unlawful action to induce to consent
    /// ]]>
    /// </summary>
    [Aka("offer (s)he couldn't refuse")]
    public class ByDuress<T> : DefenseBase<T> where T : ILegalConcept
    {
        public ByDuress(IContract<T> contract) : base(contract) { }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var offeror = this.Offeror(persons);
            var offeree = this.Offeree(persons);
            if (offeree == null || offeror == null)
                return false;

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

        public ImproperThreat<T> ImproperThreat { get; set; }
    }
}
