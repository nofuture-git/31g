using System;

namespace NoFuture.Law
{
    /// <summary>
    /// The direct antecedent which caused the effect - the effect which exist only because of it.
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// taken in reverse to absurdity, "he pushed him off the cliff, but he
    /// did not kill him... he was alive after being pushed ... he
    /// died because of the rocks at the bottom..."
    /// ]]>
    /// </remarks>
    public interface IFactualCause<T> : ILegalConcept where T: IRationale
    {
    }
}
