using System;

namespace NoFuture.Rand.Law.US.Criminal.Defense.Justification
{
    /// <summary>
    /// <![CDATA[
    /// The Model Penal Code defines self-defense in § 3.04(1) as "justifiable when the actor 
    /// believes that such force is immediately necessary for the purpose of protecting himself 
    /// against the use of unlawful force by such other person on the present occasion."
    /// ]]>
    /// </summary>
    public class DefenseOfSelf : DefenseBase
    {
        public DefenseOfSelf(ICrime crime) : base(crime)
        {
            Provacation = new Provacation(crime);
            Imminence = new Imminence(crime);
        }

        public Provacation Provacation { get; set; }

        public Imminence Imminence { get; set; }

        public ObjectivePredicate<ILegalPerson> IsDegreeOfForceReasonable { get; set; } = lp => false;

        public override bool IsValid(ILegalPerson offeror = null, ILegalPerson offeree = null)
        {
            var defendant = Government.GetDefendant(offeror, offeree, this);
            if (defendant == null)
                return false;

            if (Provacation != null && !Provacation.IsValid(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(Provacation)} is false");
                AddReasonEntryRange(Provacation.GetReasonEntries());
                return false;
            }
            if (Imminence != null && !Imminence.IsValid(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(Imminence)} is false");
                return false;
            }
            if (!IsDegreeOfForceReasonable(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsDegreeOfForceReasonable)} is false");
                return false;
            }

            return true;
        }
    }
}
