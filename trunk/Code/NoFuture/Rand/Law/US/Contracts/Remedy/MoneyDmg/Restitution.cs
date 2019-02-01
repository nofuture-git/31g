using System;

namespace NoFuture.Rand.Law.US.Contracts.Remedy.MoneyDmg
{
    /// <inheritdoc />
    /// <summary>
    /// commodum ex injuria sua nemo habere debet: 
    /// No one shall profit by his own wrong
    /// </summary>
    /// <remarks>
    /// In contract law regards money remedy not from loss but from wrongful profit.
    /// This is available remedy for payment-in-kind contracts where one party fails to 
    /// perform.
    /// </remarks>
    public class Restitution<T> : MoneyDmgBase<T> where T : IObjectiveLegalConcept
    {
        public Restitution(IContract<T> contract) : base(contract)
        {
        }

        public Func<ILegalPerson, decimal> CalcUnjustImpoverishment { get; set; }

        public Func<ILegalPerson, decimal> CalcUnjustGain { get; set; }

        protected internal override decimal CalcLoss(ILegalPerson lp)
        {
            return CalcUnjustGain(lp) + CalcUnjustImpoverishment(lp);
        }
    }
}
