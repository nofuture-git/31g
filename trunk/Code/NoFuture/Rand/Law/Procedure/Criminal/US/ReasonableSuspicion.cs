using System;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Persons;

namespace NoFuture.Rand.Law.Procedure.Criminal.US
{

    /// <summary>
    /// A reasoning less than <see cref="ProbableCause"/> but above something like &quot;a hunch&quot;
    /// </summary>
    /// <remarks> Allows for stop; however, when facts implicate someone is armed then a frisk is allowed</remarks>
    public class ReasonableSuspicion : ProbableCause
    {
        /// <summary>
        /// Resolves who is the suspect of law-enforcements suspicion
        /// </summary>
        public Func<ILegalPerson[], ILegalPerson> GetSubjectOfStop { get; set; } = lps => lps.Suspect();

        public SuspectStop Stop { get; set; }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            if (Stop == null) 
                return base.IsValid(persons);

            if (Stop.GetSuspect == null)
            {
                Stop.GetSuspect = GetSubjectOfStop;
            }

            if (Stop.GetLawEnforcement == null)
            {
                Stop.GetLawEnforcement = GetLawEnforcement;
            }

            var isValidStop = Stop.IsValid(persons);

            AddReasonEntryRange(Stop.GetReasonEntries());
            return base.IsValid(persons) && isValidStop;
        }

        protected override bool DefaultIsInformationSourceCredible(ILegalPerson lp)
        {
            if (lp is ILawEnforcement)
            {
                AddReasonEntry($"{lp.GetLegalPersonTypeName()} {lp.Name}, is type {nameof(ILawEnforcement)}");
                return true;
            }

            var informant = lp as IInformant;
            if (informant == null)
            {
                AddReasonEntry($"{lp.GetLegalPersonTypeName()} {lp.Name}, is not " +
                               $"of type {nameof(ILawEnforcement)} nor {nameof(IInformant)}");
                return false;
            }

            return informant.IsInformationSufficientlyReliable;
        }

        public override int GetRank()
        {
            return base.GetRank() - 1;
        }
    }
}
