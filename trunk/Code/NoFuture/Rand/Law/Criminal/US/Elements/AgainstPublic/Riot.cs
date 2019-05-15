using System;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Criminal.US.Elements.AgainstPublic
{
    /// <summary>
    /// Where <see cref="UnlawfulAssembly"/> is <see cref="DisorderlyConduct"/>
    /// in a group - this is additive in that it includes violence 
    /// </summary>
    public class Riot : UnlawfulAssembly, IBattery, IAssault, IElement
    {
        public Predicate<ILegalPerson> IsByViolence { get; set; } = lp => false;
        public Predicate<ILegalPerson> IsByThreatOfViolence { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = persons.Defendant();
            if (defendant == null)
                return false;

            var violent = IsByViolence(defendant);
            var threat = IsByThreatOfViolence(defendant);

            if (!violent && !threat)
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsByViolence)} is false");
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsByThreatOfViolence)} is false");
            }

            return base.IsValid(persons);
        }

    }
}
