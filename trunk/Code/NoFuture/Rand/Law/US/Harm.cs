using System;
using System.Linq;

namespace NoFuture.Rand.Law.US
{
    /// <summary>
    /// The legal idea of some kind of loss suffered by a <see cref="ILegalPerson"/>
    /// </summary>
    /// <remarks>https://en.wikipedia.org/wiki/Harm</remarks>
    public class Harm: UnoHomine, ITermCategory, IInjury
    {
        private readonly ITermCategory _termCategory = new TermCategory(nameof(Harm));
        public Harm() : this(ExtensionMethods.Defendant) { }
        public Harm(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        public Predicate<ILegalPerson> IsPain { get; set; } = lp => false;

        public Predicate<ILegalPerson> IsDeath { get; set; } = lp => false;

        /// <summary>
        /// significantly limit your ability to do basic work such as lifting,
        /// standing, walking, sitting, and remembering – for at least 12 months
        /// </summary>
        /// <remarks>
        /// https://www.ssa.gov/planners/disability/qualify.html#anchor3
        /// </remarks>
        public Predicate<ILegalPerson> IsDisability { get; set; } = lp => false;

        public Predicate<ILegalPerson> IsOfFreedomLost { get; set; } = lp => false;

        public Predicate<ILegalPerson> IsOfPleasureLost { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var person = GetSubjectPerson(persons);
            if (person == null)
                return false;

            var pain = IsPain(person);
            var death = IsDeath(person);
            var disability = IsDisability(person);
            var freedom = IsOfFreedomLost(person);
            var pleasure = IsOfPleasureLost(person);

            if (new[] {pain, death, disability, freedom, pleasure}.All(p => p == false))
            {
                AddReasonEntry($"person, {person.Name}, {nameof(IsPain)}, {nameof(IsDeath)}, {nameof(IsDisability)}" +
                               $", {nameof(IsOfFreedomLost)}, {nameof(IsOfPleasureLost)} are all false");
            }

            return true;
        }

        public virtual int GetRank()
        {
            return _termCategory.GetRank();
        }

        public virtual string GetCategory()
        {
            return _termCategory.GetCategory();
        }

        public virtual bool IsCategory(ITermCategory category)
        {
            return _termCategory.IsCategory(category);
        }

        public virtual ITermCategory As(ITermCategory category)
        {
            return _termCategory.As(category);
        }

        public virtual bool IsCategory(Type category)
        {
            return _termCategory.IsCategory(category);
        }
    }
}
