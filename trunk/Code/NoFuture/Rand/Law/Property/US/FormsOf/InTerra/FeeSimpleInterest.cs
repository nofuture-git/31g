using System;

namespace NoFuture.Rand.Law.Property.US.FormsOf.InTerra
{
    /// <summary>
    /// The simplest form of property interest - a kind of default which is assumed unless otherwise stated
    /// </summary>
    public class FeeSimpleInterest : PropertyBase, ILandPropertyInterest
    {
        public FeeSimpleInterest(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        public new RealProperty SubjectProperty { get; set; }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            throw new NotImplementedException();
        }

    }
}
