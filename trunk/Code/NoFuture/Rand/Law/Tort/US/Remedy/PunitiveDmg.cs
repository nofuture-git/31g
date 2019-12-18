using System;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Tort.US.Remedy
{
    /// <summary>
    /// Damages which are meant to punish the defendant typically for some intentional tort.
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// 1. Compensatory damages do not always compensate fully.
    /// 2. Compensatory damages may be too low to provide a deterrent.
    ///  - e.g. steal a car then advise owner to sue for the value in court
    ///  - e.g. perv pays lightly for some nasty act
    ///  - e.g. tortfeaser is very rich and compensation is easy to pay
    /// 3. Compensatory damages only regard personal costs, not social costs.
    /// 4. Punitive damages relieve some burdan of the criminal justice system.
    /// Kemezy v. Peters, 79 F.3d 33 (7th Cir. 1996)
    /// ]]>
    /// </remarks>
    public class PunitiveDmg : MoneyDmgBase
    {
        public PunitiveDmg(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        public Func<ILegalPerson, decimal> CalcCost { get; set; } = lp => 0m;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var subj = GetSubjectPerson(persons);
            if (subj == null)
            {
                AddReasonEntry($"{nameof(GetSubjectPerson)} returned nothing");
                return false;
            }
            var title = subj.GetLegalPersonTypeName();

            var cost = CalcCost(subj);
            AddReasonEntry($"{title} {subj.Name}, {nameof(CalcCost)} is {cost}");

            return cost > 0m;
        }
    }
}
