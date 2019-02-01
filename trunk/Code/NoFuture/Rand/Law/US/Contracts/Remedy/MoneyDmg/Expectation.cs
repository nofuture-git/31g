using System;
using NoFuture.Rand.Law.US.Contracts.Ucc;

namespace NoFuture.Rand.Law.US.Contracts.Remedy.MoneyDmg
{
    /// <inheritdoc />
    /// <summary>
    /// <![CDATA[
    /// Restatement (Second) of Contracts § 344(a) 
    /// The difference between the expected value and the actual value 
    /// ]]>
    /// </summary>
    public class Expectation<T> : MoneyDmgBase<T> where T : IObjectiveLegalConcept
    {
        public Expectation(IContract<T> contract) : base(contract)
        {
            if (contract is UccContract<Goods>)
            {
                CalcLossToInjured = lp => UccOrigContractPrice(lp) - UccMarketPrice(lp);
            }
        }

        /// <summary>
        /// UCC 2-708(1) the calculation of loss to injured is this less <see cref="UccMarketPrice"/> 
        /// </summary>
        public Func<ILegalPerson, decimal> UccOrigContractPrice { get; set; } = g => 0m;

        /// <summary>
        /// UCC 2-708(1) the calculation of loss to injured is <see cref="UccOrigContractPrice"/> less this 
        /// </summary>
        public Func<ILegalPerson, decimal> UccMarketPrice { get; set; } = g => 0m;
    }
}
