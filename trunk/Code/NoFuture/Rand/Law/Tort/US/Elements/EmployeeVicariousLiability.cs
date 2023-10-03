using System;
using System.Linq;
using NoFuture.Law.Attributes;
using NoFuture.Law.US;
using NoFuture.Law.US.Persons;

namespace NoFuture.Law.Tort.US.Elements
{
    /// <summary>
    /// Given a employee-employer relationship, an employer may be held liable for the acts of its employees
    /// </summary>
    public class EmployeeVicariousLiability : AdiunctusLiability
    {
        public EmployeeVicariousLiability(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        /// <summary>
        /// Defines the employee-employer relationship is beneficial to both.
        /// </summary>
        [Aka("is employee-employer")]
        public override Func<ILegalPerson, ILegalPerson, bool> IsMutuallyBeneficialRelationship { get; set; } =
            (lp1, lp2) => false;

        public Predicate<ILegalPerson> IsActInScopeOfEmployment { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var subj = GetSubjectPerson(persons);
            if (subj == null)
            {
                AddReasonEntry($"{nameof(GetSubjectPerson)} returned nothing");
                return false;
            }
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

            if (!IsMutuallyBeneficialRelationship(employer, employee))
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
