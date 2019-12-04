using System;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Persons;

namespace NoFuture.Rand.Law.Procedure.Criminal.US
{
    /// <summary>
    /// An encounter with law-enforcement in which a person is detained and questioned
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// With motor vehicle stops: driver and passengers may be ordered out of vehicle.
    /// In addition, so long as the stop's required investigative time is not prolonged:
    /// immigration status may be checked, a dog-sniff of the exterior is allowed, limited
    /// interior search is allowed for armed-and-dangerous suspects.
    /// ]]>
    /// </remarks>
    [Aka("detained")]
    public class SuspectStop : LegalConcept, IIntrusion
    {
        /// <summary>
        /// Resolves who is the suspect
        /// </summary>
        public Func<ILegalPerson[], ILegalPerson> GetSuspect { get; set; }

        /// <summary>
        /// Resolves who is the law-enforcement agent
        /// </summary>
        public Func<ILegalPerson[], ILegalPerson> GetLawEnforcement { get; set; }

        /// <summary>
        /// When the <see cref="ISuspect"/> no longer believes they are free to leave and are being detained.
        /// </summary>
        public Predicate<ILegalPerson> IsBeliefFreeToGo { get; set; } = lp => false;

        /// <summary>
        /// The timespan for which a suspect was actually detained
        /// </summary>
        public Func<ILegalPerson, TimeSpan?> GetDetainedTimespan { get; set; } = lp => null;

        /// <summary>
        /// The timespan required to effectuate the original purpose
        /// of the stop (i.e. confirm or dispel original suspicion)
        /// </summary>
        public Func<ILegalPerson, TimeSpan?> GetRequiredInvestigateTimespan { get; set; } = lp => null;

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

            if (IsBeliefFreeToGo(suspect))
            {
                AddReasonEntry($"{suspectTitle} {suspect.Name}, {nameof(IsBeliefFreeToGo)} is true");
                return false;
            }

            var detainedTs = GetDetainedTimespan(suspect) ?? GetDetainedTimespan(officer);
            if (detainedTs == null)
            {
                AddReasonEntry($"{suspectTitle} {suspect.Name}, {nameof(GetDetainedTimespan)} returned nothing");
                return false;
            }

            var requiredTs = GetRequiredInvestigateTimespan(officer);
            if (requiredTs == null)
            {
                AddReasonEntry($"{officerTitle} {officer.Name}, {nameof(GetRequiredInvestigateTimespan)} returned nothing");
                return false;
            }

            if (detainedTs.Value > requiredTs.Value)
            {
                AddReasonEntry($"{officerTitle} {officer.Name}, {nameof(GetRequiredInvestigateTimespan)} is {requiredTs}");
                AddReasonEntry($"{suspectTitle} {suspect.Name}, {nameof(GetDetainedTimespan)} is {detainedTs}");
                AddReasonEntry($"{detainedTs} is greater than {requiredTs}");
                return false;
            }

            return true;
        }
    }
}
