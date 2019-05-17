using System;

namespace NoFuture.Rand.Law
{
    /// <summary>
    /// To agree with proposition(s) often with enthusiasm
    /// </summary>
    public interface IAssent : IIntent
    {
        Predicate<ILegalPerson> IsApprovalExpressed { get; set; }
    }
}
