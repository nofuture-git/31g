using System;
using System.Linq;

namespace NoFuture.Rand.Law.Criminal.US.Elements
{
    /// <summary>
    /// The legal idea of some kind of loss suffered
    /// </summary>
    /// <remarks>https://en.wikipedia.org/wiki/Harm</remarks>
    public class Harm: UnoHomine
    {
        public Harm(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        public Predicate<ILegalPerson> IsPain { get; set; } = lp => false;

        public Predicate<ILegalPerson> IsDeath { get; set; } = lp => false;

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
    }
}
