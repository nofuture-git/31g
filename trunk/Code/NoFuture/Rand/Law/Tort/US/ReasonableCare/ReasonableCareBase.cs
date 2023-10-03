using System;
using NoFuture.Law.Attributes;
using NoFuture.Law.US;

namespace NoFuture.Law.Tort.US.ReasonableCare
{
    /// <summary>
    /// Abstract idea or caution of the hypothetical reasonable person
    /// (i.e. a person or ordinary intelligence and prudence).
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// The law takes no account of the infinite varieties of
    /// temperament, intellect, and education which make the
    /// internal character of a given act so different in
    /// different men.
    /// When a man has a distinct defect of such a nature that
    /// all can recognize it as making certain precautions
    /// impossible, he will not be held answerable for not taking them.
    /// OLIVER WENDELL HOLMES, JR., THE COMMON LAW 107-09 (1881)
    /// ]]>
    /// </remarks>
    [Aka("ordinary care", "proper care")]
    public abstract class ReasonableCareBase : UnoHomine, IIntent
    {
        protected ReasonableCareBase(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }
    }
}
