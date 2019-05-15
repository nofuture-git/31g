using System;
using System.Linq;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.ComLaw;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.PenalCode;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Criminal.AgainstGov.US.Elements
{
    /// <summary>
    /// the interference with any aspect of lay enforcement procedure, prosecution or conviction
    /// </summary>
    public class ObstructionOfJustice : LegalConcept, IActusReus
    {
        /// <summary>
        /// giving false identification to a law enforcement officer
        /// </summary>
        public Predicate<ILegalPerson> IsGiveFalseIdToAgent { get; set; } = lp => false;

        /// <summary>
        /// impersonating a law enforcement officer
        /// </summary>
        public Predicate<ILegalPerson> IsImpersonateAgent { get; set; } = lp => false;

        /// <summary>
        /// refusing to aid a law enforcement officer when requested
        /// </summary>
        public Predicate<ILegalPerson> IsRefuseAidAgentUponRequest { get; set; } = lp => false;

        /// <summary>
        /// giving false evidence
        /// </summary>
        public Predicate<ILegalPerson> IsGiveFalseEvidence { get; set; } = lp => false;

        /// <summary>
        /// hiding or concealing oneself and refusing to give evidence
        /// </summary>
        public Predicate<ILegalPerson> IsRefuseToGiveEvidence { get; set; } = lp => false;

        /// <summary>
        /// tampering with evidence
        /// </summary>
        public Predicate<ILegalPerson> IsTamperingWithEvidence { get; set; } = lp => false;

        /// <summary>
        /// and tampering with a witness or juror
        /// </summary>
        public Predicate<ILegalPerson> IsTamperingWithWitnessJuror { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = persons.Defendant();
            if (defendant == null)
                return false;

            var falseId = IsGiveFalseIdToAgent(defendant);
            var impersonate = IsImpersonateAgent(defendant);
            var refuseAid = IsRefuseAidAgentUponRequest(defendant);
            var falseEvidence = IsGiveFalseEvidence(defendant);
            var refuseEvidence = IsRefuseToGiveEvidence(defendant);
            var isTamperWitness = IsTamperingWithEvidence(defendant);
            var isTamperJuror = IsTamperingWithWitnessJuror(defendant);

            if (new [] {falseId, impersonate, refuseAid, falseEvidence,
                        refuseEvidence, isTamperWitness, isTamperJuror}.All(p => p == false))
            {
                AddReasonEntry($"defendant {defendant.Name}, {nameof(IsGiveFalseIdToAgent)}," +
                               $"{nameof(IsImpersonateAgent)}," +
                               $"{nameof(IsRefuseAidAgentUponRequest)}," +
                               $"{nameof(IsGiveFalseEvidence)}," +
                               $"{nameof(IsRefuseToGiveEvidence)}," +
                               $"{nameof(IsTamperingWithEvidence)}," +
                               $"{nameof(IsTamperingWithWitnessJuror)} are all false");
                return false;
            }

            return true;
        }

        public bool CompareTo(IMensRea criminalIntent, params ILegalPerson[] persons)
        {
            var isValid = criminalIntent is Purposely || criminalIntent is SpecificIntent
                                                      || criminalIntent is Knowingly || criminalIntent is GeneralIntent;
            if (!isValid)
            {
                AddReasonEntry($"{nameof(Bribery)} requires intent of {nameof(Purposely)}, " +
                               $"{nameof(SpecificIntent)}, {nameof(Knowingly)}, {nameof(GeneralIntent)}");
                return false;
            }

            return true;
        }
    }
}
