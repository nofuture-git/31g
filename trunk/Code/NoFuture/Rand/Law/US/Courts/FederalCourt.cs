using NoFuture.Rand.Core;

namespace NoFuture.Rand.Law.US.Courts
{
    /// <summary>
    /// Limited subject matter jurisdiction as defined
    /// in Article III, Section 2 of the Constitution -
    /// otherwise it remains with the states.
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// Cases limited to:
    /// * between states
    /// * between citizens of different states
    /// * between citizens and aliens
    /// * involving foreign ministers and consuls
    /// * admiralty & maritime
    /// * arising from federal law or federal Constitution
    /// ]]>
    /// </remarks>
    public class FederalCourt : VocaBase, ICourt
    {
    }
}
