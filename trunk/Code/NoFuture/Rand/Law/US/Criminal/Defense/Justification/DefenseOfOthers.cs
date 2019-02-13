using System;

namespace NoFuture.Rand.Law.US.Criminal.Defense.Justification
{
    /// <inheritdoc />
    /// <summary>
    /// <![CDATA[
    /// under the circumstances as the actor believes them to be, the 
    /// person whom he seeks to protect would be justified in using 
    /// such protective force (Model Penal Code § 3.05(1) (b))
    /// ]]>
    /// </summary>
    public class DefenseOfOthers : DefenseOfSelf
    {
        public DefenseOfOthers(ICrime crime) : base(crime)
        {
        }
    }
}
