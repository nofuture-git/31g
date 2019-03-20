using System;
using NoFuture.Rand.Law.US.Elements;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.Tort.US.Elements
{
    /// <summary>
    /// <![CDATA[ RESTATEMENT (SECOND) OF TORTS § 218 (1965). ]]>
    /// </summary>
    [Aka("trespass to personal property")]
    public class TrespassToChattels : TrespassBase
    {
        public Predicate<ILegalPerson> IsCauseDispossession { get; set; } = lp => false;
        public Predicate<ILegalPerson> IsInjuryToPossessor { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            throw new NotImplementedException();
        }
    }
}
