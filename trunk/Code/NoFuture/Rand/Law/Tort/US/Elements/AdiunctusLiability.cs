using System;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Tort.US.Elements
{
    /// <summary>
    /// Liability through some kind of association of relationship
    /// </summary>
    [EtymologyNote("Latin", "adiunctus", "associate")]
    public abstract class AdiunctusLiability : UnoHomine, IAct
    {
        protected AdiunctusLiability(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        public Predicate<ILegalPerson> IsVoluntary { get; set; } = lp => false;

        public Predicate<ILegalPerson> IsAction { get; set; } = lp => false;

        public virtual Func<ILegalPerson, ILegalPerson, bool> IsMutuallyBeneficialRelationship { get; set; } =
            (lp1, lp2) => false;

        protected virtual bool Is3rdPartyBeneficialRelationship(ILegalPerson[] persons)
        {
            var subj = GetSubjectPerson(persons);
            if (subj == null)
            {
                AddReasonEntry($"{nameof(GetSubjectPerson)} returned nothing");
                return false;
            }
            var title = subj.GetLegalPersonTypeName();

            var thirdParty = this.ThirdParty(persons);

            if (thirdParty == null)
                return false;

            if (!IsMutuallyBeneficialRelationship(subj, thirdParty))
            {
                AddReasonEntry($"{title} {subj.Name} to {thirdParty.GetLegalPersonTypeName()} " +
                               $"{thirdParty.Name}, {nameof(IsMutuallyBeneficialRelationship)} is false");
                return false;
            }

            return true;
        }
    }
}
