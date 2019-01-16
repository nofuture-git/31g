using System;

namespace NoFuture.Rand.Law.US.Contracts.Defense.ToAssent
{
    /// <summary>
    /// <![CDATA[
    /// improper pressure to enter into a contract not the result of direct threats
    /// ]]>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ByUndueInfluence<T> : DefenseBase<T>, IVoidable
    {
        public ByUndueInfluence(IContract<T> contract) : base(contract)
        {
        }
        /// <summary>
        /// The objective test for the legal concept
        /// </summary>
        /// <param name="offeror">
        /// <![CDATA[
        /// a.k.a. "dominant" party, 
        /// "fiduciaries" a party in a position of trust (e.g. lawyers, 
        /// physicians, trustees, guardians, etc.)
        /// ]]>
        /// </param>
        /// <param name="offeree">
        /// <![CDATA[
        /// a.k.a. "servient" party
        /// ]]>
        /// </param>
        /// <returns></returns>
        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            if (!base.IsValid(offeror, offeree))
                return false;


            throw new NotImplementedException();
        }
    }
}
