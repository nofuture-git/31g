using NoFuture.Law.Attributes;

namespace NoFuture.Law.Contract.US
{
    /// <inheritdoc />
    /// <summary>
    /// A binding of a person to a expected performance
    /// </summary>
    [Note("expected commitment will be done")]
    public abstract class Promise : LegalConcept
    {
        public override bool IsEnforceableInCourt => true;
    }
}
