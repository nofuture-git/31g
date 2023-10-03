using System;
using NoFuture.Law.Attributes;
using NoFuture.Law.Contract.US.Ucc;

namespace NoFuture.Law.Contract.US.Remedy.MoneyDmg
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
