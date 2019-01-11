using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Law.Attributes;

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

        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            var intent2Contrx = IsApprovalExpressed ?? (lp => true);

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

            return true;
        }

        public override ISet<Term<object>> GetAgreedTerms(ILegalPerson offeror, ILegalPerson offeree)
        {
            var agreedTerms = base.GetAgreedTerms(offeror, offeree);

            var offerorUqTerms = TermsOfAgreement(offeror).Where(t => agreedTerms.All(tt => !tt.EqualRefersTo(t))).ToList();
            var offereeUqTerms = TermsOfAgreement(offeree).Where(t => agreedTerms.All(tt => !tt.EqualRefersTo(t))).ToList();

            if (!offereeUqTerms.Any() && !offerorUqTerms.Any())
                return agreedTerms;

            var isEitherExpresslyCond = offereeUqTerms.Any(t => TermExpresslyConditional.Value.Equals(t)) ||
                                        offerorUqTerms.Any(t => TermExpresslyConditional.Value.Equals(t));

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
