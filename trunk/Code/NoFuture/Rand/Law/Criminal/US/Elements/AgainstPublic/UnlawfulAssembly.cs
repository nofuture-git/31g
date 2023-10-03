using System;
using NoFuture.Law.Criminal.US.Elements.Intent.ComLaw;
using NoFuture.Law.Criminal.US.Elements.Intent.PenalCode;
using NoFuture.Law.US;

namespace NoFuture.Law.Criminal.US.Elements.AgainstPublic
{
    /// <summary>
    /// assembling or meeting of a group of people with intent to commit some unlawful act or riot
    /// </summary>
    public class UnlawfulAssembly : DisorderlyConduct
    {
        /// <summary>
        /// To be an assembly (as in a congregation of people) one must be considered a member of it
        /// </summary>
        public Predicate<ILegalPerson> IsGroupMember { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = this.Defendant(persons);
            if (defendant == null)
                return false;
            var title = defendant.GetLegalPersonTypeName();
            if (!IsGroupMember(defendant))
            {
                AddReasonEntry($"{title} {defendant.Name}, {nameof(IsGroupMember)} is false");
                return false;
            }

            return base.IsValid(persons);
        }

        public override bool CompareTo(IMensRea criminalIntent, params ILegalPerson[] persons)
        {
            var validIntent = criminalIntent is Purposely || criminalIntent is SpecificIntent;
            if (!validIntent)
            {
                AddReasonEntry($"{nameof(UnlawfulAssembly)} requires intent " +
                               $"of {nameof(Purposely)}, {nameof(SpecificIntent)}");
                return false;
            }

            return true;
        }
    }
}
