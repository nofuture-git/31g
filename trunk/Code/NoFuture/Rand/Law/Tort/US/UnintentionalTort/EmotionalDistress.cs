using System;
using System.Linq;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.Criminal;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Persons;

namespace NoFuture.Rand.Law.Tort.US.UnintentionalTort
{
    /// <summary>
    /// Emotional trauma from being a witness to violence against loved ones.
    /// Not considered a matter of fear and stress in the mind,
    /// but the immediate visceral horrors presented from the senses whose
    /// mind-shattering affect is feared by all.
    /// </summary>
    /// <remarks>
    /// Dillon v. Legg, 441 P.2d 912 (Cal. 1968)
    /// </remarks>
    [Aka("bystander proximity test")]
    public class EmotionalDistress : UnoHomine, INegligence, IBattery
    {
        public EmotionalDistress(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        public Predicate<ILegalPerson> IsByViolence { get; set; }

        public Predicate<IPlaintiff> IsNearSceneOfAccident { get; set; }

        public Predicate<IPlaintiff> IsDirectPrimaryWitness { get; set; }

        public Func<IVictim, IPlaintiff, bool> IsCloselyRelated { get; set; }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var subj = GetSubjectPerson(persons);
            if (subj == null)
                return false;
            var title = subj.GetLegalPersonTypeName();

            if (!IsByViolence(subj))
            {
                AddReasonEntry($"{title} {subj.Name}, {nameof(IsByViolence)} is false");
                return false;
            }

            var plaintiff = this.Plaintiff(persons);
            if (plaintiff == null)
                return false;

            var victims = persons.Victims().Cast<IVictim>().ToList();
            var nameTitles = persons.Select(p => Tuple.Create(p.GetLegalPersonTypeName(), p.Name));

            if (!victims.Any())
            {
                AddReasonEntry($"No one is the {nameof(IVictim)} in {nameTitles}");
                return false;
            }

            title = plaintiff.GetLegalPersonTypeName();
            if (!IsNearSceneOfAccident(plaintiff))
            {
                AddReasonEntry($"{title} {plaintiff.Name}, {nameof(IsNearSceneOfAccident)} is false");
                return false;
            }

            if (!IsDirectPrimaryWitness(plaintiff))
            {
                AddReasonEntry($"{title} {plaintiff.Name}, {nameof(IsDirectPrimaryWitness)} is false");
            }

            if (victims.All(victim => IsCloselyRelated(victim, plaintiff) == false))
            {
                nameTitles = victims.Select(p => Tuple.Create(p.GetLegalPersonTypeName(), p.Name));
                AddReasonEntry($"{title} {plaintiff}, {nameof(IsCloselyRelated)} is false for all persons {nameTitles}");
                return false;
            }

            return true;
        }

    }
}
