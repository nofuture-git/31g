using System;
using System.Linq;
using NoFuture.Law.US;

namespace NoFuture.Law.Criminal.US.Elements.AgainstPersons
{
    /// <inheritdoc cref="SexBilateral"/>
    /// <summary>
    /// This seems to imply that the parties know they have some family relation
    /// </summary>
    public class Incest : SexBilateral, IActusReus
    {
        /// <summary>
        /// Typically being any family member one could not marry
        /// </summary>
        public Func<ILegalPerson, ILegalPerson, bool> IsFamilyRelation { get; set; } = (lp1, lp2) => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = this.Defendant(persons);
            if (defendant == null)
                return false;
            if (!base.IsValid(persons))
                return false;

            var otherPerson = persons.FirstOrDefault(p => IsOneOfTwo(p) && !ReferenceEquals(p, defendant));
            if (otherPerson == null)
            {
                AddReasonEntry($"the {nameof(IsOneOfTwo)} returned null for the other person");
                return false;
            }

            if (!IsSexualIntercourse(otherPerson))
            {
                AddReasonEntry($"other person, {otherPerson.Name}, {nameof(IsSexualIntercourse)} is false");
                return false;
            }

            var isFamily = IsFamilyRelation(defendant, otherPerson);

            AddReasonEntry($"{nameof(IsFamilyRelation)} is {isFamily} for {defendant.Name} and {otherPerson.Name}");
            return isFamily;
        }

        public bool CompareTo(IMensRea criminalIntent, params ILegalPerson[] persons)
        {
            return criminalIntent != null;
        }
    }
}
