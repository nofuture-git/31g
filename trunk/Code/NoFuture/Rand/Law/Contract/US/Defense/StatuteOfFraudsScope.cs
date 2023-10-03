using System;
using NoFuture.Law.Attributes;

namespace NoFuture.Law.Contract.US.Defense
{
    public class StatuteOfFraudsScope<T> : DefenseBase<T> where T : ILegalConcept
    {
        public StatuteOfFraudsScope(IContract<T> contract) : base(contract)
        {

        }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            Predicate<IContract<T>> dfx = c => false;
            var m = IsMarriageRelated ?? dfx;
            const string PREFIX = "within Statute of Frauds:";
            if (m(Contract))
            {
                AddReasonEntry($"{PREFIX} in contemplation of MARRIAGE");
                return true;
            }
            
            var y = IsYearsInPerformance ?? dfx;
            if (y(Contract))
            {
                AddReasonEntry($"{PREFIX} cannot be performed within one YEAR of " +
                               "the contract's date");
                return true;
            }
            var l = IsLandRelated ?? dfx;
            if (l(Contract))
            {
                AddReasonEntry($"{PREFIX} involving the sale of LAND or interest therein");
                return true;
            }
            var e = IsExecutorsPersonalResources ?? dfx;
            if (e(Contract))
            {
                AddReasonEntry($"{PREFIX} EXECUTORS to pay debts out of the executors' " +
                               "own pockets");
                return true;
            }
            var g = IsGoodsAbove500Usd ?? dfx;
            if (g(Contract))
            {
                AddReasonEntry($"{PREFIX} sales of GOODS above $500");
                return true;
            }
            var s = IsSuretyshipRelated ?? dfx;
            if (s(Contract))
            {
                AddReasonEntry($"{PREFIX} SURETYSHIP, one party agrees to be liable " +
                               "for the debts of someone else");
                return true;
            }

            return false;
        }

        /// <summary>
        /// <![CDATA[See Restatement (Second) of Contracts § 124]]>
        /// </summary>
        public Predicate<IContract<T>> IsMarriageRelated { get; set; }

        /// <summary>
        /// <![CDATA[See Restatement (Second) of Contracts § 130]]>
        /// </summary>
        public Predicate<IContract<T>> IsYearsInPerformance { get; set; }

        /// <summary>
        /// <![CDATA[See Restatement (Second) of Contracts § 125-129]]>
        /// </summary>
        public Predicate<IContract<T>> IsLandRelated { get; set; }

        /// <summary>
        /// <![CDATA[See Restatement (Second) of Contracts §§ 111]]>
        /// </summary>
        public Predicate<IContract<T>> IsExecutorsPersonalResources { get; set; }

        /// <summary>
        /// <![CDATA[See Uniform Commercial Code § 2-201]]>
        /// </summary>
        public Predicate<IContract<T>> IsGoodsAbove500Usd { get; set; }

        /// <summary>
        /// <![CDATA[See Restatement (Second) of Contracts §§ 112-123]]>
        /// </summary>
        [Aka("co-signer")]
        public Predicate<IContract<T>> IsSuretyshipRelated { get; set; }
    }
}
