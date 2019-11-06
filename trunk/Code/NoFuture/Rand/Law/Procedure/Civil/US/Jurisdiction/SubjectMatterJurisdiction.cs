using System;

namespace NoFuture.Rand.Law.Procedure.Civil.US.Jurisdiction
{
    /// <summary>
    /// Different kinds of courts hear different kinds of cases
    /// </summary>
    public class SubjectMatterJurisdiction : JurisdictionBase
    {
        public SubjectMatterJurisdiction(ICourt name) : base(name)
        {
        }

        /// <summary>
        /// When either to establish a claim or present a defense relies on federal law.
        /// </summary>
        public Predicate<ILegalConcept> IsArisingFromFederalLaw { get; set; }

        /// <summary>
        /// Congress must convey jurisdiction in a federal statute
        /// </summary>
        public Predicate<ILegalConcept> IsCongressConveyedJurisdiction { get; set; }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            throw new NotImplementedException();
        }

    }
}
