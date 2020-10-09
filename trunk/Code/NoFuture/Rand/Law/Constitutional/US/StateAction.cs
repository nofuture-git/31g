using System;
using NoFuture.Rand.Law.Property.US;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Constitutional.US
{
    /// <summary>
    /// The US Constitution doctrine of when a person is protected against incursions by government 
    /// </summary>
    public class StateAction : PropertyConsent
    {
        public StateAction() : base(ExtensionMethods.DefendantFx)
        {
        }

        /// <summary>
        /// Marsh v. Alabama 326 U.S. 501 (1946), if it looks like a town and operates like a town then it _is_ a town.
        /// </summary>
        public Predicate<ILegalProperty> IsPublicCommunity { get; set; } = p => false;

        public Func<ILegalPerson, IAct> GetActByPerson { get; set; } = p => null;

        public Predicate<IAct> IsProtectedRight { get; set; } = p => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            GetSubjectPerson = GetSubjectPerson ?? ExtensionMethods.DefendantFx;

            var subj = GetSubjectPerson(persons);
            var title = subj.GetLegalPersonTypeName();
            if (subj == null)
            {
                AddReasonEntry($"{GetSubjectPerson} did not return any one");
                return false;
            }

            var withConsent = !WithoutConsent(persons);
            if (withConsent)
            {
                AddReasonEntry($"{nameof(WithoutConsent)} returned false - therefore consent was given");
                return false;
            }

            var act = GetActByPerson(subj);

            if (act == null)
            {
                AddReasonEntry($"{nameof(GetActByPerson)} returned nothing");
                return false;
            }

            if (!act.IsValid(subj))
            {
                AddReasonEntryRange(act.GetReasonEntries());
                return false;
            }

            if (!IsProtectedRight(act))
            {
                AddReasonEntry($"The {nameof(IsProtectedRight)} returned false for the given act of person {title} {subj.Name}");
                return false;
            }

            if (!IsPublicCommunity(SubjectProperty))
            {
                AddReasonEntry($"{IsPublicCommunity} for {nameof(SubjectProperty)} {SubjectProperty.Name} is false");
                return false;
            }

            return true;
        }
    }
}
