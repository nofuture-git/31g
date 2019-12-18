using System;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Persons;

namespace NoFuture.Rand.Law.Criminal.US.Defense.Excuse
{
    /// <summary>
    /// when the whole of the criminal intent originated with government 
    /// </summary>
    public class Entrapment : DefenseBase
    {
        public ICrime Crime { get; }

        public Entrapment(ICrime crime) : base(ExtensionMethods.Defendant)
        {
            Crime = crime;
        }

        /// <summary>
        /// gets the person who originated the criminal intent 
        /// </summary>
        public Func<IMensRea, ILegalPerson> GetOriginatorOfIntent { get; set; } = mr => null;

        /// <summary>
        /// tests if defendant has a history or predisposition to this particular criminal intent
        /// </summary>
        public Predicate<ILegalPerson> IsPredisposedToParticularIntent { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var legalPerson = this.Defendant(persons);
            if (legalPerson == null)
                return false;

            var title = legalPerson.GetLegalPersonTypeName();

            if (Crime == null)
            {
                AddReasonEntry($"the {nameof(Entrapment)} {nameof(Crime)} is unassigned");
                return false;
            }

            var mensrea = Crime?.Concurrence?.MensRea;
            if (mensrea == null)
            {
                AddReasonEntry("there is no actus reus for the given crime");
                return false;
            }

            var originator = GetOriginatorOfIntent(mensrea);

            if (originator == null)
            {
                AddReasonEntry($"{title} {legalPerson.Name}, " +
                               $"{nameof(GetOriginatorOfIntent)} returned nothing");
                return false;
            }

            var isGovtOriginator = originator is IGovernment;

            if (!isGovtOriginator)
            {
                AddReasonEntry($"{title} {legalPerson.Name}, " +
                               $"{nameof(GetOriginatorOfIntent)} is not of type {nameof(IGovernment)}");
                return false;
            }

            if (IsPredisposedToParticularIntent(legalPerson))
            {
                AddReasonEntry($"{title}, {legalPerson.Name}, " +
                               $"{nameof(IsPredisposedToParticularIntent)} is true");
                return false;
            }
            
            return true;
        }
    }
}
