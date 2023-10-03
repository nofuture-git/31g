using System;
using System.Collections.Generic;
using NoFuture.Law.Attributes;
using NoFuture.Law.US;

namespace NoFuture.Law.Tort.US.Terms
{
    /// <summary>
    /// a manner or procedure which reflects the judgment, experience and conduct of many
    /// </summary>
    [Aka("convention","mores")]
    public class CustomsTerm : TermLegalConcept
    {
        public CustomsTerm(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        public CustomsTerm():this(ExtensionMethods.Tortfeasor) { }

        protected override string CategoryName => "Customs";

        /// <summary>
        /// Wise &amp; Co. v. Wecoline Products, 36 N.E.2d 623 (N.Y. 1941)
        /// Alberts v. Mutual Serv. Cas. Ins. Co., 123 N.W.2d 96 (S.D. 1963)
        /// </summary>
        public bool IsGeneralPractice { get; set; } = true;

        /// <summary>
        /// Dial v. Lathrop R-II Sch. Dist., 871 S.W.2d 444, 449 (Mo. 1994)
        /// </summary>
        public bool IsDefiniteInForm { get; set; } = true;

        /// <summary>
        /// Dial v. Lathrop R-II Sch. Dist., 871 S.W.2d 444, 449 (Mo. 1994)
        /// </summary>
        public bool IsFixedOverTime { get; set; } = true;

        /// <summary>
        /// Dial v. Lathrop R-II Sch. Dist., 871 S.W.2d 444, 449 (Mo. 1994)
        /// </summary>
        public bool IsReasonable { get; set; } = true;

        /// <summary>
        /// Dial v. Lathrop R-II Sch. Dist., 871 S.W.2d 444, 449 (Mo. 1994)
        /// </summary>
        public bool IsOldEnoughToBeKnown { get; set; } = true;

        /// <summary>
        /// ee El Encanto, Inc. v. Boca Raton Club, Inc., 68 So.2d 819 (Fla. 1953)
        /// </summary>
        public bool IsPracticedByManyAmongGroup { get; set; } = true;

        /// <summary>
        /// ee El Encanto, Inc. v. Boca Raton Club, Inc., 68 So.2d 819 (Fla. 1953)
        /// </summary>
        public bool IsNotIsolatedInUse { get; set; } = true;

        public Predicate<ILegalPerson> IsConformedTo { get; set; } = lp => false;

        protected internal virtual bool IsValidCustom()
        {
            var p2v = new Dictionary<string, bool>
            {
                {nameof(IsGeneralPractice), IsGeneralPractice},
                {nameof(IsDefiniteInForm), IsDefiniteInForm},
                {nameof(IsFixedOverTime), IsFixedOverTime},
                {nameof(IsReasonable), IsReasonable},
                {nameof(IsOldEnoughToBeKnown), IsOldEnoughToBeKnown},
                {nameof(IsPracticedByManyAmongGroup), IsPracticedByManyAmongGroup},
                {nameof(IsNotIsolatedInUse), IsNotIsolatedInUse},
            };

            var result = true;
            foreach (var p in p2v.Keys)
            {
                var v = p2v[p];
                if (v == false)
                {
                    AddReasonEntry($"{p} is false");
                    result = false;
                }
            }

            return result;
        }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var subj = GetSubjectPerson(persons);
            if (subj == null)
            {
                AddReasonEntry($"{nameof(GetSubjectPerson)} returned nothing");
                return false;
            }
            var title = subj.GetLegalPersonTypeName();

            if (!IsValidCustom())
            {
                AddReasonEntry($"{title} {subj.Name}, {GetType().Name} {nameof(IsValidCustom)} is false");
                return false;
            }

            if (!IsConformedTo(subj))
            {
                AddReasonEntry($"{title} {subj.Name}, {GetType().Name} {nameof(IsConformedTo)} is false");
                return false;
            }

            return true;
        }

        public override int GetRank()
        {
            return new RulesTerm().GetRank();
        }
    }
}
