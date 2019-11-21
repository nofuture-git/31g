using System;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.Procedure.Civil.US.ServiceOfProcess;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Procedure.Civil.US.Pleadings
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

            if (!TryGetSubjectPerson(persons, out var subjectPerson))
                return false;

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

        protected internal bool TryGetSubjectPerson(ILegalPerson[] persons, out ILegalPerson person)
        {
            person = null;

            if (LinkedTo == null)
            {
                AddReasonEntry($"{nameof(LinkedTo)} is unassigned");
                return false;
            }

            if (LinkedTo is Complaint)
                person = this.Plaintiff(persons);

            if (LinkedTo is Answer)
                person = this.Defendant(persons);

            if (person == null)
            {
                AddReasonEntry($"{nameof(LinkedTo)} is neither type {nameof(Complaint)} nor {nameof(Answer)}");
                return false;
            }

            return true;
        }

    }
}
