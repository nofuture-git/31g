using System;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Persons;

namespace NoFuture.Rand.Law.Procedure.Criminal.US
{
    /// <summary>
    /// quantity of facts and circumstances withing the police officer&apos;s knowledge that would
    /// warrant a reasonable person to conclude criminal activity
    /// </summary>
    public class ProbableCause : LegalConcept
    {
        public Func<ILegalPerson[], ILegalPerson> GetInformationSource { get; set; } = lps => lps.LawEnforcement();

        public Predicate<ILegalPerson> IsReasonableConcludeCriminalActivity { get; set; } = lp => false;

        public Predicate<ILegalPerson> IsInformationSourceCredible { get; set; } 

        public override bool IsValid(params ILegalPerson[] persons)
        {
            if (IsInformationSourceCredible == null)
            {
                IsInformationSourceCredible = DefaultIsInformationSourceCredible;
            }

            var sourcePerson = GetInformationSource(persons);
            if (sourcePerson == null)
            {
                AddReasonEntry($"{nameof(GetInformationSource)} returned nothing");
                return false;
            }

            var sourcePersonTitle = sourcePerson.GetLegalPersonTypeName();

            if (!IsReasonableConcludeCriminalActivity(sourcePerson))
            {
                AddReasonEntry($"{sourcePersonTitle} {sourcePerson.Name}, " +
                               $"{nameof(IsReasonableConcludeCriminalActivity)} is false");
                return false;
            }

            if (!IsInformationSourceCredible(sourcePerson))
            {
                AddReasonEntry($"{sourcePersonTitle} {sourcePerson.Name}, " +
                               $"{nameof(IsInformationSourceCredible)} is false");
                return false;
            }

            return true;
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
