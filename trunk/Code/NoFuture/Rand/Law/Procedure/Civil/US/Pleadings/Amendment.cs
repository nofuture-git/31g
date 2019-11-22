using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.Procedure.Civil.US.ServiceOfProcess;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Persons;

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

            if (!TryGetSubjectPerson(persons, out var subjectPerson))
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

            var courtOfficial = GetPersonsLessThisOne(persons, subjectPerson).CourtOfficial();

            if (Assent.IsApprovalExpressed(courtOfficial))
                return true;

            if (!TryGetOppositionPerson(persons, subjectPerson, out var opposition))
            {
                return false;
            }

            return Assent.IsApprovalExpressed(opposition);
        }

        protected internal bool TryGetSubjectPerson(ILegalPerson[] persons, out ILegalPerson person)
        {
            person = null;

            if (persons == null || !persons.Any())
                return false;

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

        protected internal bool TryGetOppositionPerson(ILegalPerson[] persons, ILegalPerson subjectPerson,
            out ILegalPerson person)
        {
            person = null;
            if (subjectPerson == null)
                return false;

            var personsLessSubj = GetPersonsLessThisOne(persons, subjectPerson);
            if (personsLessSubj == null || !personsLessSubj.Any())
                return false;

            if (subjectPerson is IPlaintiff)
                person = this.Defendant(personsLessSubj);
            if (subjectPerson is IDefendant)
                person = this.Plaintiff(personsLessSubj);

            return person != null;
        }

        private IList<ILegalPerson> GetPersonsLessThisOne(ILegalPerson[] persons, ILegalPerson exceptThisGuy)
        {
            var outPersons = new List<ILegalPerson>();

            if (persons == null || !persons.Any())
                return outPersons;

            if (exceptThisGuy == null)
                return persons.ToList();

            foreach (var person in persons)
            {
                if (NamesEqual(person, exceptThisGuy))
                    continue;
                if (ReferenceEquals(person, exceptThisGuy))
                    continue;
                outPersons.Add(person);
            }

            return outPersons;
        }

        private ILegalPerson GetCourtOfficialLessThisOne(ILegalPerson[] persons, ILegalPerson exceptThisGuy)
        {
            if (persons == null || !persons.Any())
                return null;


            throw new NotImplementedException();
        }

    }
}
