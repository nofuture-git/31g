using System;
using System.Linq;
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

        /// <summary>
        /// Shelley v. Kraemer 334 U.S. 1 (1948), denial of land and home - true
        /// Moose Lodge v. Irvis, 407 U.S. 163 (1972), denial of drinks and dinner - false
        /// </summary>
        public Predicate<IAct> IsInvidiousDiscrimination { get; set; } = a => false;

        /// <summary>
        /// A function, defined by the implementor, for determining what person did what action
        /// </summary>
        public Func<ILegalPerson, IAct> GetActByPerson { get; set; } = p => null;

        public Predicate<IAct> IsProtectedRight { get; set; } = p => false;

        /// <summary>
        /// Jackson v. Metropolitan Edison Co., 419 U.S., at 350, there is sufficiently close
        /// nexus between the State and the challenged action of the regulated entity so that the
        /// action of the latter may be fairly treated as that of the State itself
        /// </summary>
        public Predicate<IAct> IsCloseConnectionToState { get; set; } = a => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            GetSubjectPerson = GetSubjectPerson ?? ExtensionMethods.DefendantFx;

            var withConsent = !WithoutConsent(persons);
            if (withConsent)
            {
                AddReasonEntry($"{nameof(WithoutConsent)} returned false - therefore consent was given");
                return false;
            }

            var actorPerson = persons.FirstOrDefault(p => GetActByPerson(p) != null);

            var act = GetActByPerson(actorPerson);


            if (actorPerson == null || act == null)
            {
                AddReasonEntry($"{nameof(GetActByPerson)} returned nothing");
                return false;
            }

            var actorTitle = actorPerson.GetLegalPersonTypeName();

            if (!act.IsValid(actorPerson))
            {
                AddReasonEntryRange(act.GetReasonEntries());
                return false;
            }

            //most obvious case
            if (IsPublicCommunity(SubjectProperty) && IsProtectedRight(act))
                return true;

            if (IsCloseConnectionToState(act))
            {
                AddReasonEntry($"Act, {act.GetType().Name}, for person {actorTitle} " +
                               $"{actorPerson.Name}, {nameof(IsCloseConnectionToState)} is true");

                return true;
            }

            var isPrivate = !IsPublicCommunity(SubjectProperty);

            if (isPrivate && IsInvidiousDiscrimination(act))
            {
                AddReasonEntry($"{nameof(IsPublicCommunity)} for {nameof(SubjectProperty)} {SubjectProperty.Name} is false");
                AddReasonEntry($"Act, {act.GetType().Name}, for person {actorTitle} " +
                               $"{actorPerson.Name}, {nameof(IsInvidiousDiscrimination)} is true");
                return true;
            }

            return false;

        }
    }
}
