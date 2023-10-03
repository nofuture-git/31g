using System;
using System.Collections.Generic;

namespace NoFuture.Law
{
    public interface IAssentTerms : IIntent
    {
        Func<ILegalPerson, ISet<Term<object>>> TermsOfAgreement { get; set; }

        ISet<Term<object>> GetAgreedTerms(ILegalPerson offeror, ILegalPerson offeree);

        ISet<Term<object>> GetAdditionalTerms(ILegalPerson offeror, ILegalPerson offeree);

        ISet<Term<object>> GetInNameAgreedTerms(ILegalPerson offeror, ILegalPerson offeree);
    }
}