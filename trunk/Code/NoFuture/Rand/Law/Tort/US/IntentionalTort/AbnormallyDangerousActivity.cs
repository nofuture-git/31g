using System;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Tort.US.IntentionalTort
{
    /// <summary>
    /// A kind of trespass where the cause does not require much, if any, proof.
    /// </summary>
    [Aka("absolute liability", "ultrahazardous activity")]
    public class AbnormallyDangerousActivity : TrespassToProperty
    {
        public AbnormallyDangerousActivity(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        public Predicate<ILegalProperty> IsExplosives { get; set; } = pr => false;
        public Predicate<ILegalProperty> IsToxicMaterial { get; set; } = pr => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var subj = GetSubjectPerson(persons);
            if (subj == null)
                return false;
            var title = subj.GetLegalPersonTypeName();

            var property = SubjectProperty;
            if (property == null)
            {
                AddReasonEntry($"{title} {subj.Name}, {nameof(SubjectProperty)} returned nothing");
                return false;
            }

            var isExplosive = IsExplosives(property);
            var isToxic = IsToxicMaterial(property);

            if (!isExplosive && !isToxic)
            {
                AddReasonEntry($"{title} {subj.Name}, both {nameof(IsExplosives)} " +
                               $"and {nameof(IsToxicMaterial)} are both false");
                return false;
            }

            if (!IsPhysicalDamage(persons))
            {
                AddReasonEntry($"{title} {subj.Name}, {nameof(IsPhysicalDamage)} is false");
                return false;
            }

            return true;
        }
    }
}
