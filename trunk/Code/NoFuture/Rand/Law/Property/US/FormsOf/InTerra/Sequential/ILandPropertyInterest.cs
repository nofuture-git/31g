using System;

namespace NoFuture.Rand.Law.Property.US.FormsOf.InTerra.Interests
{
    /// <summary>
    /// Base interface for the various legal concepts around having an &quot;interest&quot; in land 
    /// </summary>
    public interface ILandPropertyInterest : ILegalConceptWithProperty<RealProperty>
    {
        new RealProperty SubjectProperty { get; set; }
        Func<ILegalPerson[], ILegalPerson> GetSubjectPerson { get; set; }
    }
}
