using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Property.US.Acquisition
{
    /// <summary>
    /// Idea of using some material without being considered an infringement on the author&apos;s copyright
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// Copyright Act provides guide for considering fair use
    /// (1) purpose of use (nonprofit edu v. commercial)
    /// (2) nature of work itself (fiction v. non-fiction)
    /// (3) proportion of copied material
    /// (4) effect on work's economic value
    /// ]]>
    /// </remarks>
    public class FairUse : PropertyConsent
    {
        public FairUse(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        [Aka("reporting", "teaching", "research", "scholarship")]
        public Predicate<ILegalPerson> IsCommentary { get; set; } = lp => false;

        public Predicate<ILegalPerson> IsCriticism { get; set; } = lp => false;

        /// <summary>
        /// uses some mimic of an original work to make a point
        /// </summary>
        [Eg("mimic", "make fun of", "lampooned", "distorted imitation")]
        [Cf("satire: broad ridicule the mores of a society requiring " +
            "no reference to a particular artistic work")]
        public Predicate<ILegalPerson> IsParody { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var subj = GetSubjectPerson(persons);
            if (subj == null)
            {
                AddReasonEntry($"{nameof(GetSubjectPerson)} returned nothing");
                return false;
            }
            var title = subj.GetLegalPersonTypeName();

            //there is a adversary context 
            if (base.PropertyOwnerIsSubjectPerson(persons))
                return false;
            
            //there is no permission
            if (!base.WithoutConsent(persons))
                return false;

            //any one will do as fair use
            var props = new HashSet<Tuple<bool, string>>
            {
                Tuple.Create(IsCommentary(subj), nameof(IsCommentary)),
                Tuple.Create(IsCriticism(subj), nameof(IsCriticism)),
                Tuple.Create(IsParody(subj), nameof(IsParody)),
            };

            if (props.All(p => p.Item1 == false))
            {
                var propNames = string.Join(", ", props.Select(p => p.Item2));
                AddReasonEntry($"{title} {subj.Name}, {propNames} are all false");
                return false;
            }

            return true;
        }
    }
}
