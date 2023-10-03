using System.Collections.Generic;
using System.Linq;
using NoFuture.Law.Procedure.Civil.US.Pleadings;
using NoFuture.Law.US;
using NoFuture.Law.US.Persons;

namespace NoFuture.Law.Procedure.Civil.US
{
    public static class LinkedLegalConceptExtensions
    {
        public static bool TryGetSubjectPerson(this ILinkedLegalConcept llc, ILegalPerson[] persons, out ILegalPerson person)
        {
            person = null;

            if (persons == null || !persons.Any() || llc == null)
                return false;

            if (llc.LinkedTo == null)
            {
                llc.AddReasonEntry($"{nameof(llc.LinkedTo)} is unassigned");
                return false;
            }

            if (llc.LinkedTo is Complaint)
                person = llc.Plaintiff(persons);

            if (llc.LinkedTo is Answer)
                person = llc.Defendant(persons);

            if (person == null)
            {
                llc.AddReasonEntry($"{nameof(llc.LinkedTo)} is neither type {nameof(Complaint)} nor {nameof(Answer)}");
                return false;
            }

            return true;
        }

        public static bool TryGetOppositionPerson(this ILinkedLegalConcept llc, ILegalPerson[] persons, ILegalPerson subjectPerson,
            out ILegalPerson person)
        {
            person = null;
            if (subjectPerson == null || llc == null)
                return false;

            var personsLessSubj = GetPersonsLessThisOne(llc, persons, subjectPerson);
            if (personsLessSubj == null || !personsLessSubj.Any())
                return false;

            if (subjectPerson is IPlaintiff)
                person = llc.Defendant(personsLessSubj);
            if (subjectPerson is IDefendant)
                person = llc.Plaintiff(personsLessSubj);

            return person != null;
        }


        internal static IList<ILegalPerson> GetPersonsLessThisOne(ILinkedLegalConcept llc, ILegalPerson[] persons, ILegalPerson exceptThisGuy)
        {
            var outPersons = new List<ILegalPerson>();

            if (persons == null || !persons.Any() || llc == null)
                return outPersons;

            if (exceptThisGuy == null)
                return persons.ToList();

            foreach (var person in persons)
            {
                if (llc.NamesEqual(person, exceptThisGuy))
                    continue;
                if (ReferenceEquals(person, exceptThisGuy))
                    continue;
                outPersons.Add(person);
            }

            return outPersons;
        }
    }
}
