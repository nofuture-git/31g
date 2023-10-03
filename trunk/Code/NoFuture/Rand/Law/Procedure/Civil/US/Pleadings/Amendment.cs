using System;
using NoFuture.Law.Attributes;
using NoFuture.Law.Procedure.Civil.US.ServiceOfProcess;
using NoFuture.Law.US;

namespace NoFuture.Law.Procedure.Civil.US.Pleadings
{
    public class Amendment : PleadingBase, ILinkedLegalConcept
    {
        /// <summary>
        /// Allows caller to specify some past or future date, default is current system UTC time
        /// </summary>
        public DateTime? CurrentTime { get; set; }

        /// <summary>
        /// What the this amendment is being amended to
        /// </summary>
        public ILegalConcept LinkedTo { get; set; }

        /// <summary>
        /// This is required from court or opposing party after the <see cref="AllowedTimeSpanAfterServing"/> as ended
        /// </summary>
        [Aka("obtained permission")]
        public IAssent Assent { get; set; }

        /// <summary>
        /// The service-of-process based on each person
        /// </summary>
        public Func<ILegalPerson, IProcessService> GetServiceOfProcess { get; set; } = lp => null;

        /// <summary>
        /// The number of days after serving, &quot;as a matter of course&quot;, in which an amendment may be filed 
        /// </summary>
        public Func<ILegalPerson, TimeSpan> AllowedTimeSpanAfterServing { get; set; } = lp => new TimeSpan(21,0,0,0);

        public override bool IsValid(params ILegalPerson[] persons)
        {
            if (!IsCourtAssigned())
                return false;

            if (!this.TryGetSubjectPerson(persons, out var subjectPerson))
                return false;

            return IsValidWithLeaveOfCourt(persons, subjectPerson) || IsValidAsMatterOfCourse(persons, subjectPerson);
        }

        /// <summary>
        /// One of two ways in which an amendment is valid, depends on being
        /// filed in some limited timespan after service of process
        /// </summary>
        protected internal virtual bool IsValidAsMatterOfCourse(ILegalPerson[] persons, ILegalPerson subjectPerson)
        {
            var title = subjectPerson.GetLegalPersonTypeName();

            var serviceOfProcess = GetServiceOfProcess(subjectPerson);
            if (serviceOfProcess == null)
            {
                AddReasonEntry($"{title} {subjectPerson.Name}, {nameof(GetServiceOfProcess)} returned nothing");
                return false;
            }

            if (!ProcessServiceBase.IsDateOfServiceValid(serviceOfProcess, subjectPerson, out var serviceDate))
            {
                AddReasonEntryRange(serviceOfProcess.GetReasonEntries());
                return false;
            }

            var currentTime = CurrentTime ?? DateTime.UtcNow;

            var allowedTimeSpan = AllowedTimeSpanAfterServing(subjectPerson);
            var actualTimeSpan = currentTime - serviceDate;

            if (actualTimeSpan > allowedTimeSpan)
            {
                AddReasonEntry($"{title} {subjectPerson.Name}, {nameof(AllowedTimeSpanAfterServing)} " +
                               $"is {allowedTimeSpan} is less-than actual of {actualTimeSpan}");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Two of two ways in which an amendment is valid, requires the court&apos;s permission or opposition&apos;s consent
        /// </summary>
        /// <param name="persons"></param>
        /// <param name="subjectPerson"></param>
        /// <returns></returns>
        protected internal virtual bool IsValidWithLeaveOfCourt(ILegalPerson[] persons, ILegalPerson subjectPerson)
        {
            if (Assent == null)
                return false;

            var courtOfficial = LinkedLegalConceptExtensions.GetPersonsLessThisOne(this, persons, subjectPerson).CourtOfficial();

            if (Assent.IsApprovalExpressed(courtOfficial))
                return true;

            if (!this.TryGetOppositionPerson(persons, subjectPerson, out var opposition))
            {
                return false;
            }

            return Assent.IsApprovalExpressed(opposition);
        }
    }
}
