using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.US.Contracts.Terms;

namespace NoFuture.Rand.Law.US.Contracts.Ucc
{
    /// <inheritdoc cref="MutualAssent"/>
    /// <inheritdoc cref="IAssent"/>
    /// <summary>
    /// <![CDATA[
    /// the bargain of the parties in fact, as found in their 
    /// language or inferred from other circumstances, including 
    /// course of performance, course of dealing, or usage of trade
    /// ]]>
    /// </summary>
    public class Agreement : MutualAssent
    {
        public override bool IsEnforceableInCourt => true;

        /// <summary>
        /// Additional Terms in Acceptance or Confirmation.
        /// </summary>
        [Aka("UCC 2-207")]
        public override Func<ILegalPerson, ISet<Term<object>>> TermsOfAgreement { get; set; }

        /// <summary>
        /// One of various ways a buyer accepts goods accocding the the UCC
        /// </summary>
        [Aka("UCC 2-606(1)(a)")]
        public Predicate<ILegalPerson> IsGoodsInspected { get; set; } = lp => true;

        /// <summary>
        /// One of various ways a buyer accepts accocding the the UCC
        /// </summary>
        [Aka("UCC 2-606(1)(b)")]
        public Predicate<ILegalPerson> IsRejected { get; set; } = lp => false;

        /// <summary>
        /// One of various ways a buyer accepts accocding the the UCC
        /// </summary>
        [Aka("UCC 2-606(1)(c)")]
        public Predicate<ILegalPerson> IsAnyActAsOwner { get; set; } = lp => true;

        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            var intent2Contrx = IsApprovalExpressed ?? (lp => true);

            Predicate<ILegalPerson> buyerAcceptsGoods =
                lp => IsGoodsInspected(lp) || !IsRejected(lp) || IsAnyActAsOwner(lp);

            if (!intent2Contrx(offeror))
            {
                AddReasonEntry($"{offeror?.Name} did not intend this " +
                              "agreement as a binding contract.");
                return false;
            }

            if (!intent2Contrx(offeree))
            {
                AddReasonEntry($"{offeree?.Name} did not intend this " +
                              "agreement as a binding contract.");
                return false;
            }

            if (!buyerAcceptsGoods(offeree))
            {
                AddReasonEntry($"the buyer, {offeree?.Name}, rejected the goods");
            }

            return true;
        }

        public override ISet<Term<object>> GetAgreedTerms(ILegalPerson offeror, ILegalPerson offeree)
        {
            var agreedTerms = base.GetAgreedTerms(offeror, offeree);

            var offerorUqTerms = TermsOfAgreement(offeror).Where(t => agreedTerms.All(tt => !tt.EqualRefersTo(t))).ToList();
            var offereeUqTerms = TermsOfAgreement(offeree).Where(t => agreedTerms.All(tt => !tt.EqualRefersTo(t))).ToList();

            if (!offereeUqTerms.Any() && !offerorUqTerms.Any())
                return agreedTerms;

            var isEitherExpresslyCond = offereeUqTerms.Any(t => ExpresslyConditionalTerm.Value.Equals(t)) ||
                                        offerorUqTerms.Any(t => ExpresslyConditionalTerm.Value.Equals(t));

            //terms of the same name but different meanings
            var knockoutTerms = offerorUqTerms
                .Where(t => offereeUqTerms.Any(tt => t.Equals(tt) && !t.EqualRefersTo(tt))).ToList();

            if (knockoutTerms.Any())
            {
                foreach (var kn in knockoutTerms)
                {
                    AddReasonEntry($"the term-name '{kn.Name}' is used by both {offeror.Name} " +
                                   $"and {offeree.Name} with different semantic meanings, " +
                                   "so this term is 'knocked out'" );
                }
                offerorUqTerms = offerorUqTerms.Where(t => knockoutTerms.Any(tt => !tt.Equals(t))).ToList();
                offereeUqTerms = offereeUqTerms.Where(t => knockoutTerms.Any(tt => !tt.Equals(t))).ToList();
            }

            //per UCC 2-207(1) additional terms become part of the contract when not expressly forbidden
            if (!isEitherExpresslyCond)
            {
                foreach (var offerorUqTerm in offerorUqTerms)
                    agreedTerms.Add(offerorUqTerm);

                foreach (var offereeUqTerm in offereeUqTerms)
                    agreedTerms.Add(offereeUqTerm);
            }
            return agreedTerms;
        }
    }
}
