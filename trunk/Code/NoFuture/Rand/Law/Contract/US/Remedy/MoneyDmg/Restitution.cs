using System;

namespace NoFuture.Rand.Law.Contract.US.Remedy.MoneyDmg
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
    public class Restitution<T> : MoneyDmgBase<T> where T : ILegalConcept
    {
        public Restitution(IContract<T> contract) : base(contract)
        {
        }

        public Func<ILegalPerson, decimal> CalcUnjustImpoverishment { get; set; } = lp => 0m;

        /// <summary>
        /// the injured party is entitled to restitution for any benefit that he 
        /// has conferred on the other party by way of part performance or reliance.
        /// </summary>
        public Func<ILegalPerson, decimal> CalcUnjustGain { get; set; } = lp => 0m;

        protected internal override decimal CalcMoneyRemedy(ILegalPerson lp)
        {
            return CalcUnjustGain(lp) + CalcUnjustImpoverishment(lp);
        }
    }
}
