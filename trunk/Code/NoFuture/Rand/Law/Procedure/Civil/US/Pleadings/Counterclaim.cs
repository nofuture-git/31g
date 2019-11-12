using System;

namespace NoFuture.Rand.Law.Procedure.Civil.US.Pleadings
{
    /// <summary>
    /// Complaint in opposition to some other plaintiff&apos;s
    /// complaint both of which concern the same set of facts\circumstances.
    /// </summary>
    public class Counterclaim : Complaint
    {
        public Counterclaim(PleadingBase responseTo)
        {
            if(responseTo == null)
                throw new ArgumentNullException(nameof(responseTo));

            ResponseTo = responseTo;
        }

        /// <summary>
        /// The source which makes this a counter-claim instead of just a stand-alone <see cref="Complaint"/>
        /// </summary>
        /// <remarks>
        /// The underlying <see cref="CivilProcedureBase.CausesOfAction"/> should equal each other
        /// </remarks>
        public PleadingBase ResponseTo { get; }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            //a counter claim must arise from the same set of facts
            if (!ResponseTo.CausesOfAction.Equals(CausesOfAction))
            {
                AddReasonEntry($"this {nameof(CausesOfAction)} does not equal the {nameof(ResponseTo)} {nameof(CausesOfAction)}");
                return false;
            }

            return base.IsValid(persons);
        }
    }
}
