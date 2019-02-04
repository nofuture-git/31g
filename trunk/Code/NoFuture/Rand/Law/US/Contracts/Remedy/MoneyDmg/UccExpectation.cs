using System;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.US.Contracts.Ucc;

namespace NoFuture.Rand.Law.US.Contracts.Remedy.MoneyDmg
{
    /// <inheritdoc />
    [Aka("UCC 2-708")]
    public class UccExpectation : MoneyDmgBase<Goods>
    {
        public UccExpectation(IContract<Goods> contract) : base(contract)
        {
        }

        /// <summary>
        /// UCC 2-708(1) the calculation of loss to injured is this less <see cref="UccMarketPrice"/> 
        /// </summary>
        public Func<ILegalPerson, decimal> UccOrigContractPrice { get; set; } = g => 0m;

        /// <summary>
        /// UCC 2-708(1) the calculation of loss to injured is <see cref="UccOrigContractPrice"/> less this 
        /// </summary>
        public Func<ILegalPerson, decimal> UccMarketPrice { get; set; } = g => 0m;

        protected internal override decimal CalcMoneyRemedy(ILegalPerson lp)
        {
            return UccOrigContractPrice(lp) - UccMarketPrice(lp);
        }
    }
}
