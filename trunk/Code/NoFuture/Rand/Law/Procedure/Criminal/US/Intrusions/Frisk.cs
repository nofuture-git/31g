using System;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Procedure.Criminal.US
{
    /// <summary>
    /// An encounter with law-enforcement in which a person is detained, questioned and frisked (pat-down)
    /// </summary>
    public class Frisk : SuspectStop
    {
        public Func<ILegalPerson, ILegalPerson, bool> IsBeliefArmedAndDangerous { get; set; } = (lp0, lp1) => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            if (GetSuspect == null)
                GetSuspect = lps => lps.Suspect();

            if (GetLawEnforcement == null)
                GetLawEnforcement = lps => lps.LawEnforcement();

            var suspect = GetSuspect(persons);
            if (suspect == null)
            {
                AddReasonEntry($"{nameof(GetSuspect)} returned nothing");
                return false;
            }

            var officer = GetLawEnforcement(persons);
            if (officer == null)
            {
                AddReasonEntry($"{nameof(GetLawEnforcement)} returned nothing");
                return false;
            }

            var suspectTitle = suspect.GetLegalPersonTypeName();
            var officerTitle = officer.GetLegalPersonTypeName();

            if (!IsBeliefArmedAndDangerous(officer, suspect) && !IsBeliefArmedAndDangerous(suspect, officer))
            {
                AddReasonEntry($"{officerTitle} {officer.Name}, {nameof(IsBeliefArmedAndDangerous)} " +
                               $"is false for {suspectTitle} {suspect.Name}");
                return false;
            }

            return base.IsValid(persons);
        }
    }
}
