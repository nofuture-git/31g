using System;
using NoFuture.Rand.Law.US.Criminal.Terms;

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
            Proportionality = new Proportionality<Force>(crime);
        }

        public Provacation Provacation { get; set; }

        public Imminence Imminence { get; set; }

        public Proportionality<Force> Proportionality { get; set; }

        public override bool IsValid(ILegalPerson offeror = null, ILegalPerson offeree = null)
        {
            var defendant = Government.GetDefendant(offeror, offeree, this);
            if (defendant == null)
                return false;
            if (Imminence != null && !Imminence.IsValid(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(Imminence)} is false");
                AddReasonEntryRange(Imminence.GetReasonEntries());
                return false;
            }
            if (Provacation != null && !Provacation.IsValid(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(Provacation)} is false");
                AddReasonEntryRange(Provacation.GetReasonEntries());
                return false;
            }
            if (Proportionality != null && !Proportionality.IsValid(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(Proportionality)} is false");
                AddReasonEntryRange(Proportionality.GetReasonEntries());
                return false;
            }

            return true;
        }
    }
}
