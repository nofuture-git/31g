using System;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.US.Contracts.Defense.ToAssent
{
    /// <summary>
    /// <![CDATA[
    /// threat of force or other unlawful action to induce to consent
    /// ]]>
    /// </summary>
    [Aka("offer (s)he couldn't refuse")]
    public class ByDuress<T> : DefenseBase<T>
    {
        public ByDuress(IContract<T> contract) : base(contract) { }

        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
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
