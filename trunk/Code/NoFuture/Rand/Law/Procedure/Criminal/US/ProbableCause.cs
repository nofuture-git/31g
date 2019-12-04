using System;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Persons;

namespace NoFuture.Rand.Law.Procedure.Criminal.US
{
    /// <summary>
    /// quantity of facts and circumstances withing the police officer&apos;s knowledge that would
    /// warrant a reasonable law-enforcement officer to conclude criminal activity
    /// </summary>
    public class ProbableCause : LegalConcept, IRankable
    {
        /// <summary>
        /// Who is the source of the information passed into <see cref="IsInformationSourceCredible"/>
        /// </summary>
        public Func<ILegalPerson[], ILegalPerson> GetInformationSource { get; set; } = lps => lps.LawEnforcement();

        /// <summary>
        /// Who is making the judgment concerning <see cref="IsFactsConcludeToCriminalActivity"/>
        /// </summary>
        public Func<ILegalPerson[], ILegalPerson> GetLawEnforcement { get; set; } = lps => lps.LawEnforcement();

        /// <summary>
        /// Tautological nature of this concept is by design since its intended to be flexible
        /// </summary>
        /// <remarks>
        /// What is &quot;reasonable&quot; will hinge on the facts and circumstances
        /// </remarks>
        public Predicate<ILegalPerson> IsFactsConcludeToCriminalActivity { get; set; } = lp => false;

        /// <summary>
        /// Asserts that the information-source for the reasonable conclusion of criminal activity is credible
        /// </summary>
        /// <remarks>
        /// Will default to <see cref="DefaultIsInformationSourceCredible"/>
        /// </remarks>
        public Predicate<ILegalPerson> IsInformationSourceCredible { get; set; } 

        public override bool IsValid(params ILegalPerson[] persons)
        {
            if (IsInformationSourceCredible == null)
            {
                IsInformationSourceCredible = DefaultIsInformationSourceCredible;
            }

            var officer = GetLawEnforcement(persons) ?? GetInformationSource(persons);
            if (officer == null)
            {
                AddReasonEntry($"{nameof(GetLawEnforcement)} and {nameof(GetInformationSource)}" +
                               " both returned nothing");
                return false;
            }

            var officerTitle = officer.GetLegalPersonTypeName();

            if (!IsFactsConcludeToCriminalActivity(officer))
            {
                AddReasonEntry($"{officerTitle} {officer.Name}, " +
                               $"{nameof(IsFactsConcludeToCriminalActivity)} is false");
                return false;
            }

            var sourcePerson = GetInformationSource(persons);
            if (sourcePerson == null)
            {
                AddReasonEntry($"{nameof(GetInformationSource)} returned nothing");
                return false;
            }

            var sourcePersonTitle = sourcePerson.GetLegalPersonTypeName();
            if (!IsInformationSourceCredible(sourcePerson))
            {
                AddReasonEntry($"{sourcePersonTitle} {sourcePerson.Name}, " +
                               $"{nameof(IsInformationSourceCredible)} is false");
                return false;
            }

            return true;
        }

        public virtual int GetRank()
        {
            return 2;
        }

        /// <summary>
        /// Default implementation of <see cref="IsInformationSourceCredible"/> based on &quot;Aguilar/Spinelli test&quot; 
        /// </summary>
        protected virtual bool DefaultIsInformationSourceCredible(ILegalPerson lp)
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

            return informant.IsInformationSufficientlyReliable && informant.IsPersonSufficientlyCredible;
        }
    }
}
