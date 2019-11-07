using System;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Procedure.Civil.US.Jurisdiction
{
    /// <summary>
    /// Defined in 28 U.S.C. section 1391
    /// </summary>
    public class FederalVenue : PersonalJurisdiction, IVenue
    {
        public FederalVenue(ICourt name) : base(name)
        {
        }

        /// <summary>
        /// 28 U.S.C. §1391(b)(1)
        /// The location of the defendant(s)
        /// </summary>
        protected internal virtual bool IsValidUscB1(ILegalPerson[] persons)
        {
            var defendant = this.Defendant(persons);
            if (defendant == null)
                return false;
            return IsCourtDomicileLocationOfDefendant(defendant);
        }

        /// <summary>
        /// 28 U.S.C. §1391(b)(2)
        /// The location of the injury\cause-of-action
        /// </summary>
        protected internal virtual bool IsValidUscB2(ILegalPerson[] persons)
        {
            var plaintiff = this.Plaintiff(persons);
            if (plaintiff == null)
                return false;

            return IsCourtInjuryLocationOfPlaintiff(plaintiff);
        }

        /// <summary>
        /// 28 U.S.C. §1391(b)(3)
        /// The location(s) which have personal jurisdiction over the defendant(s)
        /// </summary>
        protected internal virtual bool IsValidUscB3(ILegalPerson[] persons)
        {
            return base.IsValid(persons);
        }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            if (!IsCourtAssigned())
                return false;

            if (!IsFederalCourt())
                return false;

            if (IsValidUscB1(persons))
                return true;

            if (IsValidUscB2(persons))
                return true;

            if (IsValidUscB3(persons))
                return true;

            return false;
        }
    }
}
