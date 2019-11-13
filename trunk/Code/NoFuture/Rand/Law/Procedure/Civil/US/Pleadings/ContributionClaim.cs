using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.Procedure.Civil.US.Pleadings
{
    /// <summary>
    /// Like <see cref="Crossclaim"/> except its against a party
    /// that the plaintiff did not add - the defendant is adding
    /// them directly.
    /// </summary>
    /// <remarks>
    /// Civil Procedure Rule 14(a)(1) - the link (a.k.a. edge) is
    /// between defendant and third party - there is no link
    /// between plaintiff and third party 
    /// </remarks>
    /// <remarks>
    /// 
    /// </remarks>
    [Aka("impleaded")]
    public class ContributionClaim : Crossclaim
    {
    }
}
