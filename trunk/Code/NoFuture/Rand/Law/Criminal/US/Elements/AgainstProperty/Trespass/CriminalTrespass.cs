using NoFuture.Rand.Law.Criminal.US.Elements.Intent.ComLaw;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.PenalCode;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Criminal.US.Elements.AgainstProperty.Trespass
{
    public class CriminalTrespass : TrespassBase, IActusReus
    {
        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = persons.Defendant();
            if (defendant == null)
                return false;

            if (!WithoutConsent(persons))
                return false;

            if (!IsTangibleEntry(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsTangibleEntry)} is false");
                return false;
            }

            return true;
        }

        public virtual bool CompareTo(IMensRea criminalIntent, params ILegalPerson[] persons)
        {
            var validIntend = criminalIntent is Knowingly || criminalIntent is GeneralIntent;

            if (!validIntend)
            {
                AddReasonEntry($"{nameof(CriminalTrespass)} requires intent " +
                               $"{nameof(Knowingly)}, {nameof(GeneralIntent)}");
                return false;
            }

            return true;
        }
    }
}
