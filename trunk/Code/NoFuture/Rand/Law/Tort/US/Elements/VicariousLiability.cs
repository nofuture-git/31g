using System;
using System.Linq;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Persons;

namespace NoFuture.Rand.Law.Tort.US.Elements
{
    /// <summary>
    /// Given a employee-employer relationship, an employer may be held liable for the acts of its employees
    /// </summary>
    public class VicariousLiability : UnoHomine, IAct
    {
        public VicariousLiability(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        public Func<IEmployer, IEmployee, bool> IsEmployment { get; set; }

        public Predicate<ILegalPerson> IsActInScopeOfEmployment { get; set; }

        public Predicate<ILegalPerson> IsVoluntary { get; set; }

        public Predicate<ILegalPerson> IsAction { get; set; }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var subj = GetSubjectPerson(persons);
            if (subj == null)
                return false;
            var title = subj.GetLegalPersonTypeName();

            var employer = this.Employer(persons);
            if (employer == null)
                return false;

            var employees = this.Employees(persons).Cast<IEmployee>().ToList();
            if (!employees.Any())
                return false;

            var employee = employees.FirstOrDefault(e => e.IsSamePerson(subj));
            if (employee == null)
            {
                AddReasonEntry($"{title} {subj.Name}, is not among the {nameof(IEmployee)} in {employees.GetTitleNamePairs()}");
                return false;
            }

            if (!IsEmployment(employer, employee))
            {
                AddReasonEntry($"{title} {subj.Name}, is not employed by {employer.GetLegalPersonTypeName()} {employer.Name}");
                return false;
            }

            if (!IsActInScopeOfEmployment(subj))
            {
                AddReasonEntry($"{title} {subj.Name}, {nameof(IsActInScopeOfEmployment)} is false");
                return false;
            }

            if (!IsVoluntary(subj))
            {
                AddReasonEntry($"{title} {subj.Name}, {nameof(IsVoluntary)} is false");
                return false;
            }

            if (!IsAction(subj))
            {
                AddReasonEntry($"{title} {subj.Name}, {nameof(IsAction)} is false");
                return false;
            }

            return true;
        }

    }
}
