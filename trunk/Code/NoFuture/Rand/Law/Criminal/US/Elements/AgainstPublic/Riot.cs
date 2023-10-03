using System;
using NoFuture.Law.US;

namespace NoFuture.Law.Criminal.US.Elements.AgainstPublic
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
            var defendant = this.Defendant(persons);
            if (defendant == null)
                return false;
            var title = defendant.GetLegalPersonTypeName();
            var violent = IsByViolence(defendant);
            var threat = IsByThreatOfViolence(defendant);

            if (!violent && !threat)
            {
                AddReasonEntry($"{title} {defendant.Name}, {nameof(IsByViolence)} is false");
                AddReasonEntry($"{title} {defendant.Name}, {nameof(IsByThreatOfViolence)} is false");
            }

            return base.IsValid(persons);
        }

    }
}
