using System;

namespace NoFuture.Rand.Law
{
    /// <summary>
    /// A form of agreement that is an act of the understanding
    /// </summary>
    public interface IAssent : IIntent
    {
        Predicate<ILegalPerson> IsApprovalExpressed { get; set; }
    }
}
