using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Persons;

namespace NoFuture.Rand.Law.Tort.US.Terms
{
    /// <summary>
    /// <![CDATA[ Section 339 of the Restatement (Second) of Torts (1965). ]]>
    /// </summary>
    public class AttractiveNuisanceTerm : TermLegalConcept
    {
        protected override string CategoryName => "attractive nuisance";
        public ILegalProperty SubjectProperty { get; set; }

        public AttractiveNuisanceTerm(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        public Predicate<ILegalProperty> IsLocatedWhereChildrenLikelyAre { get; set; } = lp => false;

        public Predicate<ILegalProperty> IsDangerToChildren { get; set; } = lp => false;

        public Predicate<ILegalProperty> IsDangerOutweighUse { get; set; } = lp => true;

        public Predicate<ILegalProperty> IsArtificialCondition { get; set; } = lp => true;

        public Predicate<IChild> IsChildObliviousToDanger { get; set; }

        public Predicate<ILegalPerson> IsOwnerFailMitigateDanger { get; set; } = lp => true;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            if (SubjectProperty?.IsEntitledTo == null)
            {
                AddReasonEntry($"{nameof(SubjectProperty)} {nameof(SubjectProperty.IsEntitledTo)} is unassigned");
                return false;
            }

            var subj = GetSubjectPerson(persons);
            if (subj == null)
                return false;
            var title = subj.GetLegalPersonTypeName();

            if (SubjectProperty.IsEntitledTo != null && !SubjectProperty.IsEntitledTo(subj))
            {
                AddReasonEntry($"property named '{SubjectProperty.Name}' is " +
                               $"not {nameof(SubjectProperty.IsEntitledTo)} {title} {subj.Name} ");
                return false;
            }

            if (!IsLocatedWhereChildrenLikelyAre(SubjectProperty))
            {
                AddReasonEntry($"property '{SubjectProperty.Name}', {nameof(IsLocatedWhereChildrenLikelyAre)} is false");
                return false;
            }

            if (!IsDangerToChildren(SubjectProperty))
            {
                AddReasonEntry($"property '{SubjectProperty.Name}', {nameof(IsDangerToChildren)} is false");
                return false;
            }

            if (!IsDangerOutweighUse(SubjectProperty))
            {
                AddReasonEntry($"property '{SubjectProperty.Name}', {nameof(IsDangerOutweighUse)} is false");
                return false;
            }

            if (!IsArtificialCondition(SubjectProperty))
            {
                AddReasonEntry($"property '{SubjectProperty.Name}', {nameof(IsArtificialCondition)} is false");
                return false;
            }

            if (IsChildObliviousToDanger != null)
            {
                var children = persons.Where(p => p is IChild).Cast<IChild>().ToList();
                foreach (var child in children)
                {
                    if (!IsChildObliviousToDanger(child))
                    {
                        AddReasonEntry($"{child.GetLegalPersonTypeName()} {child.Name}, {nameof(IsChildObliviousToDanger)} is false");
                        return false;
                    }
                }
            }

            if (!IsOwnerFailMitigateDanger(subj))
            {
                AddReasonEntry($"{title} {subj.Name}, {nameof(IsOwnerFailMitigateDanger)} is false");
                return false;
            }

            return true;
        }
    }
}
