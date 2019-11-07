using System;

namespace NoFuture.Rand.Law.Procedure.Civil.US.Jurisdiction
{
    /// <summary>
    /// Defined in 28 U.S.C. section 1391
    /// </summary>
    public class FederalVenue : Venue
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
            throw new NotImplementedException();
        }

        /// <summary>
        /// 28 U.S.C. §1391(b)(2)
        /// The location of the injury\cause-of-action
        /// </summary>
        protected internal virtual bool IsValidUscB2(ILegalPerson[] persons)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 28 U.S.C. §1391(b)(3)
        /// The location(s) which have personal jurisdiction over the defendant(s)
        /// </summary>
        protected internal virtual bool IsValidUscB3(ILegalPerson[] persons)
        {
            throw new NotImplementedException();
        }


        public override bool IsValid(params ILegalPerson[] persons)
        {
            if (!IsCourtAssigned())
                return false;

            if (!IsFederalCourt())
                return false;

            if (!IsValidUscB1(persons))
                return false;


            

            throw new NotImplementedException();
        }
    }
}
