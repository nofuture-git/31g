using System;
using System.Linq;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Persons;

namespace NoFuture.Rand.Law.Procedure.Civil.US.Judgment
{
    /// <summary>
    /// Judgment issued by a judge based on the agreement between the parties to a lawsuit to settle the matter
    /// </summary>
    public class ConsentJudgment : CivilProcedureBase, IJudgment, IAssent
    {
        public Predicate<ILegalPerson> IsApprovalExpressed { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            if (persons == null)
                return false;

            if (!IsCourtAssigned())
                return false;

            foreach (var p in persons.Where(p => p is IPlaintiff))
            {
                if (IsApprovalExpressed(p))
                    continue;

                AddReasonEntry($"{nameof(IPlaintiff)} {p.Name} {p.GetLegalPersonTypeName()}, " +
                               $"{nameof(IsApprovalExpressed)} is false");
                return false;
            }

            foreach (var p in persons.Where(p => p is IDefendant))
            {
                if (IsApprovalExpressed(p))
                    continue;

                AddReasonEntry($"{nameof(IDefendant)} {p.Name} {p.GetLegalPersonTypeName()}, " +
                               $"{nameof(IsApprovalExpressed)} is false");
                return false;
            }

            return true;
        }

    }
}
