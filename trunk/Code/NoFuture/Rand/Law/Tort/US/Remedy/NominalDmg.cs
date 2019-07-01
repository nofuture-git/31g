using System;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Tort.US.Remedy
{
    /// <summary>
    /// a small amount of money awarded to a
    /// plaintiff to demo that s\he was right but suffered no significant loss.
    /// </summary>
    public class NominalDmg : MoneyDmgBase
    {
        public NominalDmg(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        public Func<ILegalPerson, decimal> CalcAmount { get; set; }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var subj = GetSubjectPerson(persons);
            if (subj == null)
                return false;

            var title = subj.GetLegalPersonTypeName();

            var nominalAmt = CalcAmount(subj);
            AddReasonEntry($"{title} {subj.Name}, {nameof(CalcAmount)} is {nominalAmt}");
            return nominalAmt > 0;
        }
    }
}
